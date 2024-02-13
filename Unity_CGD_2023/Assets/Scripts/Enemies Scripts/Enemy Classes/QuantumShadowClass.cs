using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuantumShadowClass : EnemyClass
{
    private Animator animate;
    AudioSource sound;

    [Header("Quantum Shadow Specific")]
    public AudioClip spawnsound;
    public AudioClip quantumShadowsound;
    public GameObject weapon;
    public Transform aim;
    private int throwspeed = 50;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        // Set starting state and variables
        initiateEnemy();
        spriteRenderer = GetComponent<SpriteRenderer>();

        sound = GetComponent<AudioSource>();
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

                sound.Stop();
                sound.loop = false;

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

                //Stop movement and become visable and unverable



                StartCoroutine(QSabilityOff());

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
                sound.clip = quantumShadowsound;
                sound.Play();

                itemDropLogic();
                initiateDeath();
                break;
        }
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
        NinjaStar.GetComponent<Rigidbody>().velocity = aim.forward * throwspeed;

        yield return new WaitForSeconds(2.5f);

        QSabilityOn();

    }
}
