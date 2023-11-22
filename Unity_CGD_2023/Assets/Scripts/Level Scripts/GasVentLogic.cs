using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using UnityEngine.ParticleSystemJobs;

// Not finished

public class GasVentLogic : MonoBehaviour
{
    public Tilemap floorTileMap;
    public Tilemap liquidTileMap;

    //ContainsTile	Returns true if the Tilemap contains the given Tile. Returns false if not.
    //PlayerController scriptName;
    PlayerController plrScript;
    public GameObject plr;

    public ParticleSystem ParticleSystem;
    private ParticleSystem.EmissionModule emission;
    //PartSystem.emit = true;

    private bool positive = true;

    void Awake()
    {
        emission = ParticleSystem.emission;
        emission.enabled = true;
        //ParticleSystem.Play();
      //  ParticleSystem.transform.localScale = new Vector2(0.5f, 0.5f);
        // scriptName = gameObject.GetComponent<PlayerController>();
        //ParticleSystem = gameObject.GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
     plr = GameObject.Find("Player");
     plrScript = plr.gameObject.GetComponent<PlayerController>();
    }

    // Only checks for collisions on liquid tile?
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.GetType() == typeof(CircleCollider2D))
        {
            if (collision.gameObject.tag == "Player")
            {
                // var plrScript = collision.gameObject.GetComponent<PlayerController>();
                // plrScript.MoveSpeed /= 2; // Doesn't work so great since the way force is added to the player
                ParticleSystem.Play();
                print("ForceToApply " + plrScript.ForceToApply.x + " // " + plrScript.ForceToApply.y + emission.enabled);
                //If positive turn negative
                // if negative turn positive

                //// Is Positive
                //if (plrScript.ForceToApply.x > 0)
                //{
                //    plrScript.ForceToApply.x *= -1f;
                //    print("is this negative " + plrScript.ForceToApply.x);
                //}
                //else
                //{
                //    plrScript.ForceToApply.x = Mathf.Abs(plrScript.ForceToApply.x);
                //    print("is this positive " + plrScript.ForceToApply.x);
                //}

               

            
       


                //Take damage

                //plrScript.ForceToApply = new Vector2(0.000f, 0.000f);

                print("Player colliding with gas floor " + plrScript.MoveSpeed + " // " + plrScript.ForceToApply);
               
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //TilemapCollider2D collision
    {
        if (collision.GetType() == typeof(CircleCollider2D))
        {
            if (collision.gameObject.tag == "Player")
            {
                //Reset to default speed
                ParticleSystem.Stop();
                print("Player not colliding with gas floor");

            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.GetType() == typeof(CircleCollider2D) 
            && collision.gameObject.tag == "Player")
        {
            //StartCoroutine(plrScript.stickTimer2());
            //plrScript.sticking = false;
            // Is Positive
            if (plrScript.ForceToApply.x > 0) //&& !positive
            {
                //positive = true;
                plrScript.ForceToApply.x *= -1f;
                print("is this negative " + plrScript.ForceToApply.x);
            }
            else if (plrScript.ForceToApply.x < 0) //&& positive
            {
                //positive = false;
                plrScript.ForceToApply.x = Mathf.Abs(plrScript.ForceToApply.x);
                print("is this positive " + plrScript.ForceToApply.x);
            }

            print("particles " + ParticleSystem.isPlaying);
            ParticleSystem.transform.position = collision.gameObject.transform.position;
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
