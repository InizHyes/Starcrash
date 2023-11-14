using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    public int damage = 1;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(3, 7);
        Physics2D.IgnoreLayerCollision(7, 7);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("Bullet Collided with Object");
        // If collision with enemy, call damageDetection() and deal damage
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyClass>().damageDetection(damage);
        }
        Destroy(this.gameObject);
    }

    /*
     * Redundant
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
    */
}
