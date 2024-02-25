using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : MonoBehaviour
{
    [Header("Strike")]
    public int damage = 25;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            print("Hit");

            // Get the PlayerHealth component from the colliding GameObject
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            // Check if the PlayerHealth component is not null
            if (playerStats != null)
            {
                // Deal damage to the player
                playerStats.TakeDamage(damage);
            }

        }
    }
}
