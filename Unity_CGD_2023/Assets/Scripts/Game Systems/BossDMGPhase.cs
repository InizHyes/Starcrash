using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDMGPhase : MonoBehaviour
{
    private int activationCounter = 0;
    private List<string> activatedPlates = new List<string>();

    public int requiredPlatesToOpen = 4; // Adjust this value in the Unity Editor

    
    

    public void IncreaseCounter()
    {
        activationCounter++;

        // Check if the required number of pressure plates are activated
        if (activationCounter == requiredPlatesToOpen)
        {
            Debug.Log($"Required number of plates ({requiredPlatesToOpen}) activated. Opening the door!");
            PlayerDPSPhase();
        }
    }

    public void AddActivatedPlate(string plateColor)
    {
        if (activationCounter < requiredPlatesToOpen)
        {

            if (!activatedPlates.Contains(plateColor))
            {
                activatedPlates.Add(plateColor);
                Debug.Log(plateColor + " pressure plate added to the activated list.");
                
            }
            else if (activatedPlates.Contains(plateColor))
            {

                IncreaseCounter();
                Debug.Log(activationCounter);
                activatedPlates.Clear();
            }
        }
    }

    public bool IsTubeColorActivated(string tubeColor)
    {
        return activatedPlates.Contains(tubeColor);
    }


    public void RemoveActivatedPlate(string plateColor)
    {
        if (activatedPlates.Contains(plateColor))
        {
            activatedPlates.Remove(plateColor);
            Debug.Log(plateColor + " pressure plate removed from the activated list.");
        }
    }

    private void PlayerDPSPhase()
    {
        //Add player damage phase
        this.gameObject.GetComponent<BossClass>().setVulnerability(true);
    }

    public void MechanicReset()
    {
        //MechanicReset pressure plates and tubes
        activationCounter = 0;
        //BossTubeLogic.IsBroken = false;

    }

}
