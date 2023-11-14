using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class TractorClass : EnemyClass
{

    private Animator animate;

    public GameObject Tbeam;

    void Start()
    {
        // Set starting state and variables
        initiateEnemy(10);

        Tbeam.SetActive(false); // Ablity off to start

        animate = GetComponent<Animator>(); // Maybe move into init function
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

                targetClosestPlayer();

                targetClosestGrunt();

                Tbeam.SetActive(false);

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
                Tbeam.SetActive(true);

                break;
        }

        // Check if dead (might move to function and call in Moving and Attacking
        if (health == 0 && spawnLogic != null)
        {
            //spawnLogic.NPCdeath(); //New error cause unknown
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage detection
        damageDetection(collision);
    }
}