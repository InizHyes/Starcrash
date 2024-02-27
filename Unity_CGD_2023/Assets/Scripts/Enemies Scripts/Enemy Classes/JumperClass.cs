using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumperClass : EnemyClass
{
    [Header("Jumper Specific")]
    [SerializeField] private int moveSpeed = 200;
    AudioSource sound;
    public AudioClip spawnsound;
    public AudioClip jumpsound;

    private Animator animator;

    private void Start()
    {
        // Set starting state and variables
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        initiateEnemy();
        sound.clip = spawnsound;
        sound.Play();
        attackCooldownValue = 0f;

        animator.SetBool("isMoving", false); // Enemey Moving animation bool
        animator.SetBool("isAttacking", false); // Enemey Attacking animation bool
        animator.SetBool("isDeath", false); // Enemey Death animation bool
    }

    private void Update()
    {
        switch (enemyState)
        {
            case State.Initiating:
                /*
                 * Starting state, used to run one-off functions for spawning
                 */

                // Spawn with attack cooldown to prevent insta-jumping
                attackCooldownValue = attackCooldown / 2;
                enemyState = State.Attacking;
                break;

            case State.Targeting:
                /*
                 * This is where it would determine whether or not to spend time computating pathfinding
                 * It would be if(line of sight blocked){ enemyState = Pathfinding }
                 * But not needed now so im just assuming no LOS block
                 */

                animator.SetBool("isAttacking", false);

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

                animator.SetBool("isMoving", true);

                pushTowardsPlayer();

                // Start attack cooldown
                attackCooldownValue = attackCooldown;
                enemyState = State.Attacking;

                //moveTowardsTarget0G();

                /* //look at player
                Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;
                */
                break;

            case State.Attacking:
                /*
                 * Used to wait and count down attack timer
                 */

                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", true);

                // Count-down timer
                if (attackCooldwonLogic())
                {
                    enemyState = State.Targeting;
                }

                break;

            case State.Dead:
                /*
                 * Runs item drop logic then runs the logic associated with the enemy leaving the scene
                 * Can run death animation before running these functions
                 */

                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isDeath", true);

                itemDropLogic();
                initiateDeath();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collision with wall stops momentum
        if (enemyState == State.Attacking)
        {
            // Do not check for collision instantly
            if (collision.gameObject.tag == "OuterWall" && attackCooldownValue < attackCooldown - 1)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    private void pushTowardsPlayer()
    {
        /*
         * Applies velocity in one large burst towards player
         */
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        Vector2 playerDirection = (target.transform.position - this.transform.position).normalized;
        rb.AddForce(playerDirection * moveSpeed);
        sound.clip = jumpsound;
        sound.Play();
    }
}
