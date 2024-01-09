using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraSystem : MonoBehaviour
{
    public Transform targetPosition;
    public float cameraSpeed = 2.5f;
    public string targetTag = "CameraTarget";

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        FindNewTarget();
    }

    void Update()
    {
        FindNewTarget();
    }

    Transform FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            return player.transform;
        }
        else
        {
            return null;
        }
    }

    void FindNewTarget()
    {
        GameObject[] emptyObjects = GameObject.FindGameObjectsWithTag(targetTag);
        if(emptyObjects.Length == 0)
        {
            Debug.LogWarning("NO TARGETS FOR CAMERA");
            return;
        }

        Transform playerTransform = FindPlayer();
        if(playerTransform == null)
        {
            Debug.LogWarning("NO PLAYER FOUND");
            return;
        }

        GameObject nearestObject = null;
        float shortestDistance = float.MaxValue;

        foreach (GameObject emptyObject in emptyObjects)
        {
            float distance = Vector2.Distance(playerTransform.position, emptyObject.transform.position);
            if(distance < shortestDistance) 
            { 
                shortestDistance = distance;
                nearestObject = emptyObject;
            }
        }
        if(nearestObject != null)
        {
            Debug.Log("Next room found");
            targetPosition = nearestObject.transform;
            MoveCameraToTarget();
        }
    }

    void MoveCameraToTarget()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition.position, cameraSpeed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
    }
}
