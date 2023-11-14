using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 2f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("NO PLAYER TO FOLLOW");
            return;
        }

        Vector3 targetPosition = player.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
