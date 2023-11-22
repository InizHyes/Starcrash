using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCamera : MonoBehaviour
{
    public float zoomStart = 15f; // when the camera zoom begins
    public float minZoom = 5f; // Minimum camera zoom in
    public float maxZoom = 10f; // Maximum camera zoom out
    public float zoomInLerpSpeed = 5f; // Speed of zooming in
    public float zoomOutLerpSpeed = 10f; // Speed of zooming out

    private Camera mainCamera;
    //private float minX, maxX, minY, maxY;
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

    //UpdateCameraPosition updates the camera position to always be focused on the point between all the players.
    void UpdateCameraPosition()
    {
        Vector3 centerPoint = GetCenterPoint();
        transform.position = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);
    }

    //GetCenterPoint gets the central point between all players in the game.
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

    //UpdateCameraZoom adjusts the zoom level of the camera based on the positions of the players.
    void UpdateCameraZoom()
    {
        float distance = GetPlayersDistance();
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomStart);

        float lerpSpeed = (targetZoom > mainCamera.orthographicSize) ? zoomInLerpSpeed : zoomOutLerpSpeed;

        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, Time.deltaTime * lerpSpeed);
    }

    //GetPlayersDistance gets the distance between all the players in the game.
    float GetPlayersDistance()
    {
        Bounds bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.size.x;
    }

    //FindPlayers adds objects with the tag "Player" to the player objects list so the camera can adjust for more players.
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
