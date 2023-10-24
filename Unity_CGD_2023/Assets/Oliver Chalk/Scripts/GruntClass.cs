using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntClass : EnemyClass
{
    void Start()
    {
        // Set starting state and variables
        enemyState = State.Initiating;
        health = 1;
    }

    private void Update()
    {
        switch (enemyState)
        {
            case State.Initiating:
                // On spawn state, find closest player
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                float lowestDistance = 0;
                for (int i = 0; i < players.Length; i++)
                {
                    //If target isnt set or distance is lower for other player, set player as target
                    if (target == null || Vector3.Distance(this.transform.position, players[i].transform.position) < lowestDistance)
                    {
                        target = players[i];
                        lowestDistance = Vector3.Distance(this.transform.position, players[i].transform.position);
                    }
                    // Else do nothing
                }
                enemyState = State.Targeting;
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
                break;
        }
    }
}
