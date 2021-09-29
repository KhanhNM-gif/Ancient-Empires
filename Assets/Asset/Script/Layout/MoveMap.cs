using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private SpriteRenderer map;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private Vector3 dragOrigin;

    private void Awake()
    {
        mapMinX = map.transform.position.x - map.bounds.size.x / 2f;
        mapMaxX = map.transform.position.x + map.bounds.size.x / 2f;

        mapMinY = map.transform.position.y - map.bounds.size.y / 2f;
        mapMaxY = map.transform.position.y + map.bounds.size.y / 2f;
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
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            print("origin " + dragOrigin + " newPosition " + cam.ScreenToWorldPoint(Input.mousePosition) + " =difference" + difference);

            cam.transform.position = Clampcamera(cam.transform.position + difference);
        }
    }

    private Vector3 Clampcamera(Vector3 target)
    {
        float camH = cam.orthographicSize;
        float camW = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camW;
        float maxX = mapMaxX - camW;
        float minY = mapMinY + camH;
        float maxY = mapMaxY - camH;

        float newX = Mathf.Clamp(target.x, minX, maxX);
        float newY = Mathf.Clamp(target.y, minY, maxY);

        return new Vector3(newX, newY, target.z);
    }
}
