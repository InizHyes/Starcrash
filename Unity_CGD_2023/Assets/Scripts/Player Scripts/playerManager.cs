using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerManager : MonoBehaviour
{
    public int playerCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        playerCount++;
        print(playerCount);
        if (playerCount == 1)
        {
            playerInput.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
        }
        else if (playerCount == 2)
        {
            playerInput.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);

        }
        
    }
}
