using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCone : MonoBehaviour
{
    public bool playerconeTriggered = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerconeTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerconeTriggered = false;
        }
    }

}