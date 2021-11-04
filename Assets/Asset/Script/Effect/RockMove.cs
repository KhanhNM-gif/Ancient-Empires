using Assets.Asset.Script.Effect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMove : Attack
{
    override public void Update()
    {
        if (moving && transform.position != lastPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, lastPos, step);
        }
        else
        {
            moving = false;
        }
        if (transform.position == lastPos)
        {
            EffectAfterAttack();
        }
    }
}

