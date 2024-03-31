using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingCrosshair : MonoBehaviour
{
    int timer = 0;
    public int damage = 20;
    public SpriteRenderer theSprite;
    public bool isBlue = false;
    // Start is called before the first frame update
    void Start()
    {
        theSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer = timer + 1;
        
        if (theSprite.color == Color.white)
        {
            if (isBlue)
            {
                theSprite.color = Color.blue;
            }
            else
            {
                theSprite.color = Color.red;
            }
        }
        else
        {
            theSprite.color = Color.white;
        }
        if (!isBlue)
        {
            if (timer == 60)
            {
                if (!isBlue)
                {
                    summonHitbox();
                }
            }
            if (timer > 60)
            {
                Object.Destroy(this.gameObject);
            }
        }
    }

    private void summonHitbox()
    {
        // Load the hitbox
        GameObject hitboxPrefab = Resources.Load<GameObject>("SuperHitBox");

        if (hitboxPrefab != null)
        {
            // Instantiate the hitbox prefab
            GameObject hitboxInstance = Instantiate(hitboxPrefab, transform.position, Quaternion.identity);

            hitboxInstance.transform.parent = transform; // used to make it a child (hitbox sticks to the entity)
            // Access the Hitbox script on the instance to set its variables
            SuperHitboxScript hitboxScript = hitboxInstance.GetComponent<SuperHitboxScript>();

            if (hitboxScript != null)
            {
                // Set relevant variable information for the hitbox (IMPORTANT)
                hitboxScript.damageAmount = damage;
                hitboxScript.size = new Vector2(0.6f, 0.6f); // these numbers need to be very small
                hitboxScript.rotationAngle = transform.eulerAngles.z;
                hitboxScript.offsetAmount = new Vector2(0f, 0f);
                hitboxScript.lifetime = 0.1f;
                hitboxScript.deleteOnConnect = true; // make sure this is true
            }
            else
            {
                Debug.LogError("brokey");
            }
        }
        else
        {
            Debug.LogError("dont worke");
        }
    }
}
