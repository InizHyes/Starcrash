using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class TractorClass : EnemyClass
{

    [Header("Tractor Specific")]
    private Animator animate;

    [SerializeField] public GameObject tractorBeam;

    protected GameObject targetfollow;

    void Start()
    {
        // Set starting state and variables
        initiateEnemy();

        animate = GetComponent<Animator>(); // Maybe move into init function

    }

    private void Update()
    {
        switch (enemyState)
        {
            case State.Initiating:
                /*
                 * Starting state, used to run one-off functions for spawning
                 */

                tractorBeam.SetActive(false);

                enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * This is where it would determine whether or not to spend time computating pathfinding
                 * It would be if(line of sight blocked){ enemyState = Pathfinding }
                 * But not needed now so im just assuming no LOS block
                 */

                //targetClosestGrunt();
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

                tractorBeam.SetActive(false);

                // Move towards tartget but stay away at a minimuim length to avoid player fire
                moveTowardsTarget0G();

                // look at player
                Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;
                break;


            case State.Attacking:
                // Use trackor beam ablity
                tractorBeam.SetActive(true);
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

    // Function will allow for stronger enemies to hide behind grunts for tactical play,
    // But seprate moveTowardsTarget0G function may need to be set up to allow,
    // For the stronger enemey to follow grunt whilst keeping the correct target at the player.
    // Unless simple bug fixes can be made without out getting to complex!
    protected void targetClosestGrunt()
    {
        /*
         * Finds the closest object with the tag "grunt" and sets "targetfollow" as that grunt
         */
        GameObject[] grunts = GameObject.FindGameObjectsWithTag("grunts");
        float lowestDistance = 1;
        targetfollow = null;
        for (int i = 0; i < grunts.Length; i++)
        {
            //If targetfollow isnt set or distance is lower for other grunt, set grunt as targetfollow
            if (targetfollow == null || Vector3.Distance(this.transform.position, grunts[i].transform.position) < lowestDistance)
            {
                targetfollow = grunts[i];
                lowestDistance = Vector3.Distance(this.transform.position, grunts[i].transform.position);

                // Will look at player
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                //transform.LookAt(players[0].transform);
            }

        }
        //enemyState = State.Targeting;
    }

    // Use velocity to follow grunts
    protected void moveTowardsTarget1G()
    {
        // If not at max velocity
        if (rb.velocity.x < maxVelocity.x && rb.velocity.y < maxVelocity.y)
        {
            // Use target position and add to forceToApply
            forceToApply = ((targetfollow.transform.position - this.transform.position).normalized) * forceMultiplier;
            // Add every frame for excelleration (/100 cause too fast)
            moveForce += forceToApply / 100;
            rb.velocity = moveForce;
        }
    }
}