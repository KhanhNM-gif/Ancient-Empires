using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
    private Camera cam;

    private float camH, camW, minX, maxX, minY, maxY, mapMinX, mapMaxX, mapMinY, mapMaxY;

    [SerializeField]
    public MapManager map;

    private Vector3 dragOrigin;

    private void Start()
    {
        cam = Camera.main;

        MapManager swapMap = Instantiate(map, new Vector3(0, 0), Quaternion.identity);
        swapMap.name = name;
        swapMap.ReadAndAddMap();

        camH = cam.orthographicSize;
        camW = cam.orthographicSize * cam.aspect;

        mapMinX = swapMap.transform.position.x - 0.5f;
        mapMaxX = swapMap.transform.position.x + swapMap.GetWidth() - 0.5f;
        minX = mapMinX + camW;
        maxX = mapMaxX - camW;

        mapMinY = swapMap.transform.position.y - 0.5f;
        mapMaxY = swapMap.transform.position.y + swapMap.GetHeight() - 0.5f;
        minY = mapMinY + camH;
        maxY = mapMaxY - camH;

        gameObject.transform.position = new Vector3(swapMap.GetWidth() / 2f - 0.5f, swapMap.GetHeight() / 2f - 0.5f, -10);
    }
    private void Update()
    {
        Pancamera();
    }
    private void Pancamera()
    {
        if (Input.GetMouseButtonDown(0))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - GetWorldPostion(-10f);

            cam.transform.position = Clampcamera(cam.transform.position + difference);
        }
    }

    private Vector3 Clampcamera(Vector3 target)
    {


        float newX; float newY;

        if (2f * camW > mapMaxX - mapMinX) newX = cam.transform.position.x;
        else newX = Mathf.Clamp(target.x, minX, maxX);

        if (2f * camH > mapMaxY - mapMinY) newY = cam.transform.position.y;
        else newY = Mathf.Clamp(target.y, minY, maxY);

        return new Vector3(newX, newY, target.z);
    }

    private Vector3 GetWorldPostion(float z)
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane group = new Plane(Vector3.forward, new Vector3(0, 0, z));

        group.Raycast(mousePos, out float distance);

        return mousePos.GetPoint(distance);
    }
}
