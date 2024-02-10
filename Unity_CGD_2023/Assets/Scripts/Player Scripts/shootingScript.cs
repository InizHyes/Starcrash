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
        totalAmmoHeld = ammoLoaded + ammoReserve;
        maximumAmmoPickup = totalAmmoAllowed - totalAmmoHeld;
    }

    public void Shoot(int playerNum, bool controllerShoot)
    {
        totalAmmoHeld = ammoLoaded + ammoReserve;
        maximumAmmoPickup = totalAmmoAllowed - totalAmmoHeld;

        if (playerNum == 1)
        {
            totalAmmoHeld = ammoLoaded + ammoReserve;
            maximumAmmoPickup = totalAmmoAllowed - totalAmmoHeld;
            if (Time.time > readyToShoot)
            {

                if (ammoLoaded > 0)
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

                    }
                }

                else if (ammoLoaded <= 0)
                {
                    //print("empty");

                    if (ammoReserve > 0)
                    {
                        //print("unloaded");
                        ammoLoaded = magSize;
                        ammoReserve -= magSize;
                        readyToShoot = Time.time + 1 * reloadTime;
                        audio.clip = unload;
                        audio.Play();
                        audio.clip = reload;
                        finishReload = (Time.time + (1 * reloadTime)) - audio.clip.length / 3;
                    }
                    else if (ammoReserve < 0)
                    {
                        //print("unloaded");
                        ammoLoaded = magSize;
                        ammoReserve -= magSize;
                        readyToShoot = Time.time + 1 * reloadTime;
                        audio.clip = unload;
                        audio.Play();
                        audio.clip = reload;
                        finishReload = (Time.time + (1 * reloadTime)) - audio.clip.length / 3;
                    }
                    /*if (Time.time > finishReload)
                    {
                        print("reloaded");
                        audio.Play();

                    }*/
                }
            }
        }

        if (playerNum == 2)
        {
            totalAmmoHeld = ammoLoaded + ammoReserve;
            maximumAmmoPickup = totalAmmoAllowed - totalAmmoHeld;
            if (Time.time > readyToShoot)
            {
                if (ammoLoaded > 0)
                {
                    if (controllerShoot)
                    {
                        ForceDir = Player.gameObject.transform.right;//Part of Sean's recoil scripting
                        readyToShoot = Time.time + 1 / fireRate;
                        FireBullet();
                        Player.ForceToApply = (ForceDir * recoilPower * -1.0f); //Part of Sean's recoil scripting         
                        audio.clip = gunShot;
                        audio.Play();
                        ammoLoaded -= 1;
                    }
                }

                else if (ammoLoaded <= 0)
                {
                    if (ammoReserve > 0)
                    {
                        /*if (Time.time > finishReload)
                        {
                            audio.Play();
                            finishReload = 0;

                        }*/
                        readyToShoot = Time.time + 1 * reloadTime;
                        audio.clip = unload;
                        audio.Play();
                        audio.clip = reload;
                        ammoLoaded = magSize;
                        ammoReserve -= magSize;
                        /*if(finishReload == 0)
                        {
                            finishReload = Time.time - (audio.clip.length / 3);
                        }*/

                    }
                    else if (ammoReserve < 0)
                    {
                        /*if (Time.time > finishReload)
                        {
                            audio.Play();
                            finishReload = 0;

                        }*/
                        readyToShoot = Time.time + 1 * reloadTime;
                        audio.clip = unload;
                        audio.Play();
                        audio.clip = reload;
                        ammoLoaded = magSize;
                        ammoReserve -= magSize;
                        /*if(finishReload == 0)
                        {
                            finishReload = Time.time - (audio.clip.length / 3);
                        }*/

                    }
                }
                /*if (ammoLoaded <= 0)
                {
                    if (ammoReserve > 0)
                    {
                        readyToShoot = Time.time + 1 / reloadTime;
                        audio.clip = unload;
                        audio.Play();
                        audio.clip = reload;
                        finishReload = readyToShoot - audio.clip.length;
                        ammoLoaded = magSize;
                        ammoReserve -= magSize;
                    }
                }*/
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