using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploderAOE : MonoBehaviour
{
    /*
     * On activate flash 4 times
     * Linger and explode on 5th
     * If player in radius deal damage to them
     * Set Exploder to dead - exploder will linger for a few seconds
     */

    // Dont deal damage on trigger until 5th flash
    private bool dealDamage;

    private ExploderClass exploderAttached;
    private SpriteRenderer sprite;
    private CircleCollider2D circleCollider;

    [SerializeField] private GameObject attachToObject;

    [SerializeField] private int flashAmount = 5;
    private int flashCounter;
    [SerializeField] private float flashDelay = 0.2f;
    private float flashTimer;

    // Hit targets list to prevent "juggling" the player between a wall and the explosion
    private List<Collider2D> hitTargets;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        exploderAttached = attachToObject.GetComponent<ExploderClass>();

        dealDamage = false;
        flashCounter = 0;
        flashTimer = 0f;

        hitTargets = new List<Collider2D>();
    }

    private void Update()
    {
        transform.position = attachToObject.transform.position;
    }

    private void LateUpdate()
    {
        if (!dealDamage)
        {
            // Count timer and on reset reset flash state
            if (flashTimer < flashDelay)
            {
                flashTimer += Time.deltaTime;
            }
            else
            {
                sprite.enabled = !sprite.enabled;
                circleCollider.enabled = !circleCollider.enabled;
                flashTimer = 0f;
                flashCounter += 1;
            }

            if (flashCounter > flashAmount + 2)
            {
                dealDamage = true;
                exploderAttached.deathStateChange();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Don't deal damage until charge up finishes
        if (dealDamage)
        {
            // If target hasn't been hit yet
            if (!hitTargets.Contains(collision))
            {
                // Check if it's a player or enemy
                if (collision.tag == "Player")
                {
                    // Push back target
                    Vector2 forceNormal = (this.transform.position - collision.transform.position).normalized;
                    //collision.GetComponent<PlayerController>().ForceToApply = (forceNormal * exploderAttached.explosionForce * -1f);
                    Player newPlayer = collision.GetComponent<Player>();
                    newPlayer.rb.AddForce(forceNormal * exploderAttached.explosionForce * -1f);

                    // Deal damage
                    collision.GetComponent<PlayerStats>().TakeDamage(exploderAttached.explosionDamage);

                    // Add to list
                    hitTargets.Add(collision);
                }

                if (collision.tag == "Enemy")
                {
                    // Push back target
                    /*
                    Vector2 forceNormal = (this.transform.position - collision.transform.position).normalized;
                    collision.GetComponentInChildren<EnemyClass>().moveForce = Vector2.zero;
                    collision.attachedRigidbody.velocity = (forceNormal * explosionForce * -1f);
                    */

                    // Deal damage
                    collision.GetComponentInChildren<EnemyClass>().damageDetection(exploderAttached.explosionDamage);

                    // Add to list
                    hitTargets.Add(collision);
                }
            }
        }
    }
}
