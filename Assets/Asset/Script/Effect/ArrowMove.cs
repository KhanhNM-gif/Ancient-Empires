using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    public float speed = 1;
    public int rotationSpeed;
    public float _horizontalInput = 0;
    public float _verticalInput = 0;
    bool moving;
    Rigidbody2D rb2D;
    Vector2 lastClickedPos;
    public void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); 
    }
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
    private void SetInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }
}

