using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStar : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Do large damage to player

            Destroy(gameObject);
        }
    }
}
