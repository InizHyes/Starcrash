using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TbeamBehaviour : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
            // Disable movement but not rotation,
            // Players should be able to fight back and escape by themsveles or help from other free players, but with diffculty

            //Debug.Log("Player is in tractor beam");
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            playerRigidbody.constraints = RigidbodyConstraints2D.None;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            //Debug.Log("Player is not in tractor beam");
        }
    }

}