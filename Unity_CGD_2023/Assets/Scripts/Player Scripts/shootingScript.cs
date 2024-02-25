using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class shootingScript : MonoBehaviour
{
    AudioSource audio;

    [SerializeField]
    private AudioClip gunShot;

    [SerializeField]
    private AudioClip unload;

    [SerializeField]
    private AudioClip reload;

    [SerializeField]
    private Transform gunPoint;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float fireRate;

    [SerializeField]
    private float readyToShoot;

    [SerializeField]
    private float recoilPower; //used to be "GunForce" in Sean's player controller script - Part of Sean's recoil scripting

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private int numberOfBullets;

    [SerializeField]
    private float spread;

    [SerializeField]
    public int ammoLoaded;

    [SerializeField]
    public int ammoReserve;

    [SerializeField]
    public int magSize;

    [SerializeField]
    private int maxAmmoReserves;

    [SerializeField]
    public int totalAmmoAllowed;

    [SerializeField]
    public int totalAmmoHeld;

    [SerializeField]
    private float reloadTime;

    [SerializeField]
    public int maximumAmmoPickup;

    private bool finishedReload = true;

    public bool playMuzzleSmoke;

    Vector2 ForceDir; //Part of Sean's recoil scripting

    //private PlayerController Player;

    private Player newPlayer;

    private void Start()
    {
        
    }

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        //Player = GetComponentInParent<PlayerController>();
        newPlayer = GetComponentInParent<Player>();

        ammoLoaded = magSize;
        totalAmmoAllowed = magSize + maxAmmoReserves;
        totalAmmoHeld = ammoLoaded + ammoReserve;
        maximumAmmoPickup = totalAmmoAllowed - totalAmmoHeld;
    }

    public void Shoot(bool ShootInput, bool ReloadInput)
    {
        totalAmmoHeld = ammoLoaded + ammoReserve;
        maximumAmmoPickup = totalAmmoAllowed - totalAmmoHeld;

        if (!finishedReload)
        {
            return;
        }

        if (ReloadInput)
        {
            //if (ammoReserve > 0)
            //{
            StartCoroutine(Reload());
            return;
            //}
        }

        if (ammoLoaded > 0)
        {
            if (ShootInput)
            {
                if (Time.time > readyToShoot)
                {
                    FireBullet();
                }

            }
        }

    }

    private void FireBullet() // called every time fire is pressed - Arch
    {
        ammoLoaded -= 1;
        
        for (int i = 0; i < numberOfBullets; i++)
        {
            ForceDir = newPlayer.shootDirection;
            readyToShoot = Time.time + 1 / fireRate;
            //Player.ForceToApply = (ForceDir * recoilPower * -1.0f); //Part of Sean's recoil scripting         
            newPlayer.rb.AddForce(-ForceDir * recoilPower, ForceMode2D.Impulse);
            GetComponent<SFX>().PlaySound("Gun Shot");
            playMuzzleSmoke = true;
            GameObject firedBullet = Instantiate(bullet, gunPoint.position, gunPoint.rotation); //creates an instance of bullet at the position of the "gun" - Arch
            Vector2 bulletDir = gunPoint.right;
            Vector2 spreader = Vector2.Perpendicular(bulletDir) * Random.Range(-spread, spread);
            firedBullet.GetComponent<Rigidbody2D>().velocity = (bulletDir + spreader) * bulletSpeed; //adds force to the bullet - Arch
        }
    }

    private IEnumerator Reload()
    {
        Debug.Log("Reload");

        GetComponent<SFX>().PlaySound("Unload");

        finishedReload = false;

        yield return new WaitForSeconds(reloadTime);

        GetComponent<SFX>().PlaySound("Reload");

        ammoLoaded = magSize;
        newPlayer.reloadTriggered = false;
        finishedReload = true;
        ammoReserve -= (magSize - ammoLoaded);
    }
}

