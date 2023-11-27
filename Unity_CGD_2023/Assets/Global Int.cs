using UnityEngine;

public class GlobalIntegerScript : MonoBehaviour
{
    private int globalInteger = 0;

    private int orangeTubesDestroyed = 0;
    private int blueTubesDestroyed = 0;
    private int purpleTubesDestroyed = 0;
    private int greenTubesDestroyed = 0;

    public PressurePlateScript orangePressurePlate;
    public PressurePlateScript bluePressurePlate;
    public PressurePlateScript purplePressurePlate;
    public PressurePlateScript greenPressurePlate;

    private bool orangeActivated = false;
    private bool blueActivated = false;
    private bool purpleActivated = false;
    private bool greenActivated = false;

    public void IncreaseGlobalInteger(ColorType color)
    {
        switch (color)
        {
            case ColorType.Orange:
                orangeTubesDestroyed++;
                break;
            case ColorType.Blue:
                blueTubesDestroyed++;
                break;
            case ColorType.Purple:
                purpleTubesDestroyed++;
                break;
            case ColorType.Green:
                greenTubesDestroyed++;
                break;
        }

        CheckForActivation(color);
    }

    private void CheckForActivation(ColorType color)
    {
        switch (color)
        {
            case ColorType.Orange:
                if (orangeTubesDestroyed >= 4)
                {
                    orangePressurePlate.ActivateSecondPressurePlate();
                    orangeTubesDestroyed = 0; // Reset for future activations
                }
                break;
            case ColorType.Blue:
                if (blueTubesDestroyed >= 4)
                {
                    bluePressurePlate.ActivateSecondPressurePlate();
                    blueTubesDestroyed = 0; // Reset for future activations
                }
                break;
            case ColorType.Purple:
                if (purpleTubesDestroyed >= 4)
                {
                    purplePressurePlate.ActivateSecondPressurePlate();
                    purpleTubesDestroyed = 0; // Reset for future activations
                }
                break;
            case ColorType.Green:
                if (greenTubesDestroyed >= 4)
                {
                    greenPressurePlate.ActivateSecondPressurePlate();
                    greenTubesDestroyed = 0; // Reset for future activations
                }
                break;
        }
    }

    private bool IsColorActivated(ColorType color)
    {
        switch (color)
        {
            case ColorType.Orange:
                return orangeActivated;
            case ColorType.Blue:
                return blueActivated;
            case ColorType.Purple:
                return purpleActivated;
            case ColorType.Green:
                return greenActivated;
            default:
                return false;
        }
    }

    public void IncreaseGlobalInteger()
    {
        globalInteger++;
        Debug.Log("Global Integer Increased: " + globalInteger);

        // You can add any additional logic here when the global integer increases.
    }

  
}