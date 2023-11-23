using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WandererClass : EnemyClass
{
    [Header("Wanderer Specific")]

    private Animator animate;

    public GameObject bulletPrefab;

    private float bulletSpeed = 1f;

    private bool playerInAtkZone = false;
    private bool playerInConeZone = false;

    private int attackTimer;

    private void Start()
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

                enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * This is where it would determine whether or not to spend time computating pathfinding
                 * It would be if(line of sight blocked){ enemyState = Pathfinding }
                 * But not needed now so im just assuming no LOS block
                 */
                //targetRangedClosestPlayer();
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


                // Move towards tartget but stay away at a minimuim length to avoid player fire

                moveTowardsTarget0G();

                // look at player
                Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;

                // Get collision from child triggers
                UnityEngine.Transform atkZoneTransform = transform.Find("DetectAttackZone"); // Had to manually had UnityEngine.Transform to make it work?
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

                UnityEngine.Transform coneZoneTransform = transform.Find("DetectConeZone"); // Had to manually had UnityEngine.Transform to make it work?
                if (coneZoneTransform != null)
                {
                    DetectAttack childscript = coneZoneTransform.GetComponent<DetectAttack>();
                    if (childscript != null)
                    {
                        playerInConeZone = childscript.playerTriggered;
                    }
                }

                if (playerInAtkZone)
                {
                    enemyState = State.Attacking;
                }

                if (playerInConeZone)
                {
                    enemyState = State.Attacking;
                }

                break;


            case State.Attacking:
                Debug.Log("attacking");

                shot();

                // Attack timer logic
                if (attackTimer < 100)
                {
                    attackTimer = attackTimer + 1;
                }
                if (playerInAtkZone)
                {
                    if (attackTimer > 99)
                    {
                        //animate.SetTrigger("gruntATTACK");
                        attackTimer = 0;
                        shot();
                        enemyState = State.Attacking;
                    }
                }

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

    private void shot()
    {
        Vector2 newPosition = new Vector2(0, 5);
        GameObject newBullet = Instantiate(bulletPrefab, newPosition, this.transform.rotation);
        newBullet.GetComponent<Rigidbody2D>().velocity = this.transform.position * bulletSpeed;
        SpriteRenderer bulletRenderer = newBullet.GetComponent<SpriteRenderer>();

        bulletRenderer.color = Color.red;

        Debug.Log("shot fired");

        //Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
    }
}
