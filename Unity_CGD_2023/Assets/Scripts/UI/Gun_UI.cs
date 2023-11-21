using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Sets the sprite variables
    public Sprite sp1, sp2, sp3;
  
    // Update is called once per frame
    void Update()
    {
        // If a key is pressed
        // The sprite will change 
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetComponent<SpriteRenderer>().sprite = sp1;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            GetComponent<SpriteRenderer>().sprite = sp2;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<SpriteRenderer>().sprite = sp3;
        }
    }
}
