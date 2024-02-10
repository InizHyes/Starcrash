using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HbeamBehaviour : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject[] Enemy = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");


        if (collision.gameObject.tag == "Enemy")
        {
            //Increase Enemy health
        }

        if (collision.gameObject.tag == "Player")
        {
            //Do small damage to player overtime (Posion?)

        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        
    }

}