using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeHbeam : MonoBehaviour
{
    protected MedicalDroidClass MedicalDroid;

    private void Start()
    {
        MedicalDroid = GetComponentInParent<MedicalDroidClass>();

        //tractor.//Referance function;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject[] Enemy = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            MedicalDroid.changestate(4); // Attacking 
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //tractor.changestate(1); // Targeting

        MedicalDroid.changestate(3); // Moving
    }
}
