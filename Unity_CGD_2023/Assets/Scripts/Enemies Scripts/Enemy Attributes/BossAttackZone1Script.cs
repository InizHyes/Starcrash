using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttackZone1Script : MonoBehaviour
{
    private BossClass parentScript;
    private List<PlayerStats> playersInZone;
    private float ticksPerSecondValue;

    private void Start()
    {
        parentScript = GetComponentInParent<BossClass>();

        ticksPerSecondValue = 1 / parentScript.ticksPerSecond;

        playersInZone = new List<PlayerStats>();
    }

    private void Update()
    {
        if (TPSLogic())
        {
            for (int i = playersInZone.Count - 1; i >= 0; i--)
            {
                playersInZone[i].TakeDamage(parentScript.attack1Damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playersInZone.Add(collision.gameObject.GetComponent<PlayerStats>());
        }

        /*
        // If trigger is player
        if (collision != null && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(parentScript.attack1Damage);
        }
        */
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playersInZone.Remove(collision.gameObject.GetComponent<PlayerStats>());
        }
    }

    private bool TPSLogic()
    {
        /*
         * Counts down the attack timer
         * Returns true if attackCooldown is reached
         * Run every update
         */

        if (ticksPerSecondValue > 0f)
        {
            // Countdown attack
            ticksPerSecondValue -= Time.deltaTime;
            return false;
        }
        else
        {
            // Reset attack cooldown
            ticksPerSecondValue = 1 / parentScript.ticksPerSecond;
            return true;
        }
    }
}
