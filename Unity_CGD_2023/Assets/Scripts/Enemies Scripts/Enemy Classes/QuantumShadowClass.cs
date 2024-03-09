using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuantumShadowClass : EnemyClass
{
    AudioSource sound;

    [Header("Quantum Shadow Specific")]
    public AudioClip spawnsound;
    public AudioClip quantumShadowsound;
    public GameObject weapon;
    public Transform aim;
    private float throwspeed = 5f;
    private SpriteRenderer spriteRenderer;
    private bool hasShot;

    private Animator animator;


    private void Start()
    {
        // Set starting state and variables
        animator = GetComponent<Animator>();
        hasShot = false;
        initiateEnemy();
        spriteRenderer = GetComponent<SpriteRenderer>();

        sound = GetComponent<AudioSource>();

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

                QSabilityOn();

                targetClosestPlayer();
                enemyState = State.Moving;
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

                animator.SetBool("isMoving", true);

                sound.Stop();
                sound.loop = false;

                moveTowardsTarget0G();

                // look at player
                if (target != null)
                {
                    Vector3 direction = target.transform.position - transform.position;
                    transform.up = direction;
                }
                break;

            case State.Attacking:
                /*
                 * Change State to here after attack is used
                 * Will wait here until attackCooldown is over then move back to Targeting
                 * 
                 * Before setting state to State.Attacking run //attackCooldownValue = attackCooldown;
                 * This will set the attackCooldownValue so that attackCooldwonLogic() can count it down
                 */

                //Stop movement and become visable and unverable

                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", true);

                if (!hasShot)
                {
                    hasShot = true;
                    StartCoroutine(QSabilityOff());
                }

                // Count-down timer
                if (attackCooldwonLogic())
                {
                    hasShot = false;
                    enemyState = State.Targeting;
                }

                else
                {
                    spriteRenderer.enabled = true;
                }

                break;

            case State.Dead:
                /*
                 * Runs item drop logic then runs the logic associated with the enemy leaving the scene
                 * Can run death animation before running these functions
                 */

                sound.loop = true;
                sound.clip = quantumShadowsound;
                sound.Play();

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
    private void QSabilityOn()
    {
        StopCoroutine(QSabilityOff());

        //Turn invisible and invincible
        spriteRenderer.enabled = false;
        this.gameObject.layer = 3; // Ignore bullets layer
    }

    private IEnumerator QSabilityOff()
    {
        //Turn visible and vulnerable
        spriteRenderer.enabled = true;
        this.gameObject.layer = 0; // Default layer

        var NinjaStar = Instantiate(weapon, aim.position, aim.rotation);

        Vector2 starDir = aim.right;
        NinjaStar.GetComponent<Rigidbody2D>().velocity = starDir * throwspeed;
        yield return new WaitForSeconds(2.5f);

        QSabilityOn();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         * Wall detection
         * On collision with an object with the tag "OuterWall"
         * Stops all momentum
         */

        if (collision.gameObject.tag == "OuterWall")
        {
            rb.velocity = Vector2.zero;
            moveForce = Vector2.zero;
        }
    }

    public void StartAttack()
    {
        if (enemyState != State.Attacking)
        {
            attackCooldownValue = attackCooldown;
            changestate(4); // Attacking 
        }
    }
}
