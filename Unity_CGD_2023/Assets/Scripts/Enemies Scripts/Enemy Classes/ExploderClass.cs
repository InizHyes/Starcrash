using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExploderClass : EnemyClass
{
    [Header("Exploder Specific")]
    [SerializeField] private ExploderAOE exploderAOE;
    public int explosionDamage = 10;
    public float explosionForce = 1f;
    [SerializeField][Tooltip("The higher the number the weaker the slow down on collision")] private float slowDown = 1f;
    public float deathLinger = 1f;

    private Animator animator;

    private void Start()
    {
        // Set starting state and variables
        animator = GetComponent<Animator>();
        initiateEnemy();

        animator.SetBool("isDeath", false); // Enemey Death animation bool

        exploderAOE.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (enemyState)
        {
            case State.Initiating:
                /*
                 * Starting state, used to run one-off functions for spawning
                 */

                //enemyState = State.Targeting;
                changestate(1);
                break;

            case State.Targeting:
                /*
                 * Target player and decide if State.Pathfinding is needed, otherwise change to moving
                 */

                targetClosestPlayer();
                //enemyState = State.Moving;
                changestate(3);
                break;

            case State.Pathfinding:
                /*
                 * Pathfind if line of sight is blocked
                 */

                break;

            case State.Moving:
                /*
                * Move towards player with velocity
                * Will loop here until the state is changed back to Targeting, Attackng, or Dead
                */

                moveTowardsTarget0G();

                // look at player
                /*
                Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;
                */
                break;

            case State.Attacking:
                /*
                 * Change State to here after attack is used
                 * Will wait here until attackCooldown is over then move back to Targeting
                 * 
                 * Before setting state to State.Attacking run //attackCooldownValue = attackCooldown;
                 * This will set the attackCooldownValue so that attackCooldwonLogic() can count it down
                 */

                // Slow down momentum
                rb.velocity -= (rb.velocity / 0.2f) * Time.deltaTime * slowDown;

                break;

            case State.Dead:
                /*
                 * Runs item drop logic then runs the logic associated with the enemy leaving the scene
                 * Can run death animation before running these functions
                 * 
                 * Set by AOE
                 * Lingers for a while
                 */

                //---This is broken, commented out for fix---
                // Make sure death animation plays before enemy destruction 
                //StartCoroutine(WaitForDeathAnimation());

                ///*
                // Wait after death
                if (deathLinger > 0)
                {
                    deathLinger -= Time.deltaTime;
                }
                else
                {
                    //itemDropLogic();
                    //initiateDeath();

                    // -Do manually instead-
                    NPCdeathCheck.NPCdeath();

                    // Destroy self and parent
                    Destroy(this.gameObject);
                    if (transform.parent != null && transform.parent.tag != "SpawnTrigger")
                    {
                        Destroy(transform.parent.gameObject);
                    }
                }
                //*/

                break;
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        //---Never ran as is broken---
        animator.SetBool("isDeath", true);

        // Wait for one frame to ensure that the animation has started
        yield return null;

        // Get the length of the current animation, which will be "isDeath"
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // Wait for the duration of the enemy death animation
        yield return new WaitForSeconds(animationLength);

        //Now the enemy dies after animation is done.
        //itemDropLogic();
        //initiateDeath();
        StopCoroutine(WaitForDeathAnimation());
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

    public override bool playerCollisionCheck(Collider2D collider)
    {
        /*
         * Call in CollisionEnter2D()
         * Checks if the collision is the player
         * Deals damage to the player based on bump attack value
         * 
         * Modified to explode instead
         */

        if (collider.gameObject.tag == "Player")
        {
            if (enemyState == State.Moving)
            {
                // Set AOE active
                explode();

                return true;
            }
        }

        return false;
    }

    private void explode()
    {
        // Set AOE active
        exploderAOE.gameObject.SetActive(true);
        //enemyState = State.Attacking;
        changestate(4);
    }

    public override void damageDetection(int damage)
    {
        if (enemyState != State.Attacking && enemyState != State.Dead)
        {
            // On death, explode instead
            health -= damage;

            if (damage > 0)
            {
                ChangeEnemyColor();
            }

            if (health <= 0)
            {
                explode();
            }
        }
    }

    public void deathStateChange()
    {
        // Needed by Exploder AOE
        enemyState = State.Dead;
        //changestate(5);
    }
}
