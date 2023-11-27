using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingScript : MonoBehaviour
{
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
    private int magSize;

    [SerializeField]
    private int maxAmmoReserves;

    [SerializeField]
    public int ammoReserve;

    [SerializeField]
    public int ammoLoaded;

    [SerializeField]
    public int totalAmmoAllowed;

    [SerializeField]
    public int totalAmmoHeld;

    [SerializeField]
    private float reloadTime;

    Vector2 MousePos; //Part of Sean's recoil scripting
    Vector2 PlayerPos; //Part of Sean's recoil scripting
    Vector2 ForceDir; //Part of Sean's recoil scripting

    private PlayerController Player;

    private void Awake()
    {
        Player = GetComponentInParent<PlayerController>();
        ammoLoaded = magSize;
        totalAmmoAllowed = magSize + maxAmmoReserves;
    }

    public void Shoot(int playerNum, bool controllerShoot)
    {
        if (playerNum == 1)
        {
            if (Input.GetMouseButton(0))
            {
                if (ammoLoaded > 0)
                {
                    if (Time.time > readyToShoot)
                    {
                        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//Part of Sean's recoil scripting
                        PlayerPos = Player.gameObject.transform.position;
                        ForceDir = (MousePos - PlayerPos).normalized;//Part of Sean's recoil scripting
                        readyToShoot = Time.time + 1 / fireRate;
                        FireBullet();
                        Player.ForceToApply = (ForceDir * recoilPower * -1.0f); //Part of Sean's recoil scripting         
                        ammoLoaded -= 1;
                        totalAmmoHeld = ammoLoaded + ammoReserve;
                    }
                }

                else if (ammoLoaded <= 0)
                {
                    if (ammoReserve > 0)
                    {
                        if (Time.time > readyToShoot)
                        {
                            readyToShoot = Time.time + 1 / reloadTime;
                            ammoLoaded = magSize;
                            ammoReserve -= magSize;
                        }
                    }
                }
            }
        }

        else if (playerNum == 2)
        {
            if (controllerShoot)
            {
                if (Time.time > readyToShoot)
                {
                    ForceDir = Player.gameObject.transform.right;
                    
                    readyToShoot = Time.time + 1 / fireRate;
                    FireBullet();
                    Player.ForceToApply = (ForceDir * recoilPower * -1.0f);
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