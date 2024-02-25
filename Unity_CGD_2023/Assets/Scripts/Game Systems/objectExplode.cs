using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectExplode : MonoBehaviour
{

    public int health = 1;
    private int healthCurrent;
    public GameObject particles;
   
    void Start()
    {
        healthCurrent = health;

    }

   

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            healthCurrent--;
            if (healthCurrent <= 0)
            {
                Instantiate(particles, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

        }

    }
}
