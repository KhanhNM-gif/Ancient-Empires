using Assets.Asset.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour, IMatrixCoordi,IPlateAction
{
    public GameObject controller;
    public Unit reference { get; set; }
    public Queue<IMatrixCoordi> queueWay;
    public int x { get; set; }
    public int y { get; set; }

    public void Start() { }

    public void OnMouseDown()
    {
        Prepare();
        Handle();
        Unit.DisablePlate();
    }

    public void Handle()
    {
        reference.SetStackMove(queueWay);//kích hoạt event di chuyển đến x,y mới
        reference.SetAttack();
    }

    public void Prepare()
    {
        MapManager.map.arrTile[reference.x, reference.y].MoveAble = true;
        MapManager.map.arrTile[this.x, this.y].MoveAble = false;
        reference.x = this.x;
        reference.y = this.y;
        reference.SetIsMove(false);
    }

    /// <summary>
    /// Xét vị trí của ô moveplate(dung spawn o di chuyển)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetCoords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void SetStackWay(Queue<IMatrixCoordi> queue) => queueWay = queue;

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
