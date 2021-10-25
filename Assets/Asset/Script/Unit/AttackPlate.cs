using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Asset.Model;
using System;
using System.Linq;

public class AttackPlate : MonoBehaviour, MatrixCoordi
{
    public GameObject controller;
    Unit reference = null;
    Unit target = null;

    public int x { get; set; }
    public int y { get; set; }

    public void OnMouseDown()
    {   
        Unit.DisablePlate();
        MapManager.map.arrTile[reference.x, reference.y].AttackAble = true;
        MapManager.map.arrTile[this.x, this.y].AttackAble = false;
        reference.SetIsAttack(false);
        reference.AttackToUnit(target);
        reference.AnimationAttack();
       
    }

    /// <summary>
    /// Xét vị trí của ô AttackPlate(dung spawn o di chuyển)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetCoords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void SetReference(Unit obj) => reference = obj;
    public Unit GetReference() => reference;
    public void SetTarget(Unit obj) => target = obj;
    public Unit GetTarget() => target;
}
