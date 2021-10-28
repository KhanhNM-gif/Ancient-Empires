using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Assets.Asset.Script.Effect;

public class ArrowMove : Attack
{
    override public void Update()
    {
        if (moving)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, lastPos, step);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, lastPos - transform.position);

            if (transform.position == lastPos)
            {
                moving = false;
                EffectAfterAttack();
            }
        }
    }

}

