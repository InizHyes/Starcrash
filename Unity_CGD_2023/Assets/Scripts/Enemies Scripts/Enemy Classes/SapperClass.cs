using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SapperClass : EnemyClass
{
    private Animator animate;
    AudioSource sound;

    [Header("Sapper Specific")]
    [SerializeField] public GameObject WeldATK;
    [SerializeField] public GameObject EMPAOE;
    public AudioClip spawnsound;
    public AudioClip sappersound;

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

                WeldATK.SetActive(false);
                EMPAOE.SetActive(false);

                enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * Target player and decide if State.Pathfinding is needed, otherwise change to moving
                 */

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

                WeldATK.SetActive(false);
                EMPAOE.SetActive(false);
                sound.Stop();
                sound.loop = false;


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

                //Will heal genrators and do little damage to players
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
                sound.loop = true;
                sound.clip = sappersound;
                sound.Play();

                itemDropLogic();
                initiateDeath();
                break;
        }
    }

}
