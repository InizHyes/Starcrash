using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapperEMPDeath : MonoBehaviour
{



    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Do large damage to player once 

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            playerRigidbody.constraints = RigidbodyConstraints2D.None;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

}