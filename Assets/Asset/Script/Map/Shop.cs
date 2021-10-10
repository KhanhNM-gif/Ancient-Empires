using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Asset.Model;
using UnityEngine.UI;
using System;

public class Shop : MonoBehaviour, MatrixCoordi
{

    [SerializeField] private GameObject PannelShop;
    public int x { get; set; }
    public int y { get; set; }

    private int CountArcher = 0;
    private int CountValadorn = 0;
    private int CountSolider = 0;
    private int CountCatapult = 0;
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

    // Update is called once per frame
    void Update()
    {
        TextMonney.text = GetPlayerHandle().Gold + "";
        if (GetPlayerHandle().Gold < Const.ConstGame.COST_SOLDIER)
        {
            ButtonBuyArcher.interactable = false;
            ButtonBuySolider.interactable = false;
            ButtonBuyValadorn.interactable = false;
            ButtonBuyCatapultr.interactable = false;
        }
        else if (GetPlayerHandle().Gold < Const.ConstGame.COST_ARCHER)
        {
            ButtonBuyArcher.interactable = true;
            ButtonBuySolider.interactable = false;
            ButtonBuyValadorn.interactable = false;
            ButtonBuyCatapultr.interactable = false;
        }
        else if (GetPlayerHandle().Gold < Const.ConstGame.COST_CATAPULT)
        {
            ButtonBuyArcher.interactable = true;
            ButtonBuySolider.interactable = true;
            ButtonBuyValadorn.interactable = false;
            ButtonBuyCatapultr.interactable = false;
        }
        else if (GetPlayerHandle().Gold < Const.ConstGame.COST_GENERAL)
        {
            ButtonBuyArcher.interactable = true;
            ButtonBuySolider.interactable = true;
            ButtonBuyValadorn.interactable = true;
            ButtonBuyCatapultr.interactable = false;
        }
        else 
        {
            ButtonBuyArcher.interactable = true;
            ButtonBuySolider.interactable = true;
            ButtonBuyValadorn.interactable = true;
            ButtonBuyCatapultr.interactable = true;
        }
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
        ResetTextCountBuyUnit();
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


    public static Tuple<int, int>[] POSITION_SPAWN = new Tuple<int, int>[]
         {
            new Tuple<int, int>(0,0),
            new Tuple<int, int>(1,0),
            new Tuple<int, int>(0,1),
            new Tuple<int, int>(-1,0),
            new Tuple<int, int>(0,-1),
         };

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

        PlayerHandle playerHandle = GetPlayerHandle();

        foreach (var item in POSITION_SPAWN)
        {
            if (MapManager.map.arrTile[x + item.Item1, y + item.Item2].MoveAble == true)
            {
                if (GameManager.Instance.addUnit(playerHandle, nameUnit, x + item.Item1, y + item.Item2, false))
                {
                    SetTextCountBuyUnit(nameUnit);
                }
                //playerHandle-Gold
                return;
            }
        }

    }


    /// <summary>
    /// Gom code dung chung, tra ve turn cua player hay bot
    /// </summary>
    /// <returns></returns>
    private static PlayerHandle GetPlayerHandle()
    {
        PlayerHandle playerHandle;
        if (GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player) playerHandle = GameManager.Instance.player;
        else playerHandle = GameManager.Instance.bot;
        return playerHandle;
    }

    //Set text so luong mua trong lan nay
    private void SetTextCountBuyUnit(string nameUnit)
    {
        switch (nameUnit)
        {
            case Const.NameUnit.BLUE_ARCHER:
                TextCountBuyArcher.text = "[" + ++CountArcher + "]";
                break;
            case Const.NameUnit.BLUE_SOLDIER:
                TextCountBuySolider.text = "[" + ++CountSolider + "]";
                break;
            case Const.NameUnit.BLUE_GENERAL:
                TextCountBuyValadorn.text = "[" + ++CountValadorn + "]";
                break;
            case Const.NameUnit.BLUE_CATAPULT:
                TextCountBuyCatapultr.text = "[" + ++CountCatapult + "]";
                break;
        }
    }

    //Reset so luong mua khi tat shop
    private void ResetTextCountBuyUnit()
    {
        TextCountBuyArcher.text = "";
        TextCountBuySolider.text = "";
        TextCountBuyValadorn.text = "";
        TextCountBuyCatapultr.text = "";
    }
}
