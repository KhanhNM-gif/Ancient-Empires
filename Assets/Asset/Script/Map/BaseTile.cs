using Assets.Asset.Model;
using UnityEngine;

public class BaseTile : MonoBehaviour, IMatrixCoordi
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
    public int point;

    public int Distance(IMatrixCoordi mc)
    {
        throw new System.NotImplementedException();
    }
}
