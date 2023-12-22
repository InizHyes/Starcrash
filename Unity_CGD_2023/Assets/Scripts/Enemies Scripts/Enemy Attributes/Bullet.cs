using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            print("Hit");

            // Destroy the bullet on impact with the player
            Destroy(gameObject);
        }
    }

}
