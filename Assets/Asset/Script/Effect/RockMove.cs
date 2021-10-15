using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMove : MonoBehaviour
{
    public float speed = 1;
    bool moving;
    Vector2 lastClickedPos;
    public void Update()
    {
        if (moving && (Vector2)transform.position != lastClickedPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, lastClickedPos, step);
        }
        else
        {
            moving = false;
        }
        if ((Vector2)transform.position == lastClickedPos)
        {
            Destroy(gameObject);
        }
    }
    public void SetMoving(bool moving)
    {
        this.moving = moving;
    }
    public void SetlastClickedPos(Vector3 lastClickedPos)
    {
        this.lastClickedPos = Camera.main.ScreenToWorldPoint(lastClickedPos);
    }

}

