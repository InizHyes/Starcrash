using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MedicalDroidClass: EnemyClass
{
    AudioSource sound;

    [Header("Medical Droid Specific")]
    [SerializeField] public GameObject HealATK;
    public AudioClip spawnsound;
    public AudioClip medicalDroidsound;
    private int attackType;

    private Animator animator;

    private void Start()
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

                HealATK.SetActive(false);

                enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * Target player and decide if State.Pathfinding is needed, otherwise change to moving
                 */

                animator.SetBool("isAttacking", false);

                attackType = Random.Range(1, 2);

                if (attackType == 1)
                {
                    target = null;
                    targetClosestPlayer();
                }

                if (attackType == 2)
                {
                    target = null;
                    targetClosestEnemy();
                }

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

                HealATK.SetActive(false);
                sound.Stop();
                sound.loop = false;


                // Check to see if we need to target 
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

                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", true);

                HealATK.SetActive(true);


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

                sound.loop = true;
                sound.clip = medicalDroidsound;
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
        StartCoroutine(WaitForDeathAnimation());
    }

    private void targetClosestEnemy()
    {
        /*
         * Finds the closest object with the tag "Enemy" and sets "target" to heal
         */
        GameObject[] Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        float lowestDistance = 0;
        target = null;
        for (int i = 0; i < Enemy.Length; i++)
        {
            //If target isnt set or distance is lower for other Generator, set Generator as target
            if (target == null && Vector3.Distance(this.transform.position, Enemy[i].transform.position) < lowestDistance)
            {
                target = Enemy[i];
                lowestDistance = Vector3.Distance(this.transform.position, Enemy[i].transform.position);
                Debug.LogWarning("Target Enemy");
            }

            // Else find somthing to attack
            else
            {
                enemyState = State.Targeting;
                Debug.LogWarning("Finding Target");
            }
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
