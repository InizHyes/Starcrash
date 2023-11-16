using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCamera : MonoBehaviour
{
    public float minZoom = 40f; // Minimum camera field of view
    public float maxZoom = 60f; // Maximum camera field of view
    public float zoomInLerpSpeed = 5f; // Speed of zooming in
    public float zoomOutLerpSpeed = 10f; // Speed of zooming out

    private Camera mainCamera;
    private float minX, maxX, minY, maxY;
    private List<Transform> players = new List<Transform>();

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        FindPlayers();
    }

    void Update()
    {
        if (players.Count == 0)
        {
            FindPlayers(); // In case players are dynamically added during gameplay
            return;
        }

        UpdateCameraPosition();
        UpdateCameraZoom();
    }

    void UpdateCameraPosition()
    {
        Vector3 centerPoint = GetCenterPoint();
        transform.position = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);
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

    void UpdateCameraZoom()
    {
        float distance = GetPlayersDistance();
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, distance / 10f);

        float lerpSpeed = (targetZoom > mainCamera.fieldOfView) ? zoomInLerpSpeed : zoomOutLerpSpeed;

        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetZoom, Time.deltaTime * lerpSpeed);
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
