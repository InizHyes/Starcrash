using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    public int damage = 1;
    public float knockBackForceFloat;

    AudioSource audio;

    [SerializeField]
    private AudioClip bulletHit;

    [SerializeField]
    private float bulletTimeBeforeDestroy;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        Physics2D.IgnoreLayerCollision(3, 7);
        Physics2D.IgnoreLayerCollision(7, 7);
        Destroy(this.gameObject, bulletTimeBeforeDestroy);
        audio.clip = bulletHit;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        audio.Play();

        //print("Bullet Collided with Object");
        // If collision with enemy, call damageDetection() and deal damage
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyClass>().damageDetection(damage);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.forward * knockBackForceFloat, ForceMode2D.Impulse);

            
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
