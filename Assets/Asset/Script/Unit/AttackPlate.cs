using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream
using Assets.Asset.Model;
using System;

public class AttackPlate : MonoBehaviour , MatrixCoordi
{
    public GameObject controller;
    Unit reference = null;
    private Queue<MatrixCoordi> queueAtk;
    public int x { get; set; }
    public int y { get; set; }

    void Start(){ }

    public void OnMouseDown()
    {
        MapManager.map.arrTile[reference.x, reference.y].AttackAble = true;
        MapManager.map.arrTile[this.x, this.y].AttackAble = false;

        reference.x = this.x;
        reference.y = this.y;
        reference.DestroyAttackPlate();
=======

public class AttackPlate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
>>>>>>> Stashed changes
    }
}
