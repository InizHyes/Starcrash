using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeStar : MonoBehaviour
{
    protected QuantumShadowClass QuantumShadow;

    private void Start()
    {
        QuantumShadow = GetComponentInParent<QuantumShadowClass>();

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (collision.gameObject.tag == "Player")
        {
            QuantumShadow.changestate(4); // Attacking 
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        QuantumShadow.changestate(1); // Targeting
    }
}
