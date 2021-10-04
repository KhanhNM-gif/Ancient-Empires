using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera cam;
    private float tagertZoom;
    private float zoomFactor = 3f;
    private float zoomLerpZoom = 10f;

    void Start()
    {
        cam = Camera.main;
        tagertZoom = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float ScrollData = Input.GetAxis("Mouse ScrollWheel");

        tagertZoom -= ScrollData * zoomFactor;
        tagertZoom = Mathf.Clamp(tagertZoom, 4f, 6.5f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, tagertZoom, Time.deltaTime * zoomLerpZoom);
    }
}
