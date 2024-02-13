using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeHbeam : MonoBehaviour
{
    protected MedicalDroidClass MedicalDroid;

    private void Start()
    {
        MedicalDroid = GetComponentInParent<MedicalDroidClass>();
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

        if (collision.gameObject.tag == "Player")
        {
            MedicalDroid.changestate(3); // Moving
        }
    }
}
