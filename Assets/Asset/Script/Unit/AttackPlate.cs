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
    public Unit target = null;
    float damage;

    public int x { get; set; }
    public int y { get; set; }

    public void OnMouseDown()
    {
        Unit.DisablePlate();
        Prepare();
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
        reference.AnimationAttack(target, damage);
        reference.AddExp(damage);
        if (reference.CheckDisable()) reference.DisableUnit();
    }

    public void Prepare()
    {
        reference.SetIsAttack(false);
        reference.AttackToUnit(target, out damage);
        reference.virtualHP -= damage;
        if (reference.virtualHP <= 0)
        {
            MapManager.map.arrTile[reference.x, reference.y].MoveAble = true;
            reference.isDead = true;
        }
    }
}
