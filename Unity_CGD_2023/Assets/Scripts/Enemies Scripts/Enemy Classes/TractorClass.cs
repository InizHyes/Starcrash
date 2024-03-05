using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class TractorClass : EnemyClass
{
    AudioSource sound;
    [Header("Tractor Specific")]
    [SerializeField] public GameObject tractorBeam;
    public AudioClip spawnsound;
    public AudioClip tractorsound;
    protected GameObject targetfollow;

    private Animator animator;

    void Start()
    {
        // Set starting state and variables
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        initiateEnemy();
        sound.clip = spawnsound;
        sound.Play();

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

                tractorBeam.SetActive(false);

                enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * This is where it would determine whether or not to spend time computating pathfinding
                 * It would be if(line of sight blocked){ enemyState = Pathfinding }
                 * But not needed now so im just assuming no LOS block
                 */

                animator.SetBool("isAttacking", false);

                //targetClosestGrunt();
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

                if (tractorBeam.activeInHierarchy)
                {
                    tractorBeam.SetActive(false);
                }
                sound.Stop();
                sound.loop = false;
                // Move towards tartget but stay away at a minimuim length to avoid player fire
                moveTowardsTarget0G();

                // look at player
                Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;
                break;


            case State.Attacking:

                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", true);

                // Use trackor beam ablity
                tractorBeam.SetActive(true);
                sound.loop = true;
                sound.clip = tractorsound;
                sound.Play();
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

    // Function will allow for stronger enemies to hide behind grunts for tactical play,
    // But seprate moveTowardsTarget0G function may need to be set up to allow,
    // For the stronger enemey to follow grunt whilst keeping the correct target at the player.
    // Unless simple bug fixes can be made without out getting to complex!
    protected void targetClosestGrunt()
    {
        /*
         * Finds the closest object with the tag "grunt" and sets "targetfollow" as that grunt
         */
        GameObject[] grunts = GameObject.FindGameObjectsWithTag("grunts");
        float lowestDistance = 1;
        targetfollow = null;
        for (int i = 0; i < grunts.Length; i++)
        {
            //If targetfollow isnt set or distance is lower for other grunt, set grunt as targetfollow
            if (targetfollow == null || Vector3.Distance(this.transform.position, grunts[i].transform.position) < lowestDistance)
            {
                targetfollow = grunts[i];
                lowestDistance = Vector3.Distance(this.transform.position, grunts[i].transform.position);

                // Will look at player
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                //transform.LookAt(players[0].transform);
            }

        }
        //enemyState = State.Targeting;
    }

    // Use velocity to follow grunts
    protected void moveTowardsTarget1G()
    {
        // If not at max velocity
        if (rb.velocity.x < maxVelocity.x && rb.velocity.y < maxVelocity.y)
        {
            // Use target position and add to forceToApply
            forceToApply = ((targetfollow.transform.position - this.transform.position).normalized) * forceMultiplier;
            // Add every frame for excelleration (/100 cause too fast)
            moveForce += forceToApply / 100;
            rb.velocity = moveForce;
        }
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
}