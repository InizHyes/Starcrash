using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerManager : MonoBehaviour
{
    public int playerCount = 0;
 

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        playerCount++;  ///adds to playercount, can keep track of how many players in the game
        if (playerCount == 1)   ///these if and else assign coloured based on playercount
        {
            playerInput.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
        }
        else if (playerCount == 2)
        {
            playerInput.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);

        }
        
    }
}