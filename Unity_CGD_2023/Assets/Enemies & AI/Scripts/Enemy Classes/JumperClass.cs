using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumperClass : EnemyClass
{
    void Start()
    {
        // Set starting state and variables
        initiateEnemy(10);
    }

    private void Update()
    {
        //Debug.Log(enemyState);
        switch (enemyState)
        {
            case State.Initiating:
                /*
                 * Starting state, used to run one-off functions for spawning
                 */

                enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * This is where it would determine whether or not to spend time computating pathfinding
                 * It would be if(line of sight blocked){ enemyState = Pathfinding }
                 * But not needed now so im just assuming no LOS block
                 */

                targetClosestPlayer();
                enemyState = State.Moving;
                break;

            case State.Pathfinding:
                // Pathfind if line of sight is blocked

                break;

            case State.Moving:
                /*
                * Move towards player with velocity
                * Maybe check if near to attack, maybe just change state on collision
                */

                moveTowardsTarget0G();

                // look at player
                Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;
                break;

            case State.Attacking:
                break;
        }

        // Check if dead (might move to function and call in Moving and Attacking
        if (health == 0 && spawnLogic != null)
        {
            spawnLogic.NPCdeath();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage detection
        damageDetection(collision);
    }
}
