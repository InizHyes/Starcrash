using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class BossClass : EnemyClass
{
    private GameObject[] allPlayers;

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
                 * 
                 * Boss-
                 * Make array of players, mix the array, and go down the array targeting each player equaly
                 */

                // Make array of players and randomize it
                allPlayers = GameObject.FindGameObjectsWithTag("Player");
                shuffleArray(allPlayers);

                enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * This is where it would determine whether or not to spend time computating pathfinding
                 * It would be if(line of sight blocked){ enemyState = Pathfinding }
                 * But not needed now so im just assuming no LOS block
                 */

                enemyState = State.Attacking;
                break;

            case State.Pathfinding:
                // REDUNDANT
                break;

            case State.Moving:
                //REDUNDANT
                break;

            case State.Attacking:
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

    private void shuffleArray(GameObject[] gameObjectArray)
    {
        /*
         * The Knuth Shuffle (probably)
         * Shuffles the given array randomly
         */

        // temp and rng required for shuffle
        GameObject temp;
        int rng;

        for (int i = gameObjectArray.Length - 1; i >= 0; i--)
        {
            // Store old value, move random value, put back old value
            temp = gameObjectArray[i];
            rng = Random.Range(i, gameObjectArray.Length);

            gameObjectArray[i] = gameObjectArray[rng];
            gameObjectArray[rng] = temp;
        }
    }
}
