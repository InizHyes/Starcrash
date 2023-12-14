using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RazorClass : EnemyClass
{
    /*
     * Template class
     * Use as a template for future enemy classes
     * Duplicate this and update the "public class TEMPLATECLASS : EnemyClass" to the new file name
     * By default it:
     * -Targets closest player on initiation
     * -Moves towards player with 0g physics (grunt movement)
     * -Rotates to face player
     * -Takes damage when hit by bullet
     * -Uses default values from EnemyClass
     * -Deals damage on collision with player (if the object has PlayerCollisioZone prefab as a child)
     */

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
                /*
                 * Starting state, used to run one-off functions for spawning
                 */

                enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * Target player and decide if State.Pathfinding is needed, otherwise change to moving
                 */

                targetClosestPlayer();
                enemyState = State.Moving;
                break;

            case State.Pathfinding:
                /*
                 * Pathfind if line of sight is blocked
                 */

                break;

            case State.Moving:
                /*
                * Move towards player with velocity
                * Will loop here until the state is changed back to Targeting, Attackng, or Dead
                */

                moveTowardsTarget0G();

                // look at player
                Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;
                break;

            case State.Attacking:
                /*
                 * Change State to here after attack is used
                 * Will wait here until attackCooldown is over then move back to Targeting
                 */

                // Count-down timer
                if (attackCooldwonLogic())
                {
                    enemyState = State.Targeting;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
         * Stop movement for a few seconds then do dash attack
         */
    }
}
