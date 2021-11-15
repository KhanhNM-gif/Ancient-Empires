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
        Handle();
    }

    public void Handle()
    {
        //BinhBh khi di chuyen den thi set vi tri day khong di chuyen dc nua de nhung unit khac khong di vao dc
        //Khi di ra thi set co the di chuyen dc
        MapManager.map.arrTile[reference.x, reference.y].MoveAble = true;
        MapManager.map.arrTile[this.x, this.y].MoveAble = false;
        //BinhBH end
        reference.x = this.x;
        reference.y = this.y;
        reference.SetIsMove(false);
        reference.SetStackMove(queueWay);//kích hoạt event di chuyển đến x,y mới
        reference.SetAttack();
        Unit.DisablePlate();
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
}
