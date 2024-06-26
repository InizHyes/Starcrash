using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    // Enemy common variables
    [Header("Common Variables")]
    [Header("Health")]
    [SerializeField] protected float health = 10;
    [SerializeField] protected int armour = 0;
    [SerializeField] protected int meat = 0;
    [SerializeField] protected float showHitDuration = 0.2f;

    // Attack value
    [Header("Damage")]
    [SerializeField] private int bumpDamage = 1; // Used when collision with the player
    // Attack cooldown
    [SerializeField] protected float attackCooldown = 5f; // In seconds, can be set in inspector
    protected float attackCooldownValue = 0f;

    // rb movement variables
    [Header("Movement")]
    [SerializeField] protected float forceMultiplier = 1f;
    [SerializeField] protected Vector2 maxVelocity = new Vector2(100f, 100f);
    protected GameObject target;
    protected Rigidbody2D rb;
    protected Vector2 forceToApply;
    [HideInInspector] public Vector2 moveForce;

    // Set spawnlogic prefab onto spawnLogic, will find and assign script to NPCdeathCheck
    //protected GameObject spawnLogic;
    [HideInInspector] public SpawnLogic NPCdeathCheck;

    // Item drop variables
    [Header("Spawning/Drops")]
    [SerializeField] private GameObject[] droppedObejcts;
    [Tooltip("Odds of dropping, 1/x chance")] [SerializeField] private int dropOdds = 1;

    // States
    protected enum State
    {
        Initiating, // Can be used to freeze enemies while the player is loading into the room
        Targeting, // Running script to find nearest player on first spawn, or change targeting to closer player on cone collision
        Pathfinding, // Calculating pathfinding around objects
        Moving, // Will be in this state 90% of the time, moving towards target player
        Attacking, // Run attack animation, will prevent Sans-like attacking (-1 hp every frame)
        Dead // Dead. Run drop item script (may be part of this code)
    }
    protected State enemyState;

    protected void initiateEnemy()
    {
        /*
         * Assignes and runs all the common variables/functions between all enemy types
         * Health, state, spawnLogic, etc.
         */

        //enemyState = State.Initiating;
        changestate(0);
        rb = GetComponent<Rigidbody2D>();

        // Set on instantiaion by SpawnLogic instead
        /*
        spawnLogic = GameObject.Find("SpawnController");
        if (spawnLogic != null )
        {
            NPCdeathCheck = spawnLogic.GetComponent<SpawnLogic>();
        }
        */
    }

    protected bool targetClosestPlayer()
    {
        /*
         * Finds the closest object with the tag "Player" and sets "target" as that player
         */
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float lowestDistance = 0;
        target = null;
        for (int i = 0; i < players.Length; i++)
        {
            // If player is downed, skip
            if (players[i].GetComponent<Down>().downed == true)
            {
                continue;
            }

            // If target isnt set or distance is lower for other player, set player as target
            if (target == null || Vector3.Distance(this.transform.position, players[i].transform.position) < lowestDistance)
            {
                target = players[i];
                lowestDistance = Vector3.Distance(this.transform.position, players[i].transform.position);
            }
            // Else do nothing
        }

        if (target == null)
        {
            Debug.Log("Add a player to the scene");
            return false;
        }

        return true;

        //---Moved due to Jumper needing constant acceess to this function---
        //enemyState = State.Targeting;
    }

    protected void targetRangedClosestPlayer()
    {
        /*
         * Finds the closest object with the tag "Player" and sets "target" as that player, but from distance
         */
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float lowestDistance = 5;
        for (int i = 0; i < players.Length; i++)
        {
            //If target isnt set or distance is lower for other player, set player as target
            if (target == null || Vector3.Distance(this.transform.position, players[i].transform.position) < lowestDistance)
            {
                target = players[i];
                lowestDistance = Vector3.Distance(this.transform.position, players[i].transform.position);
            }
            // Else do nothing
        }
        //---Moved due to Jumper needing constant acceess to this function---
        //enemyState = State.Targeting;
    }

    protected void moveTowardsTarget0G()
    {
        /*
         * When seting rb.velocity to 0 set forceToApply to 0 too
         */

        // If not at max velocity
        if (rb.velocity.x < maxVelocity.x && rb.velocity.y < maxVelocity.y)
        {
            // If target is set
            if (target != null)
            {
                // Use target position and add to forceToApply
                forceToApply = ((target.transform.position - this.transform.position).normalized) * forceMultiplier;
                // Add every frame for excelleration (/100 cause too fast)
                moveForce += forceToApply / 100;
                rb.velocity = moveForce;
            }
        }
    }

    protected void slowDownAndStop()
    {
        rb.velocity *= 0.98f;
        moveForce = rb.velocity;
    }

    protected void lungeForward()
    {
        if (rb.velocity.x < maxVelocity.x && rb.velocity.y < maxVelocity.y)
        {
            forceToApply = ((target.transform.position - this.transform.position).normalized) * forceMultiplier;
            moveForce += forceToApply;
            rb.velocity = moveForce;
        }
    }

    public virtual void damageDetection(int damage)
    {
        /*
         * Deals damage to the enemy, called by the bullet itself
         * Checks if itself is dead ._.
         */

        health -= damage;
        
        if (damage > 0)
        {
            ChangeEnemyColor();
        }

        // Check if dead after damage detection
        if (health <= 0)
        {
            //enemyState = State.Dead;
            changestate(5);

            /*
             * Change state instead, move this to function
             * This is so different enemies can drop different items on death
            NPCdeathCheck.NPCdeath();
            Destroy(this.gameObject);
            */
        }
    }



    public void initiateDeath()
    {
        /*
         * Runs general functions for on death
         */
        if (NPCdeathCheck != null)
        {
            NPCdeathCheck.NPCdeath();
        }

        // Destroy self and parent
        Destroy(this.gameObject);
        if (transform.parent != null && transform.parent.tag != "SpawnTrigger")
        {
            Destroy(transform.parent.gameObject);
        }
    }

    protected void itemDropLogic()
    {
        /*
         * If used, run before initiateDeath()
         * Spawns assigned gameobject(s) on this position
         * Uses dropOdds to determine if the object is spawned at a ratio of 1/x chance 
         * droppedOjects could be an array of objects or singular
         */

        for (int i = 0; i < droppedObejcts.Length; i++)
        {
            if (Random.Range(1, dropOdds) == 1)
            {
                // Sucess! Spawn object
                Instantiate(droppedObejcts[i], this.transform.position, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// <para>0 - Initiating</para>
    /// <para>1 - Targeting</para>
    /// <para>2 - Pathfinding</para>
    /// <para>3 - Moving</para>
    /// <para>4 - Attacking</para>
    /// <para>5 - Dead</para>
    /// </summary>
    public void changestate(int stateValue)
    {
        if (stateValue == 5)
        {
            // Death state, run one-off death functions
            itemDropLogic();
            initiateDeath();

            enemyState = (State)Mathf.Clamp(stateValue, 0, 5);
        }

        else if (enemyState != State.Dead)
        {
            enemyState = (State)Mathf.Clamp(stateValue, 0, 5);
        }
    }

    protected bool attackCooldwonLogic()
    {
        /*
         * Counts down the attack timer
         * Returns true if attackCooldown is reached
         * Run every update
         */

        if (attackCooldownValue > 0f)
        {
            // Countdown attack
            attackCooldownValue -= Time.deltaTime;
            return false;
        }
        else
        {
            // Reset attack cooldown
            attackCooldownValue = attackCooldown;
            return true;
        }
    }

    virtual public bool playerCollisionCheck(Collider2D collider)
    {
        /*
         * Call in CollisionEnter2D()
         * Checks if the collision is the player
         * Deals damage to the player based on bump attack value
         */

        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerStats>().TakeDamage(bumpDamage);
            return true;
        }

        return false;
    }

    /*
     * Enemy colour change on hit
     */
    protected void ChangeEnemyColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;

        // Alternatively, trigger an animation
        //anim.SetTrigger("Hit");

        StartCoroutine(ResetHitState());
    }

    private IEnumerator ResetHitState()
    {
        yield return new WaitForSeconds(showHitDuration);

        // Reset the "Hit" trigger
        //anim.ResetTrigger("Hit");

        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
