using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SapperClass : EnemyClass
{
    

    [Header("Sapper Specific")]
    [SerializeField] public GameObject WeldATK;
    [SerializeField] public GameObject EMPAOE;
    
    private int attackType;

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

                if (WeldATK.activeInHierarchy)
                {
                    WeldATK.SetActive(false);
                }
                if (EMPAOE.activeInHierarchy)
                {
                    EMPAOE.SetActive(false);
                }

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
                    targetClosestGenerator();
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

                if (WeldATK.activeInHierarchy)
                {
                    WeldATK.SetActive(false);
                }
                if (EMPAOE.activeInHierarchy)
                {
                    EMPAOE.SetActive(false);
                }

               


                // Check to see if we need to target genrators
                moveTowardsTarget0G();

                // look at player
                Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;
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

                WeldATK.SetActive(true);


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

                // Use EMP AOE attack before death
                EMPAOE.SetActive(true);
                GetComponent<SFX>().PlaySound("");

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

    private void targetClosestGenerator()
    {
        /*
         * Finds the closest object with the tag "Generator" and sets "target" to heal
         */
        GameObject[] Generator = GameObject.FindGameObjectsWithTag("Generator");
        float lowestDistance = 0;
        target = null;
        for (int i = 0; i < Generator.Length; i++)
        {
            //If target isnt set or distance is lower for other Generator, set Generator as target
            if (target == null || Vector3.Distance(this.transform.position, Generator[i].transform.position) < lowestDistance)
            {
                target = Generator[i];
                lowestDistance = Vector3.Distance(this.transform.position, Generator[i].transform.position);
                Debug.LogWarning("Target gen");
            }

            // Else find somthing to attack
            else
            {
                enemyState = State.Targeting;
                Debug.LogWarning("Finding Target");
            }
        }
        print(target.transform.position);
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
