using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFly : Projectile
{
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Vector3 vectorTo;
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
        if (Input.GetMouseButtonDown(1) && !isFlying)
        {
            Vector3 mouse = Input.mousePosition;
            vectorTo = Camera.main.ScreenToWorldPoint(mouse);
            vectorTo.z = transform.position.z;
            vectorFrom = transform.position;
            float x = vectorTo.x - vectorFrom.x;
            float y = vectorTo.y - vectorFrom.y;
            distanceMax = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
            change.x = x / distanceMax;
            change.y = y / distanceMax;

            //float Z= AngleVectorOx(change);*/
            myRigidbody.transform.Rotate(0, 0, AngleVectorOx(change) - transform.rotation.eulerAngles.z);

            isFlying = true;
        }

        if (isFlying) Move();
    }

    void Move()
    {
        float step =  speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, vectorTo, step);

        float distanse= Vector3.SqrMagnitude(transform.position - vectorTo);
        if (distanse < 0.5) isFlying = false;
    }

    float AngleVectorOx(Vector3 vector) => (vectorTo.y - vectorFrom.y < 0 ? 1 : -1) * Mathf.Acos(vector.x * -1 / Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y)) * 180 / Mathf.PI;


}
