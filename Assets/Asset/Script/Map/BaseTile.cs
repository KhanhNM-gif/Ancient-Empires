using Assets.Asset.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTile : MonoBehaviour , MatrixCoordi
{
    public int x { get; set; }
    public int y { get; set; }
    public int Ammor;
    public int ShootingRange;
    public int MoveRange;
    public int Heal;
    public bool MoveAble;
    public bool IsHouse;
    public bool IsCastle;
    public bool AttackAble;
}
