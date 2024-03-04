using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotgunTurretClass : EnemyClass
{
    // This is a repurposed laser turret script to instead shoot bullets.
    // This turret fires like a shotgun at the player.

    [Header("Laser Sniper Specific")]
    [SerializeField] private GameObject childLaser;
    [SerializeField] private int attackTimer = 1;
    public int laserDamage = 1; // This is public but should not be accessed outside of Laserdetection Script
    //bool laserReference = false;
    //private BoxCollider2D playerDetect;
    AudioSource sound;
    public AudioClip spawnsound;
    public AudioClip shootsound;
    public GameObject bulletPrefab;
    private float bulletSpeed = 5f;
    private Vector3 initialRot;
    private void Start()
    {
        // Set starting state and variables
        sound = GetComponent<AudioSource>();
        initiateEnemy();
        sound.clip = spawnsound;
        sound.Play();
        attackTimer = Random.Range(0, 18);
    }

    private void FixedUpdate()
    {
        switch (enemyState)
        {
            case State.Initiating:
                targetClosestPlayer();
                enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * This is where it would determine whether or not to spend time computating pathfinding
                 * It would be if(line of sight blocked){ enemyState = Pathfinding }
                 * But not needed now so im just assuming no LOS block
                 */

                enemyState = State.Moving;
                break;

            case State.Pathfinding:
                // Pathfind if line of sight is blocked
                break;

            case State.Moving:
                if (attackTimer > 59)
                {
                    attackTimer = 0;
                    enemyState = State.Attacking;
                }
                if (attackTimer < 60)
                {
                    attackTimer = attackTimer + 1;
                }

                Vector3 direction = target.transform.position - transform.position; // look at player
                transform.right = direction;
                break;

            case State.Attacking:
                // Accessing child
                Laserdetection script = childLaser.GetComponent<Laserdetection>();
                if (script != null)
                {
                    // Accessing child's variable

                    script.laserState = 1;
                    attackTimer = attackTimer + 1;
                    if (attackTimer > 40)
                    {
                        script.laserState = 0;
                        if (attackTimer == 41)
                        {
                            sound.clip = shootsound;
                            shoot();
                            sound.Play();
                        }
                        if (attackTimer > 42)
                        {
                            script.laserState = 0;
                            attackTimer = 0;
                            enemyState = State.Targeting;
                        }
                    }
                }

                else
                {
                    Debug.Log("laserenemy dont work");
                }

                break;

            case State.Dead:
                /*
                 * Runs item drop logic then runs the logic associated with the enemy leaving the scene
                 * Can run death animation before running these functions
                 */

                itemDropLogic();
                initiateDeath();
                break;
        }
    }

    private void shoot()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject firedBullet = Instantiate(bulletPrefab, childLaser.transform.position, childLaser.transform.rotation);
            Vector2 bulletDir = childLaser.transform.right;
            Vector2 spreader = Vector2.Perpendicular(bulletDir) * Random.Range(0.2f,-0.2f);
            firedBullet.GetComponent<Rigidbody2D>().velocity = (bulletDir + spreader) * bulletSpeed;
            firedBullet.transform.localScale = new Vector2(3, 5);
            SpriteRenderer bulletRenderer = firedBullet.GetComponent<SpriteRenderer>();
            bulletRenderer.color = Color.red;
        }

        //Vector2 spreader = Vector2.Perpendicular(bulletDir) * Random.Range(0.5f, 0.5f);
        //firedBullet.GetComponent<Rigidbody2D>().velocity = (bulletDir + spreader) * bulletSpeed;
    }  
}
