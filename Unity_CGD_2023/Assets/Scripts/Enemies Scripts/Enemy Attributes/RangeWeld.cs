using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RangeWeld : MonoBehaviour
{
    protected SapperClass sapper;

    private void Start()
    {
        sapper = GetComponentInParent<SapperClass>();

        //tractor.//Referance function;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject[] Generator = GameObject.FindGameObjectsWithTag("Generator");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Generator")
        {
            sapper.changestate(4); // Attacking 
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //tractor.changestate(1); // Targeting

        sapper.changestate(3); // Moving
    }
}
