using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMove : Catapult
{
    public float speed = 10f;
    Vector2 lastClickedPos;
    bool moving;
    public void move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moving = true;
        }
        if (moving && (Vector2)transform.position != lastClickedPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, lastClickedPos, step);
        }
        else
        {
            moving = false;
        }
    }
    public void Update()
    {
        move();
    }

}
