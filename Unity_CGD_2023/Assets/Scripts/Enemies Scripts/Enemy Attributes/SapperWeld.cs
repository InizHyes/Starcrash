using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapperEMP : MonoBehaviour
{

// Used when after orignal attack type has ended with the player
    [SerializeField] private int burnDamage = 1; // Used when if afterATKbool is true

    public void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject[] Generator = GameObject.FindGameObjectsWithTag("Generator");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");


        if (collision.gameObject.tag == "Generator")
        {
            //Increase Genetator health
        }

        if (collision.gameObject.tag == "Player")
        {
            //Do small damage to player overtime (Burning?)

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
    }

}