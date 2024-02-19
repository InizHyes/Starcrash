using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RazorClass : EnemyClass
{
    [Header("Razor Specific")]
    [SerializeField] private int dashSpeed = 200;
    [SerializeField] private RazorBlade razorBlade;

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
                 * USED AS WAIT TIME, not pathfinding
                 * Using collision with wall as exit time
                 * 
                 * //Using attackCooldwonLogic() as a timer
                 * //Waits as long as attackCooldown
                 */

                /*
                if (attackCooldwonLogic())
                {
                    enemyState = State.Targeting;
                } 
                */

                break;

            case State.Moving:
                /*
                * Move towards player with velocity
                * Will loop here until the state is changed back to Targeting, Attackng, or Dead
                */

                moveTowardsTarget0G();

                // Slow down razor to default
                if (razorBlade.spinSpeed >= razorBlade.DEFAULTSPINSPEED)
                {
                    razorBlade.spinSpeed -= razorBlade.spinSpeed / 100;
                }

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
                    pushTowardsTarget();

                    // Use pathfinding to wait
                    attackCooldownValue = attackCooldown;
                    enemyState = State.Pathfinding;
                }

                // Speed up razor to max
                if (razorBlade.spinSpeed <= razorBlade.maxSpinSpeed)
                {
                    razorBlade.spinSpeed += razorBlade.spinSpeed / 100;
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

    private void pushTowardsTarget()
    {
        /*
         * Applies velocity in one large burst towards the Target
         */

        if (target != null)
        {
            rb.velocity = Vector2.zero;
            Vector2 playerDirection = (target.transform.position - this.transform.position).normalized;
            rb.AddForce(playerDirection * dashSpeed);
        }
    }

    public void areaTriggered()
    {
        /*
         * Stop movement for a few seconds then do dash attack
         * Called from RazorBlade
         * If in State.Pathfinding do nothing
         * If target is null do nothing
         */

        if (enemyState != State.Pathfinding || target == null)
        {
            rb.velocity = Vector2.zero;
            attackCooldownValue = attackCooldown;
            enemyState = State.Attacking;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collision with wall
        if (collision.gameObject.tag == "OuterWall")
        {
            rb.velocity = Vector2.zero;
            forceToApply = Vector2.zero;

            // If in "Pathfinding" state (waiting after attack) change to Targeting state
            if (enemyState == State.Pathfinding)
            {
                enemyState = State.Targeting;
            }
        }
    }
}
