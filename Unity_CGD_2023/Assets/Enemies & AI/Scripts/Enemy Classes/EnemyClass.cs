using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    // Enemy common variables
    public int health;
    public GameObject target;

    // rb movement variables
    private Vector2 forceToApply;
    private Vector2 moveForce;
    public float forceMultiplier = 1f;
    public Vector2 maxVelocity = new Vector2(100f, 100f);
    public Rigidbody2D rb;

    // Set spawnlogic prefab onto NPCdeathCheck, will find and assign script to spawnLogic
    public GameObject NPCdeathCheck;
    public SpawnLogic spawnLogic;


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
}
