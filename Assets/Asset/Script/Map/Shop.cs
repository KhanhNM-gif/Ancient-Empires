using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Asset.Model;
using UnityEngine.UI;

public class Shop : MonoBehaviour, MatrixCoordi
{

    [SerializeField] private GameObject PannelShop;
    public int x { get; set; }
    public int y { get; set; }

    private int CountArcher = 0; 
    private int CountValadorn = 0;
    private int CountSolider = 0;
    private int CountCatapult= 0;
    [SerializeField] private Text TextCountBuyArcher;
    [SerializeField] private Text TextCountBuySolider;
    [SerializeField] private Text TextCountBuyValadorn;
    [SerializeField] private Text TextCountBuyCatapultr;
    [SerializeField] private Text TextMonney;
    [SerializeField] private Text TextUnit;

    [SerializeField] private Button ButtonBuyArcher;
    [SerializeField] private Button ButtonBuySolider;
    [SerializeField] private Button ButtonBuyValadorn;
    [SerializeField] private Button ButtonBuyCatapultr;
    private void Awake()
    {
        GameManager.shop = this;
    }
    
    /// <summary>
    /// An shop
    /// </summary>
    public void showShop(int i, int j)
    {
        showNumberOfUnit();
        x = i;
        y = j;
        PannelShop.gameObject.SetActive(true);
    }


    /// <summary>
    /// Hien thi shop
    /// </summary>
    public void HideShop()
    {
        CountArcher = 0;
        CountValadorn = 0;
        CountSolider = 0;
        CountCatapult = 0;
        PannelShop.gameObject.SetActive(false);
    }


    /// <summary>
    /// Hien thi da co bao nhieu Unit tren tong so maxUnit
    /// </summary>
    private void showNumberOfUnit()
    {
        TextUnit.text = GameManager.Instance.getNumberUnit() + "/" + Const.ConstGame.MAX_UNIT;
    }

    /// <summary>
    /// BinhBH Mua quan xung quan vi tri thanh,
    /// uu tien vi tri trong thanh va ben duoi thanh
    /// </summary>
    /// <param name="nameUnit"> ten quan muon mua</param>
    public void Buy(string nameUnit)
    {
        showNumberOfUnit();
        if (GameManager.Instance.getNumberUnit() == Const.ConstGame.MAX_UNIT)
        {
            return;
        }

        switch (nameUnit)
        {
            case Const.NameUnit.BLUE_ARCHER:
                CountArcher++;
                TextCountBuyArcher.text = "[" + CountArcher +"]";
                break;
        }

        if(MapManager.map.arrTile[x,y].MoveAble == true)
        {
            GameManager.Instance.addUnit(nameUnit, x, y);
            return;
        }
        else if (MapManager.map.arrTile[x, y-1].MoveAble == true)
        {
            GameManager.Instance.addUnit(nameUnit, x, y-1);
            return;
        }
        for (int i = x+1; i >= x - 1; i--)
        {
            for (int j = y -1; j <= y + 1; j++)
            {
                if (MapManager.map.arrTile[i, j].MoveAble == true)
                {
                    GameManager.Instance.addUnit(nameUnit, i, j);
                    return;
                }
            }
        }

    }





}
