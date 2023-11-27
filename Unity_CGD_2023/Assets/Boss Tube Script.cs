using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossTubeLogic : TubeLogic
{
    [Header("Boss Tube Specific")]
    [Header("Color Pressure Plate")]
    public PressurePlateScript connectedPressurePlate;

    public GlobalIntegerScript globalIntegerScript;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (connectedPressurePlate != null && connectedPressurePlate.isFirstPressurePlateActivated && IsBroken == false)
        {
            // Check if the collision is with the object you want to trigger the destruction
            if (collision.gameObject.CompareTag("Bullet"))
            {
                IsBroken = true;
                entitySpawn();
            }
        }
    }
}
