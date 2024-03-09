using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserSniperClass : EnemyClass
{
    [Header("Laser Sniper Specific")]
    [SerializeField] private GameObject childLaser;
    [SerializeField] private int attackTimer = 1;
    public int laserDamage = 1; // This is public but should not be accessed outside of Laserdetection Script
    //bool laserReference = false;
    //private BoxCollider2D playerDetect;
    
    
    private void Start()
    {
        // Set starting state and variables

        initiateEnemy();

    }

    private void Update()
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
                if (attackTimer > 199)
                {
                    attackTimer = 0;
                    enemyState = State.Attacking;
                }
                if (attackTimer < 200)
                {
                    attackTimer = attackTimer + 1;
                }

                if (target != null)
                {
                    Vector3 direction = target.transform.position - transform.position; // look at player
                    transform.right = direction;
                }
                break;

            case State.Attacking:
                // Accessing child
                Laserdetection script = childLaser.GetComponent<Laserdetection>();
                if (script != null)
                {
                    // Accessing child's variable

                    script.laserState = 1;
                    attackTimer = attackTimer + 1;
                    if (attackTimer > 250)
                    {
                        script.laserState = 2;
                        if (attackTimer == 251)
                        {
                            GetComponent<SFX>().PlaySound("");
                        }
                        if (attackTimer > 400)
                        {
                            script.laserState = 0;
                            attackTimer = 1;
                            enemyState = State.Targeting;
                            targetClosestPlayer();
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
}
