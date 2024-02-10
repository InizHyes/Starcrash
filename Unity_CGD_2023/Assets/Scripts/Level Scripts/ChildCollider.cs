using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Sends messages to the colourFloor or GasVentLogic script whenever the player/enemy triggers a collision with the floor

public class ChildCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        this.transform.parent.SendMessage("ReceiveOnTriggerEnter", collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        this.transform.parent.SendMessage("ReceiveOnTriggerStay", collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        this.transform.parent.SendMessage("ReceiveOnTriggerExit", collision);
    }
}
