using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(3, 7);
        Physics2D.IgnoreLayerCollision(7, 7);
    }
    [Header("BulletDamage")]
    public int damage = 10;

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
            Destroy(this.gameObject);


            // Destroy the bullet on impact with the player
            Destroy(gameObject);
        }

        if (other.tag == "Walls")
        {
            // Destroy the bullet on impact with the Walls
            Destroy(this.gameObject);
        }
    }

}
