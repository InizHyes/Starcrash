using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumperClass : EnemyClass
{
    private float attackCooldown;
    public float ATTACKCOOLDOWN = 10f; // In seconds, can be set in inspector
    public int moveSpeed = 200;

    void Start()
    {
        // Set starting state and variables
        initiateEnemy(10);

        attackCooldown = 0f;
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

                pushTowardsPlayer();

                // Start attack cooldown
                attackCooldown = ATTACKCOOLDOWN;
                enemyState = State.Attacking;

                //moveTowardsTarget0G();

                /* //look at player
                Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;
                */
                break;

            case State.Attacking:
                // Count-down timer
                if (attackCooldown > 0f)
                {
                    attackCooldown -= Time.deltaTime;
                }
                // Change back to targeting/moving
                else
                {
                    enemyState = State.Targeting;
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collision with wall stops momentum
        if (enemyState == State.Attacking)
        {
            // Do not check for collision instantly
            if (collision.gameObject.tag == "OuterWall" && attackCooldown < ATTACKCOOLDOWN - 1)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }

        // Damage detection
        damageDetection(collision);
    }

    private void pushTowardsPlayer()
    {
        /*
         * Applies velocity in one large burst towards player
         */
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 playerDirection = (target.transform.position - this.transform.position).normalized;
        rb.AddForce(playerDirection * moveSpeed);
    }
}
