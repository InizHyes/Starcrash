using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RangeTbeam : MonoBehaviour
{
    protected TractorClass tractor;

    private void Start()
    {
        tractor = GetComponentInParent<TractorClass>();

        //tractor.//Referance function;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (collision.gameObject.tag == "Player")
        {
            tractor.changestate(4); // Attacking 
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //tractor.changestate(1); // Targeting

        tractor.changestate(3); // Moving
    }
}
