using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RazorClass : EnemyClass
{
    [Header("Razor Specific")]
    [SerializeField] private int dashSpeed = 200;
    [SerializeField] private RazorBlade razorBlade;
    private int waitExitTime = 5;
    private float waitExitTimeCounter = 5;

    private Animator animator;

    private void Start()
    {
        // Set starting state and variables
        animator = GetComponent<Animator>();
        initiateEnemy();

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

                enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * Target player and decide if State.Pathfinding is needed, otherwise change to moving
                 */

                animator.SetBool("isAttacking", false);

                waitExitTimeCounter = waitExitTime;
                targetClosestPlayer();
                enemyState = State.Moving;
                break;

            case State.Pathfinding:
                /*
                 * USED AS WAIT TIME, not pathfinding
                 * Using collision with wall as exit time
                 * 
                 * //Using attackCooldwonLogic() as a timer
                 * //Waits as long as attackCooldown
                 */

                /*
                if (attackCooldwonLogic())
                {
                    enemyState = State.Targeting;
                } 
                */

                if (waitExitTimeCounter > 0)
                {
                    waitExitTimeCounter -= Time.deltaTime;
                }
                else
                {
                    enemyState = State.Targeting;
                }

                break;

            case State.Moving:
                /*
                * Move towards player with velocity
                * Will loop here until the state is changed back to Targeting, Attackng, or Dead
                */

                animator.SetBool("isMoving", true);

                moveTowardsTarget0G();

                // Slow down razor to default
                if (razorBlade.spinSpeed >= razorBlade.DEFAULTSPINSPEED)
                {
                    razorBlade.spinSpeed -= razorBlade.spinSpeed / 100;
                }

                // look at player
                Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;
                break;

            case State.Attacking:
                /*
                 * Change State to here after attack is used
                 * Will wait here until attackCooldown is over then move back to Targeting
                 */

                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", true);

                // Count-down timer
                if (attackCooldwonLogic())
                {
                    pushTowardsTarget();

                    // Use pathfinding to wait
                    GetComponent<SFX>().PlaySound("");
                    attackCooldownValue = attackCooldown;
                    enemyState = State.Pathfinding;
                }

                // Speed up razor to max
                GetComponent<SFX>().PlaySound("");
                if (razorBlade.spinSpeed <= razorBlade.maxSpinSpeed)
                {
                    razorBlade.spinSpeed += razorBlade.spinSpeed / 100;
                }

                break;

            case State.Dead:
                /*
                 * Runs item drop logic then runs the logic associated with the enemy leaving the scene
                 * Can run death animation before running these functions
                 */

                // Make sure death animation plays before enemy destruction 
                StartCoroutine(WaitForDeathAnimation());

                break;
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDeath", true);

        // Wait for one frame to ensure that the animation has started
        yield return null;

        // Get the length of the current animation, which will be "isDeath"
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // Wait for the duration of the enemy death animation
        yield return new WaitForSeconds(animationLength);

        //Now the enemy dies after animation is done.
        itemDropLogic();
        initiateDeath();
        StopCoroutine(WaitForDeathAnimation());
    }
    private void pushTowardsTarget()
    {
        /*
         * Applies velocity in one large burst towards the Target
         */

        if (target != null)
        {
            rb.velocity = Vector2.zero;
            Vector2 playerDirection = (target.transform.position - this.transform.position).normalized;
            rb.AddForce(playerDirection * dashSpeed);
        }
    }

    public void areaTriggered()
    {
        /*
         * Stop movement for a few seconds then do dash attack
         * Called from RazorBlade
         * If in State.Pathfinding do nothing
         * If target is null do nothing
         */

        if (enemyState != State.Pathfinding || target == null)
        {
            rb.velocity = Vector2.zero;
            attackCooldownValue = attackCooldown;
            enemyState = State.Attacking;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collision with wall
        if (collision.gameObject.tag == "OuterWall")
        {
            rb.velocity = Vector2.zero;
            forceToApply = Vector2.zero;

            // If in "Pathfinding" state (waiting after attack) change to Targeting state
            if (enemyState == State.Pathfinding)
            {
                enemyState = State.Targeting;
            }
        }
    }
}
