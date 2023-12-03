using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDropBehaviour : MonoBehaviour
{
    /*
     * Oliver Chalk
     * Currently an all-in-one ammot type
     * On collision give ammo to player
     */

    [Tooltip("Ammount of ammo given within a range")][SerializeField][Range(0, 100)] private int[] ammoRange = { 1, 10 };
    AudioSource audioPlayer;
    [SerializeField] private AudioClip ammoPickup;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            audioPlayer = collision.GetComponent<AudioSource>();
            audioPlayer.clip = ammoPickup; ///audio pickup noise, simple so change if wanted
            audioPlayer.Play();
            print("hello");
            /*
             * "Pseudocode" for giving ammo to the player
             * Re-write where needed
             * 
            // Get variables
            AmmoHolder ammoHolder = collision.GetComponent<AmmoHolder>();
            int randomAmount = Random.Range(ammoRange[0], ammoRange[1]);

            // Check if over ammo count limit
            if (randomAmount > ammoHolder.maxAmmo - ammoHolder.currentAmmo)
            {
                // If so make current = max
                ammoHolder.currentAmmo = ammoHolder.maxAmmo;
            }
            else
            {
                // If not, give all ammo
                ammoHolder.currentAmmo += randomAmount;
            }
            */

            Destroy(this.gameObject);
        }
    }
}
