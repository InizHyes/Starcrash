using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public Vector3 spinAxis = Vector3.up; // Axis to spin around
    public float spinSpeed = 60f; // Spin speed in degrees per second
    public float bounceForce = 5f; // Force applied to bounce the player

    // Update is called once per frame
    void Update()
    {
        // Calculate rotation amount based on spin speed and frame time
        float spinAmount = spinSpeed * Time.deltaTime;

        // Rotate the object around the specified axis
        transform.Rotate(spinAxis, spinAmount, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a player object
        if (other.CompareTag("Player"))
        {
            // Calculate the bounce direction (opposite to the spin axis)
            Vector3 bounceDirection = -spinAxis.normalized;

            // Apply force to bounce the player
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
            }
        }
    }
}
