using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WandererClass : EnemyClass
{
    private Animator animate;

    [Header("Wanderer Specific")]
    public GameObject bulletPrefab;

    public Transform gunPoint;

    private float bulletSpeed = 5f;

    private bool playerInAtkZone = false;
    //private bool playerInConeZone = false;

    public bool canAttack;

    private void Start()
    {
        // Set starting state and variables
        initiateEnemy();

        canAttack = false;

        animate = GetComponent<Animator>(); // Maybe move into init function
    }

    private void Update()
    {

        // Get collision from child triggers
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

                if (playerInAtkZone == true)
                {
                    StartCoroutine(delayShot());
                    enemyState = State.Attacking;
                }

                break;


            case State.Attacking:
                Debug.Log("attacking");

                if (canAttack == true)
                {
                    fireShot();
                    StartCoroutine(delayShot());
                }

                if (playerInAtkZone == false)
                {
                    StopCoroutine(delayShot());
                    enemyState = State.Targeting;
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

    //How long should it take to fire at player
    private IEnumerator delayShot()
    {
        canAttack = false;

        yield return new WaitForSeconds(3);

        canAttack = true;
    }

    //Fire at target
    private void fireShot()
    {   
        GameObject firedBullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        Vector2 bulletDir = gunPoint.right ;
        firedBullet.GetComponent<Rigidbody2D>().velocity = bulletDir * bulletSpeed;

        SpriteRenderer bulletRenderer = firedBullet.GetComponent<SpriteRenderer>();
        bulletRenderer.color = Color.red;

        Debug.Log("shot fired");
    }
}
