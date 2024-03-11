using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    public int damage = 1;
    public float knockBackForceFloat;
    
    [SerializeField]
    private float bulletTimeBeforeDestroy;

    private void Start()
    {
        
        Physics2D.IgnoreLayerCollision(3, 7);
        Physics2D.IgnoreLayerCollision(7, 7);
        Destroy(this.gameObject, bulletTimeBeforeDestroy);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BulletCollision(collision.gameObject);
    }

    public void BulletCollision(GameObject collision)
    {
        GetComponent<SFX>().PlaySound("");

        //print("Bullet Collided with Object");
        // If collision with enemy, call damageDetection() and deal damage
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyClass>().damageDetection(damage);
            collision.GetComponent<Rigidbody2D>().AddForce(transform.forward * knockBackForceFloat, ForceMode2D.Impulse);


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
