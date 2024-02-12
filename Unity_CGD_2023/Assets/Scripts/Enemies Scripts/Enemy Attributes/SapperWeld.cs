using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapperEMP : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Do small damage to player overtime

        //Increase Genetator health

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
    }

}