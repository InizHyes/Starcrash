using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{
    public float activationTime = 7.5f;
    private bool isActivated = false;
    private float currentTime = 0f;

    // Color identifier for the pressure plate
    public ColorType colorType;

    public bool isFirstPressurePlateActivated = false;

    // Reference to the script that handles the global integer
    public GlobalIntegerScript globalIntegerScript;

    // Variable to check if this is the first pressure plate
    public bool isFirstPressurePlate = false;

    private void Update()
    {
        if (isActivated)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= activationTime)
            {
                if (isFirstPressurePlate)
                {
                    Debug.Log("First pressure plate (" + colorType + ") activated.");
                    globalIntegerScript.IncreaseGlobalInteger(colorType);
                    isFirstPressurePlateActivated = true;
                }

                ResetPressurePlate();
            }
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched the pressure plate.");
            isActivated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ResetPressurePlate();
        }
    }

    private void ResetPressurePlate()
    {
        isActivated = false;
        currentTime = 0f;
    }
    public void ActivateSecondPressurePlate()
    {
        Debug.Log("Second pressure plate (" + colorType + ") activated.");

        if (isFirstPressurePlateActivated && isActivated)
        {
            globalIntegerScript.IncreaseGlobalInteger();
            Debug.Log("Second pressure plate activated. Global integer increased.");
            isFirstPressurePlateActivated = false; // Reset for future activations
        }
        // Implement the logic for activating the second pressure plate
    }

}

// Enum to represent different color types
public enum ColorType
{
    Orange,
    Blue,
    Purple,
    Green
}