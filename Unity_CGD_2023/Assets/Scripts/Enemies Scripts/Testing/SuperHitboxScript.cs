using UnityEngine;

public class SuperHitboxScript : MonoBehaviour
{
    public int damageAmount = 0;
    public Vector2 size = new Vector2(1f, 1f);
    public float rotationAngle = 0f;
    public float lifetime = 1f; 
    public bool deleteOnConnect = true; // destroys itself upon successfully hitting a player (dont make this false lol)
    public Vector2 offsetAmount = new Vector2(0f, 0f);

    private void Start()
    {
        // Set position
        transform.localPosition = transform.localPosition + new Vector3(offsetAmount.x, offsetAmount.y + 0f);

        // Set size
        transform.localScale = new Vector3(size.x, size.y, 1f);

        // Set rotation angle
        transform.Rotate(new Vector3(0, 0, rotationAngle));

        // Set lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            //Destroy(other); // for testing
            // Get the health component of the collided object and apply damage
            /*
            PlayerController playerAccess = other.GetComponent<PlayerController>();
            if (playerAccess != null)
            {
                // UN-CODE THE LINE BELOW WHEN THE PLAYER HAS A HEALTH VARAIBLE (and change .health it to what it is)
                
                //playerAccess.health = (playerAccess.health - damageAmount);
            }
            */
            print("A");
            other.GetComponent<PlayerStats>().TakeDamage(damageAmount);

            if (deleteOnConnect)
            {
                Destroy(gameObject);
            }
            // testing purposes
            
        }
        else if (other.CompareTag("Enemy"))
        {
            // Handle damaging an enemy if it's needed
        }
    }
}