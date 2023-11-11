using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GruntClass : EnemyClass
{
    int atktimer = 41;
    bool playerInAtkZone = false;
    bool playerInConeZone = false;
    private Animator animate;
    //private BoxCollider2D playerDetect;


    void Start()
    {
        // Set starting state and variables
        initiateEnemy(1);
        animate = GetComponent<Animator>(); // Maybe move into init function
    }

    private void Update()
    {
        //Debug.Log(enemyState);
        switch (enemyState)
        {
            case State.Initiating:
                targetClosestPlayer();
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
                if (playerInAtkZone)
                {
                   if (atktimer > 39)
                    {
                        atktimer = 0;
                        animate.SetTrigger("gruntATTACK");
                        enemyState = State.Attacking;
                    }

                }
                if (atktimer < 40)
                {
                    atktimer = atktimer + 1;
                }


                /* didn't actually check if the cone works properly */
                if (playerInConeZone)
                {
                    // do something
                }


                /*
                * Move towards player with velocity
                * Maybe check if near to attack, maybe just change state on collision
                */
                moveTowardsTarget0G();
                Vector3 direction = target.transform.position - transform.position; // look at player
                transform.up = direction;


                break;

            case State.Attacking:
                moveTowardsTarget0G();
                if (atktimer < 40)
                {
                    atktimer = atktimer + 1;
                }
                if (atktimer > 30)
                {
                    enemyState = State.Moving;
                }
                /* before the animation finishes, 
                 * will spawn a hitbox prefab (ideally 0.25 seconds) in
                 * that damages the player tag & self deletes */



                break;

        }


        // getting collision from child triggers
        Transform atkZoneTransform = transform.Find("DetectAttackZone");
        if (atkZoneTransform != null)
        {
            // Accessing child
            DetectAttack childscript = atkZoneTransform.GetComponent<DetectAttack>();
            if (childscript != null)
            {
                // Accessing child's variable
                playerInAtkZone = childscript.playerTriggered;
            }
        }

        Transform coneZoneTransform = transform.Find("DetectConeZone");
        if (coneZoneTransform != null)
        {
            DetectAttack childscript = coneZoneTransform.GetComponent<DetectAttack>();
            if (childscript != null)
            {
                playerInConeZone = childscript.playerTriggered;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage detection
        damageDetection(collision);
    }
}
