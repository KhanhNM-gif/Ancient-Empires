using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Asset.Model;
using System;
using System.Linq;

public class AttackPlate : MonoBehaviour, IMatrixCoordi,IPlateAction
{
    public GameObject controller;
    public Unit reference { get; set; }
    Unit target = null;

    public int x { get; set; }
    public int y { get; set; }

    public void OnMouseDown()
    {
        Handle();
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
    public void SetTarget(Unit obj) => target = obj;
    public Unit GetTarget() => target;

    public void Handle()
    {
        Unit.DisablePlate();
        MapManager.map.arrTile[reference.x, reference.y].AttackAble = true;
        MapManager.map.arrTile[this.x, this.y].AttackAble = false;
        reference.SetIsAttack(false);
        reference.AttackToUnit(target, out float damage);
        reference.AnimationAttack(target, damage);
    }
}
