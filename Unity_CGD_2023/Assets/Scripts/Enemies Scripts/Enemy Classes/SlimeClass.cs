using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeClass : EnemyClass
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

    // When showing variables in the inspector use a header to show the unique variables
    //[Header("TEMPLATECLASS Specific")]

    [Header("Slime Movement Values")]
    public float stoppingDistance = 1.0f; // Adjust this value in the Inspector or programmatically
    public float slimeMaxVelocity = 5.0f; // Adjust this value as needed
    public float slimeForceMultiplier = 10.0f; // Adjust this value as needed
    public float damping = 0.9f; // Adjust this value in the Inspector or programmatically
    public float smoothness = 5f; // Adjust this value to control the smoothness of movement
    public float activationRange = 5.0f; // Adjust this value based on your desired activation range

    [Header("Slime Damage")]
    public float damage = 10;
    private Animator animator;
    private float deathAnimationDuration = 2.0f; // Adjust this value based on your actual death animation duration

    // Array of destination points
    public Transform[] destinationPoints;
    private int currentDestinationIndex;

    private void Start()
    {
        // Set starting state and variables
        initiateEnemy();

        animator = GetComponent<Animator>();

        // Set the initial destination
        SetNextDestination();
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

                slimeMove();

                // look at player
                //Vector3 direction = target.transform.position - transform.position;
                //transform.up = direction;
                break;

            case State.Attacking:
                /*
                 * Change State to here after attack is used
                 * Will wait here until attackCooldown is over then move back to Targeting
                 * 
                 * Before setting state to State.Attacking run //attackCooldownValue = attackCooldown;
                 * This will set the attackCooldownValue so that attackCooldwonLogic() can count it down
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
                animator.SetBool("IsDead", true);

                StartCoroutine(WaitForDeathAnimation());
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            // Change the enemy color or apply the animation here
            ChangeEnemyColor();
        }

        else if (collision.gameObject.tag == "Player")
        {
            print("Hit");

            // Get the PlayerHealth component from the colliding GameObject
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();

            // Check if the PlayerHealth component is not null
            if (playerStats != null)
            {
                // Deal damage to the player
                playerStats.TakeDamage(damage);
            }
        }
    }

    protected void slimeMove()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);

        // Check if the player is within a specific range before moving
        if (distanceToPlayer <= activationRange)
        {
            // Check if the enemy is close enough to the player to stop moving
            if (distanceToPlayer > stoppingDistance)
            {
                // Calculate the direction to the player
                Vector2 directionToPlayer = (target.transform.position - transform.position).normalized;

                // Smoothly interpolate between the current velocity and the desired velocity towards the player
                rb.velocity = Vector2.Lerp(rb.velocity, directionToPlayer * slimeMaxVelocity, smoothness * Time.deltaTime);

                // Trigger jump animation when moving
                animator.SetBool("IsMoving", true);
            }
            else
            {
                // Stop moving if close enough to the player
                rb.velocity = Vector2.zero;

                // Stop jump animation when not moving
                animator.SetBool("IsMoving", false);

                // Trigger attack animation
                animator.SetTrigger("Attack");

                // Set the next destination
                SetNextDestination();
            }
        }
        else
        {
            // Player is outside the activation range, move towards the current destination
            Vector2 directionToDestination = (destinationPoints[currentDestinationIndex].position - transform.position).normalized;
            rb.velocity = Vector2.Lerp(rb.velocity, directionToDestination * slimeMaxVelocity, smoothness * Time.deltaTime);

            // Trigger jump animation when moving
            animator.SetBool("IsMoving", true);

            // Check if the slime has reached its current destination
            float distanceToDestination = Vector2.Distance(transform.position, destinationPoints[currentDestinationIndex].position);
            if (distanceToDestination < 0.1f)
            {
                // Set the next destination
                SetNextDestination();
            }
        }
    }

    private void SetNextDestination()
    {
        // Increment the destination index or reset to 0 if reached the end
        currentDestinationIndex = (currentDestinationIndex + 1) % destinationPoints.Length;
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Wait for the duration of the death animation
        yield return new WaitForSeconds(deathAnimationDuration);

        // Perform item drop logic and initiate death after the pause
        itemDropLogic();
        initiateDeath();
    }

    private void ChangeEnemyColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;

        StartCoroutine(ResetHitState());
    }

    private IEnumerator ResetHitState()
    {
        // Wait for a short duration before resetting the hit state
        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
