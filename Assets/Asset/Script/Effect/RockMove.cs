using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMove : MonoBehaviour
{
    public float speed = 1;
    bool moving;
    public GameObject fire;
    Vector3 lastClickedPos;
    public void Update()
    {
        if (moving && transform.position != lastClickedPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, lastClickedPos, step);
        }
        else
        {
            moving = false;
        }
        if (transform.position == lastClickedPos)
        { 
            GameObject f = Instantiate(fire, lastClickedPos, Quaternion.identity);
            Destroy(f, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
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

