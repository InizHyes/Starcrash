using System.Collections;
using UnityEngine;

public class shootingScript : MonoBehaviour
{
    [SerializeField]
    private GameObject reloadText;

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
    public int damagePerHit; //not implemented, change public damage int on corresponding bullet (attached to shootingScript)

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

    [SerializeField]
    private HapticEffectSO gunShotRumble;

    private bool finishedReload = true;

    private bool noMag;

    private bool reloadStarted;

    public bool playMuzzleSmoke;

    private Vector3 playerPos;

    Vector2 ForceDir; //Part of Sean's recoil scripting

    //private PlayerController Player;

    private Player newPlayer;

    private SFX gunSound;

    private void Start()
    {
        
    }

    private void Awake()
    {
        
        //Player = GetComponentInParent<PlayerController>();
        newPlayer = GetComponentInParent<Player>();

        gunSound = GetComponent<SFX>();

        playerPos = newPlayer.gameObject.transform.position;

        reloadText.SetActive(false);

        ammoLoaded = magSize;
        reloadStarted = false;
        noMag = false;
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
            if(reloadStarted)
            {
                StartCoroutine(Reload());
            }
            reloadText.SetActive(true);
            return;

        }
        else if (ammoLoaded > 0)
        {
            
            if (ammoLoaded == (magSize))
            {
                gunSound.maxPitch = 1f;
                gunSound.minPitch = 1f;
            }
            else if (ammoLoaded == (magSize / 2))
            {
                gunSound.maxPitch = 1.1f;
                gunSound.minPitch = 1.1f;
            }
            else if (ammoLoaded == (magSize / 4))
            {
                gunSound.maxPitch = 1.2f;
                gunSound.minPitch = 1.2f;
            }
            else if (ammoLoaded == (1))
            {
                gunSound.maxPitch = 1.3f;
                gunSound.minPitch = 1.3f;
            }


            if (Time.time > readyToShoot)
            {
                reloadText.SetActive(false);
                if (ShootInput)
                {
                    FireBullet();
                    //HapticManager.PlayEffect(gunShotRumble, newPlayer.gameObject.transform.position);
                }

            }

        }
        else
        {
            StartCoroutine(Reload());
        }
        /*if (ReloadInput)
        {

            StartCoroutine(Reload());
            return;

        }*/

    }

    private void FireBullet() // called every time fire is pressed - Arch
    {
        ammoLoaded -= 1;
        ForceDir = newPlayer.shootDirection;
        newPlayer.rb.AddForce(-ForceDir * recoilPower, ForceMode2D.Impulse);
        gunSound.PlaySound("Gun Shot");

        for (int i = 0; i < numberOfBullets; i++)
        {
            
            
            //Player.ForceToApply = (ForceDir * recoilPower * -1.0f); //Part of Sean's recoil scripting         
            
            
            
            playMuzzleSmoke = true;
            GameObject firedBullet = Instantiate(bullet, gunPoint.position, gunPoint.rotation); //creates an instance of bullet at the position of the "gun" - Arch
            Vector2 bulletDir = gunPoint.right;
            Vector2 spreader = Vector2.Perpendicular(bulletDir) * Random.Range(-spread, spread);
            firedBullet.GetComponent<Rigidbody2D>().velocity = (bulletDir + spreader) * bulletSpeed; //adds force to the bullet - Arch
            
        }
        readyToShoot = Time.time + (1 / fireRate);
    }

    private IEnumerator Reload()
    {
        startReload();

        yield return new WaitForSeconds(reloadTime);
        
        if (Time.time > readyToShoot)
        {
            finishReloading();
            finishedReload = true;
        }

    }

    private void startReload()
    {
        if (!reloadStarted)
        {
            gunSound.PlaySound("Unload");
            readyToShoot = Time.time + (reload.length);
        }

        reloadStarted = true;

        finishedReload = false;

        noMag = true;

        
    }

    private void finishReloading()
    {

        ammoLoaded = magSize;

        newPlayer.reloadTriggered = false;

        reloadStarted = false;

        ammoReserve -= (magSize - ammoLoaded);

        if (noMag) { gunSound.PlaySound("Reload"); noMag = false; }

    }
}

