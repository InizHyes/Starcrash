using UnityEngine;
using System.Collections;


public class ScreenEdgeColliders : MonoBehaviour
{
    //For now when setting up the camera it will have to start on the same size as the max zoom out size.
    //Im still looking for a way to have it dynamically change to the correct size from the start.
    //-Cameron
    void Awake()
    {
        AddCollider();
    }

    void AddCollider()
    {
    if (Camera.main == null)
    { 
        Debug.LogError("Camera.main not found, colliders can not be made"); 
        return; 
    }

    var cam = Camera.main;
    if (!cam.orthographic) 
    { 
         Debug.LogError("Camera.main is not Orthographic, collidees can not be made"); 
         return; 
    }

    var bottomLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
    var topLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
    var topRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
    var bottomRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));

    var edge = GetComponent<EdgeCollider2D>() == null ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();

    var edgePoints = new[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
    edge.points = edgePoints;
    }
}
