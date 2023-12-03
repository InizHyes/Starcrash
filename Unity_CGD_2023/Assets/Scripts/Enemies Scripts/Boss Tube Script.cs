using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossTubeLogic : TubeLogic
{
    [Header("Boss Tube Specific")]
    [Header("Color Pressure Plate")]
   
    private bool connectedPlate;
    public string TubeColor;

    public BossDMGPhase TubeCol;

    
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (TubeCol != null && TubeCol.IsTubeColorActivated(TubeColor))
        {
            Debug.Log(TubeCol);
            // Check if the collision is with the object you want to trigger the destruction
            if (collision.gameObject.CompareTag("Bullet"))
                {
                    IsBroken = true;
                    entitySpawn();
                }
            
        }
    }
}
