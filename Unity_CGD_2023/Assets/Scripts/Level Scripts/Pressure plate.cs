using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public float activationTime = 2f;  // Set activation time in the Inspector
    public Door connectedDoor;  // Reference to the connected door
    private bool isActivated = false;
    private bool canActivate = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated && canActivate)
        {
            StartCoroutine(ActivatePlate());
            Debug.Log("player on plate");
        }
    }

    private IEnumerator ActivatePlate()
    {
        yield return new WaitForSeconds(activationTime);

        isActivated = true;
        canActivate = false;
        connectedDoor.IncreaseActivatedPlates();  // Notify the connected door
        Debug.Log("Pressure plate activated!");
    }

    public bool IsActivated()
    {
        return isActivated;
    }
}