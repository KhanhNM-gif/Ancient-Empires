using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFly : Projectile
{
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Vector3 lastClickedPos;
    private Vector3 vectorFrom;
    private bool isFlying;
    float distanceMax;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        isFlying = false;
        Application.targetFrameRate = Const.ConstGame.FRAME;
    }


    // Update is called once per frame
    void Update()
    {       
        if (isFlying)
        {
            // lastClickedPos.z = transform.position.z;
            vectorFrom = transform.position;
            float x = lastClickedPos.x - vectorFrom.x;
            float y = lastClickedPos.y - vectorFrom.y;
            distanceMax = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
            change.x = x / distanceMax;
            change.y = y / distanceMax;

            //float Z= AngleVectorOx(change);*/
            myRigidbody.transform.Rotate(0, 0, AngleVectorOx(change) - transform.rotation.eulerAngles.z);
            Move();
        } 
    }

    void Move()
    {
        float step =  speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, lastClickedPos, step);

        float distanse= Vector3.SqrMagnitude(transform.position - lastClickedPos);
        if (distanse < 0.5) isFlying = false;
    }

    float AngleVectorOx(Vector3 vector) => (lastClickedPos.y - vectorFrom.y < 0 ? 1 : -1) * Mathf.Acos(vector.x * -1 / Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y)) * 180 / Mathf.PI;

    public void SetMoving(bool isFlying)
    {
        this.isFlying = isFlying;
    }
    public void SetlastClickedPos(Vector3 lastClickedPos)
    {
        this.lastClickedPos = Camera.main.ScreenToWorldPoint(lastClickedPos);
    }

}
