using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
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
    //private FollowMidpoint midpoint;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraZoom();
    }

    void UpdateCameraZoom()
    {
        float distance = 50f;
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomStart);

        float lerpSpeed = (targetZoom > mainCamera.orthographicSize) ? zoomInLerpSpeed : zoomOutLerpSpeed;

        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, Time.deltaTime * lerpSpeed);
    }
}
