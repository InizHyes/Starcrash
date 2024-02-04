using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MedicalDroidClass: EnemyClass
{
    private Animator animate;
    AudioSource sound;

    [Header("Medical Droid Specific")]
    [SerializeField] public GameObject HealATK;
    public AudioClip spawnsound;
    public AudioClip medicalDroidsound;

    private void Start()
    {
        // Set starting state and variables
        sound = GetComponent<AudioSource>();
        initiateEnemy();
        sound.clip = spawnsound;
        sound.Play();
        animate = GetComponent<Animator>(); // Maybe move into init function
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

                targetClosestPlayer();

                //targetClosestEnemy();

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

                HealATK.SetActive(false);
                sound.Stop();
                sound.loop = false;


                // Check to see if we need to target 
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

                itemDropLogic();
                initiateDeath();
                break;
        }
    }

    private void targetClosestEnemy()
    {
        /*
         * Finds the closest object with the tag "Enemy" and sets "target" to heal
         */
        GameObject[] Enemy = GameObject.FindGameObjectsWithTag("Generator");
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

            // Else find player to attack
            else
            {
                targetClosestPlayer();
                Debug.LogWarning("Target player");
            }
        }

    }

}
