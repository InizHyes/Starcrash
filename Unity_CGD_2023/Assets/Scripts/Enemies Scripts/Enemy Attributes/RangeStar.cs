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

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (collision.gameObject.tag == "Player")
        {
            QuantumShadow.StartAttack();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            QuantumShadow.changestate(1); // Targeting
        }
    }
}
