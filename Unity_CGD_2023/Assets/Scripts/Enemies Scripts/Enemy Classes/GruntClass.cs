using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GruntClass : EnemyClass
{
    // Variable to store hitbox prefab
    private GameObject superHitbox;
    AudioSource sound;
    [Header("Grunt Specific")]
    [SerializeField] private int attackTimer = 41;
    private bool playerInAtkZone = false;
    private bool playerInConeZone = false;
    private Animator animate;
    //private BoxCollider2D playerDetect;
    public AudioClip spawnsound;
    public AudioClip swipe;
    void Start()
    {
        sound = GetComponent<AudioSource>();
        initiateEnemy();
        animate = GetComponent<Animator>(); // Maybe move into init function
        sound.clip = spawnsound;
        sound.Play();
    }

    private void Update()
    {
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
                // Attack timer logic
                if (attackTimer < 100)
                {
                    attackTimer = attackTimer + 1;
                }
                if (playerInAtkZone)
                {
                   if (attackTimer > 99)
                    {
                        animate.SetTrigger("gruntATTACK");
                        attackTimer = 0;
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

                Transform coneZoneTransform = transform.Find("DetectConeZone");
                if (coneZoneTransform != null)
                {
                    DetectAttack childscript = coneZoneTransform.GetComponent<DetectAttack>();
                    if (childscript != null)
                    {
                        playerInConeZone = childscript.playerTriggered;
                    }
                }

                break;

            case State.Attacking:
                if (attackTimer == 2)
                {
                    lungeForward();
                }
                slowDownAndStop();
                if (attackTimer < 60)
                {
                    if (attackTimer == 45)
                    {
                        sound.clip = swipe;
                        sound.Play();
                        /* before the animation finishes, 
                         * will spawn a hitbox prefab (ideally 0.25 seconds) in
                         * that damages the player tag & self deletes */
                        summonHitbox();
                    }
                    attackTimer = attackTimer + 1;
                }
                else
                {
                    enemyState = State.Moving;
                    attackTimer = 0;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Wall detection
        if (collision.gameObject.tag == "OuterWall")
        {
            rb.velocity = Vector2.zero;
            moveForce = Vector2.zero;
        }
    }

    private void summonHitbox() // eventually i plan to make this in the enemyclass/somewhere, with passable variables
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
