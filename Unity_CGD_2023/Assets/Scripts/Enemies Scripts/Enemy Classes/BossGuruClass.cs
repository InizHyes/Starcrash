using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossGuruClass : EnemyClass
{
    [Header("Slime Movement Values")]
    public float stoppingDistance = 1.5f; // Adjust this value in the Inspector or programmatically
    public float MaxVelocity = 5.0f; // Adjust this value as needed
    public float ForceMultiplier = 10.0f; // Adjust this value as needed
    public float damping = 0.9f; // Adjust this value in the Inspector or programmatically
    public float smoothness = 5f; // Adjust this value to control the smoothness of movement
    public float activationRange = 5.0f; // Adjust this value based on your desired activation range

    public float timeAtDestination = 2.0f; // Adjust this value as needed
    private float timeAtCurrentDestination = 0.0f;
    public GameObject enemyPrefab; // Reference to the enemy prefab to spawn
    public Transform spawnPointOne; // Reference to the spawn point empty game object
    public Transform spawnPointTwo; // Reference to the spawn point empty game object

    public GameObject gasPrefab;

    // Array of destination points
    public Transform[] destinationPoints;
    private int currentDestinationIndex;

    private Animator anim;

    private void Start()
    {
        // Set starting state and variables
        initiateEnemy();
        anim = GetComponent<Animator>();
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
                // Calculate the distance to the current destination

                BossMove();

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
                /*if (attackCooldwonLogic())
                {
                    enemyState = State.Targeting;
                }*/

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

    protected void BossMove()
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
                rb.velocity = Vector2.Lerp(rb.velocity, directionToPlayer * MaxVelocity, smoothness * Time.deltaTime);

                // Trigger jump animation when moving
                anim.SetBool("SwipeRight", false);
                anim.SetBool("SwipeLeft", false);
                anim.SetBool("IsMoving", true);
                anim.SetBool("IsAttacking", false);
            }
            else
            {
                // Stop moving if close enough to the player
                rb.velocity = Vector2.zero;

                anim.SetBool("SwipeRight", false);
                anim.SetBool("SwipeLeft", false);
                anim.SetBool("IsMoving", false);
                anim.SetBool("IsAttacking", true);

                // Set the next destination
                SetNextDestination();
            }
        }
        else
        {
            // Player is outside the activation range, move towards the current destination
            Vector2 directionToDestination = (destinationPoints[currentDestinationIndex].position - transform.position).normalized;
            rb.velocity = Vector2.Lerp(rb.velocity, directionToDestination * MaxVelocity, smoothness * Time.deltaTime);

            // Trigger walk animation when moving
            anim.SetBool("SwipeRight", false);
            anim.SetBool("SwipeLeft", false);
            anim.SetBool("IsMoving", true);
            anim.SetBool("IsAttacking", false);

            // Check if the Guru has reached its current destination
            float distanceToDestination = Vector2.Distance(transform.position, destinationPoints[currentDestinationIndex].position);
            if (distanceToDestination < 0.5f)
            {
                // Increment the time spent at the current destination
                timeAtCurrentDestination += Time.deltaTime;

                // Check if the Guru has reached its current destination
                int reachedDestinationIndex = CheckReachedDestination();

                if (reachedDestinationIndex != -1)
                {
                    // Boss has reached a destination point
                    Debug.Log("Boss reached destination point: " + reachedDestinationIndex);

                    if (reachedDestinationIndex == 0) 
                    {
                        anim.SetBool("SwipeRight", true);
                        anim.SetBool("SwipeLeft", false);
                        anim.SetBool("IsMoving", false);
                        anim.SetBool("IsAttacking", false);
                    }

                    if (reachedDestinationIndex == 1)
                    {
                        anim.SetBool("SwipeRight", false);
                        anim.SetBool("SwipeLeft", true);
                        anim.SetBool("IsMoving", false);
                        anim.SetBool("IsAttacking", false);
                    }
                }


                // Check if enough time has passed at the current destination
                if (timeAtCurrentDestination >= timeAtDestination)
                {
                    // Reset the timer
                    timeAtCurrentDestination = 0.0f;
                    anim.SetBool("SwipeRight", false);
                    anim.SetBool("SwipeLeft", false);
                    anim.SetBool("IsMoving", true);
                    anim.SetBool("IsAttacking", false);

                    // Set the next destination
                    SetNextDestination();
                }
            }
            else
            {
                // Reset the timer if the boss moves away from the destination
                timeAtCurrentDestination = 0.0f;
            }
        }
    }

    private int CheckReachedDestination()
    {
        for (int i = 0; i < destinationPoints.Length; i++)
        {
            float distanceToDestination = Vector2.Distance(transform.position, destinationPoints[i].position);
            if (distanceToDestination < 0.5f)
            {
                return i; // Return the index of the reached destination point
            }
        }
        return -1; // Return -1 if no destination point is reached
    }

    private void SpawnMinionOne()
    {
        if (enemyPrefab != null && spawnPointOne != null)
        {
            Instantiate(enemyPrefab, spawnPointOne.position, spawnPointOne.rotation);
            Instantiate(gasPrefab, spawnPointOne.position, spawnPointOne.rotation);
        }
    }

    private void SpawnMinionTwo()
    {
        if (enemyPrefab != null && spawnPointOne != null)
        {
            Instantiate(enemyPrefab, spawnPointTwo.position, spawnPointOne.rotation);
            Instantiate(gasPrefab, spawnPointOne.position, spawnPointOne.rotation);
        }
    }


    private void SetNextDestination()
    {
        // Increment the destination index or reset to 0 if reached the end
        currentDestinationIndex = (currentDestinationIndex + 1) % destinationPoints.Length;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            // Change the enemy color or apply the animation here
            ChangeEnemyColor();
        }
    }

    private void ChangeEnemyColor()
    {
        // Implement logic to change the enemy color or apply the animation here
        // Change the sprite renderer color
       GetComponent<SpriteRenderer>().color = Color.red;

        // Alternatively, trigger an animation
        //anim.SetTrigger("Hit");

        StartCoroutine(ResetHitState());
    }

    private IEnumerator ResetHitState()
    {
        // Wait for a short duration before resetting the hit state
        yield return new WaitForSeconds(0.1f);
        // Reset the "Hit" trigger
        //anim.ResetTrigger("Hit");

        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
