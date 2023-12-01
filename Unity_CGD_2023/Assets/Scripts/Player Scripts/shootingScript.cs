using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int magSize;

    [SerializeField]
    private int maxAmmoReserves;

    [SerializeField]
    public int totalAmmoAllowed;

    [SerializeField]
    public int totalAmmoHeld;

    [SerializeField]
    private float reloadTime;

    [SerializeField]
    private float maximumAmmoPickup;

    private float finishReload;
  

    Vector2 MousePos; //Part of Sean's recoil scripting
    Vector2 PlayerPos; //Part of Sean's recoil scripting
    Vector2 ForceDir; //Part of Sean's recoil scripting

    private PlayerController Player;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        Player = GetComponentInParent<PlayerController>();
        ammoLoaded = magSize;
        totalAmmoAllowed = magSize + maxAmmoReserves;
        maximumAmmoPickup = totalAmmoAllowed - totalAmmoHeld;
    }

    public void Shoot(int playerNum, bool controllerShoot)
    {
        if (playerNum == 1)
        {
            if (ammoLoaded > 0)
            {

                if (Time.time > readyToShoot)
                {
                    if (Input.GetMouseButton(0))
                    {
                        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//Part of Sean's recoil scripting
                        PlayerPos = Player.gameObject.transform.position;
                        ForceDir = (MousePos - PlayerPos).normalized;//Part of Sean's recoil scripting
                        readyToShoot = Time.time + 1 / fireRate;
                        FireBullet();
                        Player.ForceToApply = (ForceDir * recoilPower * -1.0f); //Part of Sean's recoil scripting         
                        audio.clip = gunShot;
                        audio.Play();
                        ammoLoaded -= 1;
                        totalAmmoHeld = ammoLoaded + ammoReserve;
                    }
                }
            }

            if (ammoLoaded <= 0)
            {
                if (ammoReserve > 0)
                {
                    if (Time.time > readyToShoot)
                    {
                        readyToShoot = Time.time + 1 * reloadTime;
                        audio.clip = reload;
                        audio.Play();
                        ammoLoaded = magSize;
                        ammoReserve -= magSize;
                    }
                }
            }
        }

        if (playerNum == 2)
        {
            if (controllerShoot)
            {
                if (ammoLoaded > 0)
                {
                    if (Time.time > readyToShoot)
                    {
                        ForceDir = Player.gameObject.transform.right;//Part of Sean's recoil scripting
                        readyToShoot = Time.time + 1 / fireRate;
                        FireBullet();
                        Player.ForceToApply = (ForceDir * recoilPower * -1.0f); //Part of Sean's recoil scripting         
                        audio.clip = gunShot;
                        audio.Play();
                        ammoLoaded -= 1;
                        totalAmmoHeld = ammoLoaded + ammoReserve;
                    }
                }
            }

            if (ammoLoaded <= 0)
            {
                if (ammoReserve > 0)
                {
                    if (Time.time > readyToShoot)
                    {
                        readyToShoot = Time.time + 1 / reloadTime;
                        audio.clip = reload;
                        audio.Play();
                        ammoLoaded = magSize;      
                        ammoReserve -= magSize;
                    }
                }
            }
        }
    }

    private void FireBullet() // called every time fire is pressed - Arch
    {
        for (int i=0; i<numberOfBullets; i++)
        {
            GameObject firedBullet = Instantiate(bullet, gunPoint.position, gunPoint.rotation); //creates an instance of bullet at the position of the "gun" - Arch
            Vector2 bulletDir = gunPoint.right;
            Vector2 spreader = Vector2.Perpendicular(bulletDir) * Random.Range(-spread, spread);
            firedBullet.GetComponent<Rigidbody2D>().velocity = (bulletDir + spreader) * bulletSpeed; //adds force to the bullet - Arch
        }
    }


}