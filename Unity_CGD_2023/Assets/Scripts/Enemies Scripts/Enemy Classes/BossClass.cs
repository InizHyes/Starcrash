using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossClass : EnemyClass
{
    // List of players
    private GameObject[] allPlayers;
    private int currentPlayerNumeral;

    //[Header("Boss Specific")]

    // Damageable
    private bool vulnerable = false; // Use setVulnerability() to change
    private float maxHealth = 0;
    private int threshold = 0;

    // Attack zones
    [Header("Boss Specific")]
    //[SerializeField] private float rotationSpeed = 100;

    //Death
    [SerializeField] private GameObject fade;
    private float fadeSpeed = 0.2f;
    private Animator animator;

    [Header("Attack 1")]
    [SerializeField] private GameObject bossAttackZone1;
    public float attack1Damage = 1;
    [SerializeField] private float attack1Uptime = 3f; // In seconds
    private float attack1UptimeValue = 0f;
    public float ticksPerSecond = 10;

    private void Start()
    {
        // Set starting state and variables
        initiateEnemy();

        attack1UptimeValue = attack1Uptime;
        animator = GetComponent<Animator>();

        //enemyState = State.Dead;
    }

    private void Update()
    {
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        switch (enemyState)
        {
            case State.Initiating:
                /*
                 * Starting state, used to run one-off functions for spawning
                 * 
                 * Boss-
                 * Make array of players, mix the array, and go down the array targeting each player equaly
                 */

                // Make array of players and randomize it
                shuffleArray(allPlayers);
                currentPlayerNumeral = 0;

                // Set health threshold (to 2/3rds, aka 20 health)
                maxHealth = health;
                threshold = Convert.ToInt32(maxHealth) * 2 / 3;

                enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * Countdown attack
                 * Set target to next in the list of randomized players
                 */

                // Attack cooldown countdown
                if (attackCooldwonLogic())
                {
                    // Target next player in array
                    if (currentPlayerNumeral >= allPlayers.Length)
                    {
                        //Reset numeral and reshuffle list
                        shuffleArray(allPlayers);
                        currentPlayerNumeral = 0;
                    }
                    target = allPlayers[currentPlayerNumeral];
                    currentPlayerNumeral++;

                    // Activate attack and wait to deactivate
                    bossAttackZone1.gameObject.SetActive(true);
                    enemyState = State.Attacking;
                }

                // Point attack at player
                //transform.forward = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, rotationSpeed * Time.deltaTime, 1);
                if (target != null)
                {
                    //---Bug: doesnt look at player correctly on the left, re-write code---
                    transform.right = target.transform.position - transform.position;
                }

                break;

            case State.Pathfinding:
                // REDUNDANT
                break;

            case State.Moving:
                //REDUNDANT (use as "stunned"?)
                break;

            case State.Attacking:
                /*
                 * Attack logic
                 * (multiple attacks?)
                 */

                if (attack1UptimeLogic())
                {
                    bossAttackZone1.SetActive(false);
                    enemyState = State.Targeting;
                }

                break;

            case State.Dead:
                /*
                 * Runs item drop logic then runs the logic associated with the enemy leaving the scene
                 * Can run death animation before running these functions
                 */

                // Fade
                fade.GetComponent<Image>().color = new Color(225, 225, 225, fade.GetComponent<Image>().color.a + Time.deltaTime * fadeSpeed);

                // Ending Animation
                animator.Play("Death");

                // Stop attacks
                bossAttackZone1.SetActive(false);

                //itemDropLogic();
                //initiateDeath();
                break;
        }
    }

    private void shuffleArray(GameObject[] gameObjectArray)
    {
        /*
         * The Knuth Shuffle (probably)
         * Shuffles the given array randomly
         */

        // temp and rng required for shuffle
        GameObject temp;
        int rng;

        for (int i = gameObjectArray.Length - 1; i >= 0; i--)
        {
            // Store old value, move random value, put back old value
            temp = gameObjectArray[i];
            rng = UnityEngine.Random.Range(i, gameObjectArray.Length);

            gameObjectArray[i] = gameObjectArray[rng];
            gameObjectArray[rng] = temp;
        }
    }

    public override void damageDetection(int damage)
    {
        /*
         * Deals damage to the enemy, called by the bullet itself
         * Checks if itself is dead ._.
         * 
         * OVERRIDE of EnemyClass damageDetection
         */

        // Check if vulnerable
        if (vulnerable)
        {
            health -= damage;

            // Check if dead after damage detection
            if (health <= 0)
            {
                enemyState = State.Dead;
            }

            // Check if damage reached threshold
            else if (health <= threshold)
            {
                // Reset vulnerability and set new threshold
                vulnerable = false;
                threshold -= Convert.ToInt32(maxHealth * 1 / 3); // Aka 20-10, or 10-10

                // Reset Plates
                this.gameObject.GetComponent<BossDMGPhase>().MechanicReset();
            }
        }
    }

    public void setVulnerability(bool value)
    {
        /*
         * Set boss vulnerability here
         * Set to True or False
         * Done here so that logic can be run in the event of bug fixes being required
         */

        vulnerable = value;
    }

    private bool attack1UptimeLogic()
    {
        /*
         * Counts down the attack timer
         * Returns true if attackCooldown is reached
         * Run every update
         */

        if (attack1UptimeValue > 0f)
        {
            // Countdown attack
            attack1UptimeValue -= Time.deltaTime;
            return false;
        }
        else
        {
            // Reset attack cooldown
            attack1UptimeValue = attack1Uptime;
            return true;
        }
    }
}