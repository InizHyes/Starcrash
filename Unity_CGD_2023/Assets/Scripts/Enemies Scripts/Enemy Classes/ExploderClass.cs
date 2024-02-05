using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Exploder : EnemyClass
{
    [Header("Exploder Specific")]
    private ExploderAOE exploderAOE;
    [SerializeField][Tooltip("The higher the number the weaker the slow down on collision")] private float slowDown = 0.2f;

    private void Start()
    {
        // Set starting state and variables
        initiateEnemy();

        exploderAOE = GetComponentInChildren<ExploderAOE>();
        exploderAOE.gameObject.SetActive(false);
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
                /*
                Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;
                */
                break;

            case State.Attacking:
                /*
                 * Change State to here after attack is used
                 * Will wait here until attackCooldown is over then move back to Targeting
                 * 
                 * Before setting state to State.Attacking run //attackCooldownValue = attackCooldown;
                 * This will set the attackCooldownValue so that attackCooldwonLogic() can count it down
                 */

                // Slow down momentum
                rb.velocity -= rb.velocity / 0.2f * Time.deltaTime;

                break;

            case State.Dead:
                /*
                 * Runs item drop logic then runs the logic associated with the enemy leaving the scene
                 * Can run death animation before running these functions
                 * 
                 * Set by AOE
                 * Lingers for a while
                 */

                itemDropLogic();
                initiateDeath();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Wall detection
        if (collision.gameObject.tag == "OuterWall")
        {
            rb.velocity = Vector2.zero;
            moveForce = Vector2.zero;
        }
    }

    public override bool playerCollisionCheck(Collider2D collider)
    {
        /*
         * Call in CollisionEnter2D()
         * Checks if the collision is the player
         * Deals damage to the player based on bump attack value
         * 
         * Modified to explode instead
         */

        if (collider.gameObject.tag == "Player")
        {
            if (enemyState == State.Moving)
            {
                // Set AOE active
                exploderAOE.gameObject.SetActive(true);
                enemyState = State.Attacking;

                return true;
            }
        }

        return false;
    }
}
