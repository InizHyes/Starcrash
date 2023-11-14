using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    // Enemy common variables
    public int health;
    public GameObject target;
    public GameObject targetfollow;

    // rb movement variables
    private Vector2 forceToApply;
    [HideInInspector] public Vector2 moveForce;
    public float forceMultiplier = 1f;
    public Vector2 maxVelocity = new Vector2(100f, 100f);
    public Rigidbody2D rb;

    // Set spawnlogic prefab onto spawnLogic, will find and assign script to NPCdeathCheck
    public GameObject spawnLogic;
    public SpawnLogic NPCdeathCheck;


    // States
    public enum State
    {
        Initiating, // Can be used to freeze enemies while the player is loading into the room
        Targeting, // Running script to find nearest player on first spawn, or change targeting to closer player on cone collision
        Pathfinding, // Calculating pathfinding around objects
        Moving, // Will be in this state 90% of the time, moving towards target player
        Attacking, // Run attack animation, will prevent Sans-like attacking (-1 hp every frame)
        Dead // Dead. Run drop item script (may be part of this code)
    }
    public State enemyState;

    public void initiateEnemy(int iHealth)
    {
        /*
         * Assignes and runs all the common variables/functions between all enemy types
         * Health, state, spawnLogic, etc.
         * Set health with the iHealth variable
         */

        enemyState = State.Initiating;
        health = iHealth;
        rb = GetComponent<Rigidbody2D>();

        NPCdeathCheck = spawnLogic.GetComponent<SpawnLogic>();
        if (NPCdeathCheck != null)
        {
            NPCdeathCheck.NPCdeath();
        }
    }

    public void targetClosestPlayer()
    {
        /*
         * Finds the closest object with the tag "Player" and sets "target" as that player
         */
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float lowestDistance = 0;
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

    public void targetRangedClosestPlayer()
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

    // Function will allow for stronger enemies to hide behind grunts for tactical play,
    // But seprate moveTowardsTarget0G function may need to be set up to allow,
    // For the stronger enemey to follow grunt whilst keeping the correct target at the player.
    // Unless simple bug fixes can be made without out getting to complex!
    public void targetClosestGrunt()
    {
        /*
         * Finds the closest object with the tag "grunt" and sets "targetfollow" as that grunt
         */
        GameObject[] grunts = GameObject.FindGameObjectsWithTag("grunts");
        float lowestDistance = 1;
        for (int i = 0; i < grunts.Length; i++)
        {
            //If targetfollow isnt set or distance is lower for other grunt, set grunt as targetfollow
            if (targetfollow == null || Vector3.Distance(this.transform.position, grunts[i].transform.position) < lowestDistance)
            {
                targetfollow = grunts[i];
                lowestDistance = Vector3.Distance(this.transform.position, grunts[i].transform.position);
            }
            // Else do nothing
        }
        enemyState = State.Targeting;
    }

    public void moveTowardsTarget0G()
    {
        // If not at max velocity
        if (rb.velocity.x < maxVelocity.x && rb.velocity.y < maxVelocity.y)
        {
            // Use target position and add to forceToApply
            forceToApply = ((target.transform.position - this.transform.position).normalized) * forceMultiplier;
            // Add every frame for excelleration (/100 cause too fast)
            moveForce += forceToApply / 100;
            rb.velocity = moveForce;
        }
    }

    public void slowDownAndStop()
    {
        rb.velocity *= 0.98f;
        moveForce = rb.velocity;
    }
    public void lungeForward()
    {
        if (rb.velocity.x < maxVelocity.x && rb.velocity.y < maxVelocity.y)
        {
            forceToApply = ((target.transform.position - this.transform.position).normalized) * forceMultiplier;
            moveForce += forceToApply;
            rb.velocity = moveForce;
        }
    }

    public void damageDetection(Collision2D collision)
    {
        /*
         * Detects when hit by an object with the "Bullet" tag
         * ---Currently just destorys the enemy on collision---
         */

        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
        }
        deathCheck();
    }

    public void deathCheck()
    {
        // Check if dead after damage detection
        if (health == 0 && spawnLogic != null)
        {
            NPCdeathCheck.NPCdeath();
        }
    }
}
