using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserSniperClass : EnemyClass
{
    public GameObject childLaser;
    public int atktimer = 1;
    bool laserReference = false;
    private BoxCollider2D playerDetect;


    void Start()
    {
        // Set starting state and variables
        enemyState = State.Initiating;
        health = 1;
        spawnLogic = NPCdeathCheck.GetComponent<SpawnLogic>();

        if (spawnLogic != null)
        {
            spawnLogic.NPCdeath();
        }


    }
    private void Update()
    {
        Debug.Log(enemyState);
        switch (enemyState)
        {
            case State.Initiating:
                targetClosestPlayer();
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
                if (atktimer > 199)
                {
                    atktimer = 0;
                    enemyState = State.Attacking;
                }
                if (atktimer < 200)
                {
                    atktimer = atktimer + 1;
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
                    atktimer = atktimer + 1;
                    if (atktimer > 250)
                    {
                        script.laserState = 2;
                        if (atktimer > 400)
                        {
                            script.laserState = 0;
                            atktimer = 1;
                            enemyState = State.Targeting;
                        }
                    }



                }
                
                else
                {
                    Debug.Log("laserenemy dont work");
                }




                break;

        }

        if (health == 0 && spawnLogic != null)
        {
            spawnLogic.NPCdeath();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
        }
    }


}
