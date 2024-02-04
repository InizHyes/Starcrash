using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCamera : MonoBehaviour
{
    private float zoomStart = 10f; // when the camera zoom begins
    [SerializeField]
    private float minZoom = 5f; // Minimum camera zoom in
    [SerializeField]
    private float maxZoom = 10f; // Maximum camera zoom out
    [SerializeField]
    private float zoomInLerpSpeed = 5f; // Speed of zooming in
    [SerializeField]
    private float zoomOutLerpSpeed = 10f; // Speed of zooming out

    private Camera mainCamera;
    private List<Transform> players = new List<Transform>();

    void Start()
    {
        mainCamera = GetComponent<Camera>();    
    }

    void Update()
    {
        FindPlayers();
        // Calculate the midpoint
        Vector3 midpoint = GetCenterPoint();


        // Set the position of this GameObject to the midpoint
        transform.position = midpoint;

        Debug.Log("Midpoint: " + midpoint);

        UpdateCameraZoom();
    }

    void UpdateCameraZoom()
    {
        float distance = GetPlayersDistance();
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomStart);

        float lerpSpeed = (targetZoom > mainCamera.orthographicSize) ? zoomInLerpSpeed : zoomOutLerpSpeed;

        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, Time.deltaTime * lerpSpeed);
    }

    Vector3 GetCenterPoint()
    {
        if (players.Count == 1)
            return players[0].position;

        Bounds bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.center;
    }

    float GetPlayersDistance()
    {
        Bounds bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.size.x;
    }

    void FindPlayers()
    {
        players.Clear();
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObject in playerObjects)
        {
            players.Add(playerObject.transform);
        }
    }
}

