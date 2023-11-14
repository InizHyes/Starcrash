using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(3, 7);
        Physics2D.IgnoreLayerCollision(7, 7);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            print("Bullet Collided with Object");
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}