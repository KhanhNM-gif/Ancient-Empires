using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ArrowMove : MonoBehaviour
{
    public float speed = 5;
    bool moving;
    public GameObject fire;
    Vector3 lastClickedPos;
    public void Update()
    {
        if (moving && transform.position != lastClickedPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, lastClickedPos, step);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, lastClickedPos - transform.position);
        }
        else
        {
            moving = false;
        }
        if (transform.position == lastClickedPos)
        {
            GameObject f = Instantiate(fire, lastClickedPos, Quaternion.identity);
            Destroy(f, f.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
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
    public void GetSpeed(float speed)
    {
        speed = this.speed;
    }

}

