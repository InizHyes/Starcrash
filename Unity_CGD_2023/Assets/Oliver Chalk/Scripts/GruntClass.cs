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
                targetClosestPlayer();
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

                moveTowardsTarget0G();
                break;
        }
    }
}
