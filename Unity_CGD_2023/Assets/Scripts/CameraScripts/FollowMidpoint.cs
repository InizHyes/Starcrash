using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMidpoint : MonoBehaviour
{
    public Transform midpoint;
    public float smoothSpeed = 2f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private void LateUpdate()
    {
        if (midpoint == null)
        {
            Debug.LogWarning("NO MIDPOINT TO FOLLOW");
            return;
        }

        Vector3 targetPosition = midpoint.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
