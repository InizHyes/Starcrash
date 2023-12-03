using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    /*
     * Due to layer overides, player trigger detection must be controled by a child object with rb and trigger
     */

    private EnemyClass parentScript;

    private void Start()
    {
        parentScript = GetComponentInParent<EnemyClass>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parentScript.playerCollisonCheck(collision);
    }
}