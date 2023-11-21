using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class TractorClass : EnemyClass
{

    private Animator animate;

    [Header("Tractor Specific")]
    [SerializeField] private GameObject tractorBeam;

    private void Start()
    {
        // Set starting state and variables
        initiateEnemy();

        tractorBeam.SetActive(true); // Ablity off to start

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

                targetClosestPlayer();

                targetClosestGrunt();

                //tractorBeam.SetActive(false);

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
                /*
                * Move towards player with velocity
                * Maybe check if near to attack, maybe just change state on collision
                */

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
}