using Assets.Asset.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour, MatrixCoordi
{
    public GameObject controller;
    Unit reference = null;
    private Queue<MatrixCoordi> queueWay;
    public int x { get; set; }
    public int y { get; set; }

    public void Start() { }

    public void OnMouseDown()
    {
        reference.x = this.x;
        reference.y = this.y;
        reference.SetStackMove(queueWay);
        reference.DestroyMovePlate();
    }

    public void SetCoords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void SetReference(Unit obj) => reference = obj;
    public void SetStackWay(Queue<MatrixCoordi> queue) => queueWay = queue;
    public Unit GetReference() => reference;
}
