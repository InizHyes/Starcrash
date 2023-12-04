using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//myParticlePrefab references the GasRoomParticleSystem in the Game Systems Prefabs folder
//GasLeakSOund is in auidoFolder put this as the clip on the audiSource on liquid floor on gasRoom

public class GasVentLogic : MonoBehaviour
{
    public Tilemap liquidTileMap;
    public ParticleSystem myParticlePrefab;

    private ParticleSystem.EmissionModule emission;
    private Dictionary<GameObject, ParticleSystem> particleDictionary = new Dictionary<GameObject, ParticleSystem>();

    public float speedReductionInGas = 0.001f;
    public float gasVentDamage = 0.1f;
    private float speedBeforeEnteredGas;

    //Audio
    public AudioSource audioSource;

    // Enter Gas logic
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.GetType() == typeof(CircleCollider2D))
        {
            if (collision.gameObject.tag == "Player")
            {
                //Play gas sound if no players have entered gas yet
                if (particleDictionary.Count == 0)
                    audioSource.PlayOneShot(audioSource.clip);

                //Instantiate a new particleSystem prefab &
                //store current player's gameObject and particleSystem as key values in a dictionary
                ParticleSystem myParticle = Instantiate(
                    myParticlePrefab,
                    collision.gameObject.transform.position,
                    Quaternion.identity);
                particleDictionary.Add(collision.gameObject, myParticle);

                //Enable the particle
                emission = myParticle.emission;
                emission.enabled = true;
                myParticle.Play();

                // Reduce some player stats 
                var plrScript = collision.GetComponent<PlayerController>();
                speedBeforeEnteredGas = plrScript.MoveSpeed;
                plrScript.MoveSpeed = speedReductionInGas;       
            }
        }
    }

    // Stay in Gas logic
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.GetType() == typeof(CircleCollider2D)
            && collision.gameObject.tag == "Player")
        {
            // Take damage
            var plrStats = collision.GetComponent<PlayerStats>(); //I know this is not efficient but will do for now
            plrStats.TakeDamage(gasVentDamage);

            //Prevent clamping in gas
            //plrScript.sticking = false; // Doesn't work obviously needs to work in future

            // Gets the correct particleSystem by indexing players GameObject
            // as the key to get the value which is the ParticleSystem instantiated earlier 
            particleDictionary[collision.gameObject].transform.position = collision.gameObject.transform.position;
        }
    }
    
    // Exit Gas logic
    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.GetType() == typeof(CircleCollider2D))
        {
            if (collision.gameObject.tag == "Player")
            {
                //Reset to default speed
                var plrScript = collision.GetComponent<PlayerController>();
                particleDictionary[collision.gameObject].Stop();

                //Removes the current player's key value pair from dictionary and destroys their particleSystem
                if (particleDictionary.Remove(collision.gameObject, out ParticleSystem particleSystem))
                {
                    particleSystem.Stop();
                    Destroy(particleSystem);
                }

                //Stop gas sound if there are no players in gas
                if (particleDictionary.Count == 0)
                 audioSource.Stop();

                // Set players speed to normal
                plrScript.MoveSpeed = speedBeforeEnteredGas; 
            }
        }
    }
}

    //plrScript.MoveSpeed 
    //plrScript.ForceToApply
    //plrScript.MoveForce2 

