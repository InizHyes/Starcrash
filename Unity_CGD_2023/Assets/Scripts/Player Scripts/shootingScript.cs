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
    private float bulletSpeed;

    [SerializeField]
    private int numberOfBullets;

    [SerializeField]
    private float spread;


    public void shoot(int playerNum, bool controllerShoot)
    {
        if(playerNum == 1)
        {
            if(Input.GetMouseButton(0))
            {
                if(Time.time > readyToShoot)
                {
                    readyToShoot = Time.time + 1/fireRate;
                    FireBullet();
                }
                
            }
        }
        else if(playerNum == 2)
        {
            if (controllerShoot)
            {
                if (Time.time > readyToShoot)
                {
                    readyToShoot = Time.time + 1 / fireRate;
                    FireBullet();
                }
            }
        }
    }

    private void FireBullet() // called every time fire is pressed - Arch
    {
        for(int i=0; i<numberOfBullets; i++)
        {
            GameObject firedBullet = Instantiate(bullet, gunPoint.position, gunPoint.rotation); //creates an instance of bullet at the position of the "gun" - Arch
            Vector2 bulletDir = gunPoint.right;
            Vector2 spreader = Vector2.Perpendicular(bulletDir) * Random.Range(-spread, spread);
            firedBullet.GetComponent<Rigidbody2D>().velocity = (bulletDir + spreader) * bulletSpeed; //adds force to the bullet - Arch
        }
    }
}