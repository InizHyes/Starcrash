using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserSniperClass : EnemyClass
{
    [Header("Laser Sniper Specific")]
    [SerializeField] private GameObject childLaser;
    public int attackTimer = 1;
    public bool bossturret = false;
    public bool activate = true;
    public float laserDamage = 1; // This is public but should not be accessed outside of Laserdetection Script
    int bossModifier = 1;
    //bool laserReference = false;
    //private BoxCollider2D playerDetect;
    
    
    private void Start()
    {
        // Set starting state and variables

        initiateEnemy();
        if (!bossturret)
        {
            activate = true;
        }
        else
        {
            attackTimer = 0;
            bossModifier = 50;
        }

    }

    private void Update()
    {
        switch (enemyState)
        {
            case State.Initiating:
                if (!bossturret)
                {
                    targetClosestPlayer();
                }
                //enemyState = State.Targeting;
                changestate(1);
                break;

            case State.Targeting:
                /*
                 * This is where it would determine whether or not to spend time computating pathfinding
                 * It would be if(line of sight blocked){ enemyState = Pathfinding }
                 * But not needed now so im just assuming no LOS block
                 */

                //enemyState = State.Moving;
                if (activate)
                {
                    if (bossturret)
                    {
                        changestate(4);
                    }
                    else
                    {
                        changestate(3);
                    }
                }
                break;

            case State.Pathfinding:
                // Pathfind if line of sight is blocked
                break;

            case State.Moving:
                if (bossturret)
                {
                    attackTimer = 0; //negates recharging if a boss turret
                }
                if (attackTimer > 199)
                {
                    attackTimer = 0;
                    //enemyState = State.Attacking;
                    changestate(4);
                }
                if (attackTimer < 200)
                {
                    attackTimer = attackTimer + 1;
                }

                if (target != null)
                {
                    if (!bossturret)
                    {
                        Vector3 direction = target.transform.position - transform.position; // look at player
                        transform.right = direction;
                    }
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
                            //enemyState = State.Targeting;
                            changestate(1);
                            if (!bossturret)
                            {
                                targetClosestPlayer();
                            }
                            else
                            {
                                activate = false;
                            }
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

                //itemDropLogic();
                //initiateDeath();
                break;
        }
    }
}
