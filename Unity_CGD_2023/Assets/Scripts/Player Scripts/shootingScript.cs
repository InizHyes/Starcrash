using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    private AudioClip dryFire;

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

    [SerializeField]
    private GameObject muzzleLight;

    private int tempAmmoAddedTracker = 0;

    private bool untilFull = false;

    public void emptyOnSwitchReset()
    {
        untilFull = false;
        reloadStarted = false;
        newPlayer.reloadTriggered = false;
        ammoLoaded = magSize;
        readyToShoot = Time.time;
        tempAmmoAddedTracker = 0;
    }

    private void Start()
    {
        
    }

    private void Awake()
    {
        
        //Player = GetComponentInParent<PlayerController>();
        newPlayer = GetComponentInParent<Player>();

        gunSound = GetComponent<SFX>();

        muzzleLight.SetActive(false);

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

        /*
        if (!finishedReload)
        {
            if(reloadStarted)
            {
                StartCoroutine(Reload());
            }
            reloadText.SetActive(true);
            return;

        }*/





        if (!untilFull)
        {
            
            if (!ShootInput && ammoLoaded < magSize)
            {
                StartCoroutine(Reloading());
            }

            if (ammoLoaded == 0)
            {
                untilFull = true;
                StartCoroutine(Reloading());
            }
            else if (ShootInput && ammoLoaded == 0)
            {
                untilFull = true;
                StartCoroutine(Reloading());
                /*
                gunSound.maxPitch = 2f;
                gunSound.minPitch = 2f;
                if (Time.time > readyToShoot)
                {
                    if (ShootInput)
                    {
                        tempAmmoAddedTracker = 0;
                        readyToShoot = Time.time + 0.2f;//(reload.length);
                        gunSound.PlaySound("Empty");
                    }
                }*/
            }

            if (ammoLoaded > 0)
            {


                if (ammoLoaded == (1))
                {
                    gunSound.maxPitch = 1.3f;
                    gunSound.minPitch = 1.3f;
                }
                else if (ammoLoaded < (magSize / 4))
                {
                    gunSound.maxPitch = 1.2f;
                    gunSound.minPitch = 1.2f;
                }
                else if (ammoLoaded < (magSize / 2))
                {
                    gunSound.maxPitch = 1.1f;
                    gunSound.minPitch = 1.1f;
                }
                else
                {
                    gunSound.maxPitch = 1f;
                    gunSound.minPitch = 1f;
                }

                if (Time.time > readyToShoot)
                {
                    muzzleLight.SetActive(false);
                    reloadText.SetActive(false);
                    if (ShootInput)
                    {
                        tempAmmoAddedTracker = 0;
                        newPlayer.reloadTriggered = false;
                        muzzleFlashStartEffect();
                        FireBullet();
                        //HapticManager.PlayEffect(gunShotRumble, newPlayer.gameObject.transform.position);
                    }

                }

            }
        }
        else
        {
            StartCoroutine(Reloading());
            if (ammoLoaded == magSize)
            {
                untilFull = false;
            }
        }

        /*  else if (ammoLoaded == 0)
                 {
                     StartCoroutine (Reloading());
                 }
                 else
                 {
                     StartCoroutine(Reload());
                 }
                 if (ReloadInput)
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
        gunSound.volume = 0.25f;
        gunSound.PlaySound("Gun Shot");
        
        for (int i = 0; i < numberOfBullets; i++)
        {       
            playMuzzleSmoke = true;
            GameObject firedBullet = Instantiate(bullet, gunPoint.position, gunPoint.rotation); //creates an instance of bullet at the position of the "gun" - Arch
            Vector2 bulletDir = gunPoint.right;
            Vector2 spreader = Vector2.Perpendicular(bulletDir) * Random.Range(-spread, spread);
            firedBullet.GetComponent<Rigidbody2D>().velocity = (bulletDir + spreader) * bulletSpeed; //adds force to the bullet - Arch  
        }
        readyToShoot = Time.time + (1 / fireRate);
    }

    private IEnumerator Reloading()
    {
        reloadText.SetActive(true);

        if (Time.time < readyToShoot)
        {
            yield return null;
        }
        else
        {
            if (ammoLoaded == magSize)
            {
                newPlayer.reloadTriggered = false;
                tempAmmoAddedTracker = 0;
                yield return null;
            }
            else if (untilFull)
            {
                reloadSequence();

            }
            else
            {
                reloadSequence();
            }

        }
    }

    private IEnumerator Reload()
    {
        //startReload();

        yield return new WaitForSeconds(reloadTime);
        
        if (Time.time > readyToShoot)
        {
            //finishReloading();
            StartCoroutine(Reloading());
            //finishedReload = true;
        }


    }

    private void startReload()
    {
        if (!reloadStarted)
        {
            gunSound.maxPitch = 1f;
            gunSound.minPitch = 1f;
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

        if (noMag) 
        {
            gunSound.maxPitch = 1f;
            gunSound.minPitch = 1f;
            gunSound.PlaySound("Reload"); 
            noMag = false; 
        }

    }

    private void muzzleFlashStartEffect()
    {
        muzzleLight.SetActive(true);
        Invoke("muzzleFlashEndEffect", 0.02f);
    }

    private void muzzleFlashEndEffect()
    {
        muzzleLight.SetActive(false);
    }

    private void reloadSequence()
    {
        newPlayer.reloadTriggered = true;

        if (gunSound.maxPitch == 3f)
        {
            gunSound.maxPitch = 3f;
            gunSound.minPitch = 3f;
        }
        else
        {
            gunSound.maxPitch = (1f + (tempAmmoAddedTracker / 10));
            gunSound.minPitch = (1f + (tempAmmoAddedTracker / 10));
        }

        if (untilFull)
        {
            if (tempAmmoAddedTracker > magSize / 4)
            {
                readyToShoot = Time.time + (reloadTime / 2);
                if ((ammoLoaded + 1) > magSize)
                {
                    ammoLoaded += magSize - ammoLoaded;
                    tempAmmoAddedTracker += magSize - ammoLoaded;
                    gunSound.maxPitch = 1f;
                    gunSound.minPitch = 1f;
                    gunSound.PlaySound("Reload");
                }
                else
                {
                    if ((ammoLoaded + 1) == magSize)
                    {
                        gunSound.maxPitch = 1f;
                        gunSound.minPitch = 1f;
                        gunSound.PlaySound("Reload");
                    }
                    else
                    {
                        gunSound.PlaySound("Reload");
                    }
                    ammoLoaded += 1;
                    tempAmmoAddedTracker += 1;
                }
            }
            else if (tempAmmoAddedTracker > (magSize / 2))
            {
                readyToShoot = Time.time + (reloadTime / 4);
                if ((ammoLoaded + 2) > magSize)
                {
                    ammoLoaded += magSize - ammoLoaded;
                    tempAmmoAddedTracker += magSize - ammoLoaded;
                    gunSound.maxPitch = 2f;
                    gunSound.minPitch = 2f;
                    gunSound.PlaySound("Reload");
                }
                else
                {
                    if ((ammoLoaded + 2) == magSize)
                    {
                        gunSound.maxPitch = 1f;
                        gunSound.minPitch = 1f;
                        gunSound.PlaySound("Reload");
                    }
                    else
                    {

                        gunSound.PlaySound("Reload");
                    }
                    ammoLoaded += 2;
                    tempAmmoAddedTracker += 1;

                }

            }
            else
            {
                readyToShoot = Time.time + (reloadTime);
                if ((ammoLoaded + 1) > magSize)
                {
                    ammoLoaded += magSize - ammoLoaded;
                    tempAmmoAddedTracker += magSize - ammoLoaded;
                    gunSound.maxPitch = 1f;
                    gunSound.minPitch = 1f;
                    gunSound.PlaySound("Reload");
                }
                else
                {
                    if ((ammoLoaded + 1) == magSize)
                    {
                        gunSound.maxPitch = 1f;
                        gunSound.minPitch = 1f;
                        gunSound.PlaySound("Reload");
                    }
                    else
                    {
                        gunSound.PlaySound("Reload");
                    }
                    ammoLoaded += 1;
                    tempAmmoAddedTracker += 1;
                }

            }
        }
        else if (!untilFull)
        {
            if (tempAmmoAddedTracker > magSize / 4)
            {
                readyToShoot = Time.time + (reloadTime / 2);
                if ((ammoLoaded + 1) > magSize)
                {
                    ammoLoaded += magSize - ammoLoaded;
                    tempAmmoAddedTracker += magSize - ammoLoaded;
                    gunSound.maxPitch = 1f;
                    gunSound.minPitch = 1f;
                    gunSound.PlaySound("Reload");
                }
                else
                {
                    if ((ammoLoaded + 1) == magSize)
                    {
                        gunSound.maxPitch = 1f;
                        gunSound.minPitch = 1f;
                        gunSound.PlaySound("Reload");
                    }
                    else
                    {
                        gunSound.PlaySound("Reload");
                    }
                    ammoLoaded += 1;
                    tempAmmoAddedTracker += 1;
                }
            }
            else if (tempAmmoAddedTracker > magSize / 2)
            {
                readyToShoot = Time.time + (reloadTime / 4);
                if ((ammoLoaded + 2) > magSize)
                {
                    ammoLoaded += magSize - ammoLoaded;
                    tempAmmoAddedTracker += magSize - ammoLoaded;
                    gunSound.maxPitch = 1f;
                    gunSound.minPitch = 1f;
                    gunSound.PlaySound("Reload");
                }
                else
                {
                    if ((ammoLoaded + 2) == magSize)
                    {
                        gunSound.maxPitch = 1f;
                        gunSound.minPitch = 1f;
                        gunSound.PlaySound("Reload");
                    }
                    else
                    {
                        gunSound.PlaySound("Reload");
                    }
                    ammoLoaded += 2;
                    tempAmmoAddedTracker += 1;

                }

            }
            else
            {
                readyToShoot = Time.time + (reloadTime);
                if ((ammoLoaded + 1) > magSize)
                {
                    ammoLoaded += magSize - ammoLoaded;
                    tempAmmoAddedTracker += magSize - ammoLoaded;
                    gunSound.maxPitch = 1f;
                    gunSound.minPitch = 1f;
                    gunSound.PlaySound("Reload");
                }
                else
                {
                    if ((ammoLoaded + 1) == magSize)
                    {
                        gunSound.maxPitch = 1f;
                        gunSound.minPitch = 1f;
                        gunSound.PlaySound("Reload");
                    }
                    else
                    {

                        gunSound.PlaySound("Reload");
                    }
                    ammoLoaded += 1;
                    tempAmmoAddedTracker += 1;
                }

            }
        }

    }


}

