using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GruntClass : EnemyClass
{
    // Variable to store hitbox prefab
    public GameObject superHitbox;

    int atktimer = 41;
    bool playerInAtkZone = false;
    bool playerInConeZone = false;
    public Animator animate;
    //private BoxCollider2D playerDetect;


    void Start()
    {
        //Set starting state and variables
        initiateEnemy();
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
                if (atktimer < 100)
                {
                    atktimer = atktimer + 1;
                }
                if (playerInAtkZone)
                {
                   if (atktimer > 99)
                    {
                        animate.SetTrigger("gruntATTACK");
                        atktimer = 0;
                        enemyState = State.Attacking;
                    }

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
                if (atktimer == 2)
                {
                    lungeForward();
                }
                slowDownAndStop();
                if (atktimer < 60)
                {
                    if (atktimer == 45)
                    {
                        /* before the animation finishes, 
                         * will spawn a hitbox prefab (ideally 0.25 seconds) in
                         * that damages the player tag & self deletes */
                        summonHitbox();
                    }
                    atktimer = atktimer + 1;
                }
                else
                {
                    enemyState = State.Moving;
                    atktimer = 0;
                }

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
        // Wall detection
        if (collision.gameObject.tag == "OuterWall")
        {
            rb.velocity = Vector2.zero;
            moveForce = Vector2.zero;
        }
    }

    void summonHitbox() // eventually i plan to make this in the enemyclass/somewhere, with passable variables
    {
        // Load the hitbox
        GameObject hitboxPrefab = Resources.Load<GameObject>("SuperHitBox");

        if (hitboxPrefab != null)
        {
            // Instantiate the hitbox prefab
            GameObject hitboxInstance = Instantiate(hitboxPrefab, transform.position, Quaternion.identity);

            hitboxInstance.transform.parent = transform; // used to make it a child (hitbox sticks to the entity)
            // Access the Hitbox script on the instance to set its variables
            SuperHitboxScript hitboxScript = hitboxInstance.GetComponent<SuperHitboxScript>();

            if (hitboxScript != null)
            {
                // Set relevant variable information for the hitbox (IMPORTANT)
                hitboxScript.damageAmount = 10;
                hitboxScript.size = new Vector2(0.6f, 0.8f); // these numbers need to be very small lol
                hitboxScript.rotationAngle = transform.eulerAngles.z;
                hitboxScript.offsetAmount = new Vector2(0f, 0.2f);
                hitboxScript.lifetime = 0.1f;
                hitboxScript.deleteOnConnect = false; // make sure this is true
            }
            else
            {
                Debug.LogError("brokey");
            }
        }
        else
        {
            Debug.LogError("dont worke");
        }
    }
}
