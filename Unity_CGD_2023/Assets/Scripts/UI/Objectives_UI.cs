using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Objectives : MonoBehaviour
{
    // Varaibles
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the text to false on play
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Sets the text to true when a key is pressed
        if (Input.GetKeyDown(KeyCode.O))
        {
            text.enabled = true;
        }
        // Sets the text to false when a key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            text.enabled = false;
        }
    }
}
