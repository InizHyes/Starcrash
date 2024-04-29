using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RogueScientistFlanks : MonoBehaviour
{
    // This enemy is a simple turret that, when active, fires bullets upwards.

    public GameObject origTurretRef;
    public GameObject sideTurretRef;
    private GameObject currentDestination;
    public bool activated = false;
    bool forward = true;
    private float speed = 0.05f;
    public GameObject bulletPrefab;
    private float bulletSpeed = 4f;
    private int atkCooldown = 0;
    private int stopCooldown = 0;
    public int moves = 6; // the amount of moves back and forth the turrets fire for
    private int movecount = 0;
    public bool theLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        currentDestination = sideTurretRef;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentDestination.transform.position, speed);
        if (theLeft)
        {
            if (forward)
            {
                transform.Rotate(0, 0, 3);
            }
            else
            {
                transform.Rotate(0, 0, -3);
            }
        }
        else
        {
            if (forward)
            {
                transform.Rotate(0, 0, -3);
            }
            else
            {
                transform.Rotate(0, 0, 3);
            }
        }
        
        
        if (transform.position == currentDestination.transform.position)
        {
            if (forward)
            {
                currentDestination = origTurretRef;
                forward = false;
                if (activated)
                {
                    movecount = movecount + 1;
                }
            }
            else
            {
                currentDestination = sideTurretRef;
                forward = true;
                if (activated)
                {
                    movecount = movecount + 1;
                }
            }
            
            

        }
        if (activated)
        {
            if (movecount < moves)
            {
                shoot();
            }
            else
            {
                activated = false;
                movecount = 0;
            }
            /*
            if (stopCooldown > bulletamount)
            {
                activated = false;
                stopCooldown = 0;
            }
            else
            {
                stopCooldown += 1;
                shoot();
            }
            
            else
            {
                if (atkCooldown > 1)
                {
                    shoot();
                    atkCooldown = 0;
                }
                else
                {
                    atkCooldown += 1;
                    stopCooldown += 1;
                }
            }
            */

        }
        else
        {
            movecount = 0;
        }
    }
    private void shoot()
    {
        GameObject firedBullet = Instantiate(bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
        firedBullet.transform.localScale = new Vector2(1f, 0.5f);
        Vector2 bulletDir = transform.right;
        firedBullet.GetComponent<Rigidbody2D>().velocity = bulletDir * (bulletSpeed + Random.Range(0.2f, -0.2f));
        SpriteRenderer bulletRenderer = firedBullet.GetComponent<SpriteRenderer>();
        firedBullet.GetComponent<Bullet>().damage = 3;
        bulletRenderer.color = Color.cyan;
        firedBullet.GetComponent<Light2D>().color = Color.cyan;
    }
}
