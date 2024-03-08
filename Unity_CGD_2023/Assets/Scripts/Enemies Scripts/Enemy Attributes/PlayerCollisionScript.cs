using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    /*
     * Due to layer overides, player trigger detection must be controled by a child object with rb and trigger
     * Can overwrite "parentScript" in the inspector if needed.
     */

    [Tooltip("Overwrite is not needed if the parent has the EnemyClass script")] [SerializeField] private EnemyClass parentScript;
    [SerializeField] private bool destroyBullets = false;

    private void Start()
    {
        if (parentScript == null)
        {
            parentScript = GetComponentInParent<EnemyClass>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyBullets && collision.tag == "Bullet")
        {
            Destroy(collision.gameObject);
        }

        parentScript.playerCollisionCheck(collision);
    }
}
