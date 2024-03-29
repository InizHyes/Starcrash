using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyRazorBlade : MonoBehaviour
{
    GameObject recentlyHit1 = null;
    GameObject recentlyHit2 = null;
    GameObject recentlyHit3 = null;
    GameObject recentlyHit4 = null;
    public GameObject particles;
    int timer = 60;
    public string direction = "up";
    float speed = 0.03f;
    public float speed2 = 5; // EDIT THIS ONE, NOT SPEED
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    [Header("BulletDamage")]
    public int damage = 30;
    private void Update()
    {
        speed = speed2 / 50;
        if (timer < 60)
        {
            timer = timer + 1;
        }
        else
        {
            recentlyHit1 = null;
            recentlyHit2 = null;
            recentlyHit3 = null;
            recentlyHit4 = null;
        }

        transform.Rotate(0, 0, 1000 * Time.deltaTime);
        if (direction == "up")
        {
            rb.velocity = Vector2.up * speed2;
        }
        else if (direction == "left")
        {
            rb.velocity = Vector2.left * speed2;
        }
        else if (direction == "down")
        {
            rb.velocity = Vector2.down * speed2;
        }
        else if (direction == "right")
        {
            rb.velocity = Vector2.right * speed2;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //print("Hit");
            if (other == recentlyHit1)
            {
            }
            else if (other == recentlyHit2)
            {
            }
            else if (other == recentlyHit3)
            {
            }
            else if (other == recentlyHit4)
            {
            }
            else
            {
                // Get the PlayerHealth component from the colliding GameObject
                PlayerStats playerStats = other.GetComponent<PlayerStats>();

                // Check if the PlayerHealth component is not null
                if (playerStats != null)
                {
                    // Deal damage to the player
                    playerStats.TakeDamage(damage);

                    if (recentlyHit1 == null)
                    {
                        recentlyHit1 = other.gameObject;
                        timer = 0;
                    }
                    else if (recentlyHit2 == null)
                    {
                        recentlyHit2 = other.gameObject;
                        timer = 0;
                    }
                    else if (recentlyHit3 == null)
                    {
                        recentlyHit3 = other.gameObject;
                        timer = 0;
                    }
                    else if (recentlyHit4 == null)
                    {
                        recentlyHit4 = other.gameObject;
                        timer = 0;
                    }

                }
            }

            //Destroy(gameObject);
        }

        ///if (other.tag == "Walls")
        //{
        //    
        //}
        
        if (other.gameObject.layer == 6) // the wall layer, in case walls aren't tagged
        {
            Instantiate(particles, transform.position, Quaternion.identity);
            // change direction on impact with the Walls
            if (direction == "up")
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - (speed * 2 + 0.05f));
                direction = "left";
            }
            else if (direction == "left")
            {
                transform.position = new Vector2(transform.position.x + (speed * 2 + 0.05f), transform.position.y);
                direction = "down";
            }
            else if (direction == "down")
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + (speed * 2 + 0.05f));
                direction = "right";
            }
            else if (direction == "right")
            {
                transform.position = new Vector2(transform.position.x - (speed * 2 + 0.05f), transform.position.y);
                direction = "up";
            }
        }
    }

}
