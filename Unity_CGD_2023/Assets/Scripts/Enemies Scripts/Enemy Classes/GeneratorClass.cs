using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneratorClass : EnemyClass
{
    private void Start()
    {
        // Set starting state and variables

        health = 25;

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
