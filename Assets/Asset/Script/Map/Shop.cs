using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Asset.Model;
using UnityEngine.UI;
using System;

public class Shop : MonoBehaviour, IMatrixCoordi
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
    [SerializeField] private Image ImageGeneral;
    [SerializeField] private Text TextNumberUnit;

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
        Unit.DisablePlate();
        showNumberOfUnit();
        x = i;
        y = j;
        ActiveBuyUnit();

        PannelShop.gameObject.SetActive(true);
        UIManager.Instance.SetActivePnStatus(false);
    }

    private void ActiveBuyUnit()
    {
        int gold = GameManager.Instance.player.Gold;

        if (GameManager.Instance.player.hasGeneral)
        {
            ImageGeneral.gameObject.SetActive(false);
        }
        else
        {
            ImageGeneral.gameObject.SetActive(true);
        }
        if (gold < Const.ConstGame.COST_SOLIDER)
        {
            ButtonBuySolider.gameObject.SetActive(false);
            ButtonBuyArcher.gameObject.SetActive(false);
            ButtonBuyCatapultr.gameObject.SetActive(false);
            ButtonBuyValadorn.gameObject.SetActive(false);
        }
        else if (gold < Const.ConstGame.COST_ARCHER)
        {
            ButtonBuySolider.gameObject.SetActive(true);
            ButtonBuyArcher.gameObject.SetActive(false);
            ButtonBuyCatapultr.gameObject.SetActive(false);
            ButtonBuyValadorn.gameObject.SetActive(false);
        }
        else if (gold < Const.ConstGame.COST_CAPUTAL)
        {
            ButtonBuySolider.gameObject.SetActive(true);
            ButtonBuyArcher.gameObject.SetActive(true);
            ButtonBuyCatapultr.gameObject.SetActive(false);
            ButtonBuyValadorn.gameObject.SetActive(false);
        }
        else if (gold < Const.ConstGame.COST_GENERAL)
        {
            ButtonBuySolider.gameObject.SetActive(true);
            ButtonBuyArcher.gameObject.SetActive(true);
            ButtonBuyCatapultr.gameObject.SetActive(true);
            ButtonBuyValadorn.gameObject.SetActive(false);
        }
        else
        {
            ButtonBuySolider.gameObject.SetActive(true);
            ButtonBuyArcher.gameObject.SetActive(true);
            ButtonBuyCatapultr.gameObject.SetActive(true);
            ButtonBuyValadorn.gameObject.SetActive(true);
        }
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
        TextNumberUnit.text = GameManager.Instance.getNumberUnit() + "/" + Const.ConstGame.MAX_UNIT;
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
    /// </summary>
    /// <param name="nameUnit"> ten quan muon mua</param>
    public void Buy(string nameUnit)
    {
        showNumberOfUnit();
        if (GameManager.Instance.getNumberUnit() == Const.ConstGame.MAX_UNIT)
        {
            return;
        }
        int goldBuyUnit = 0;
        switch (nameUnit)
        {
            case Const.NameUnit.BLUE_ARCHER:
                TextCountBuyArcher.text = "[" + ++CountArcher + "]";
                goldBuyUnit = Const.ConstGame.COST_ARCHER;
                break;
            case Const.NameUnit.BLUE_SOLDIER:
                TextCountBuySolider.text = "[" + ++CountSolider + "]";
                goldBuyUnit = Const.ConstGame.COST_SOLIDER;
                break;
            case Const.NameUnit.BLUE_CATAPULT:
                TextCountBuyCatapultr.text = "[" + ++CountCatapult + "]";
                goldBuyUnit = Const.ConstGame.COST_CAPUTAL;
                break;
            case Const.NameUnit.BLUE_GENERAL:
                TextCountBuyValadorn.text = "[" + ++CountValadorn + "]";
                goldBuyUnit = Const.ConstGame.COST_GENERAL;
                break;
        }


        PlayerHandle playerHandle;
        if (GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player) playerHandle = GameManager.Instance.player;
        else playerHandle = GameManager.Instance.bot;

        foreach (var item in POSITION_SPAWN)
        {
            if (MapManager.map.arrTile[x + item.Item1, y + item.Item2].MoveAble == true)
            {
                if(GameManager.Instance.addUnit(playerHandle, nameUnit, x + item.Item1, y + item.Item2,
                    GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Bot))
                {
                    playerHandle.Gold -= goldBuyUnit;
                    UIManager.Instance.UpdateGold(GameManager.Instance.player.Gold);
                    MapManager.map.arrTile[x + item.Item1, y + item.Item2].MoveAble = false;
                    ActiveBuyUnit();
                    showNumberOfUnit();
                }
                return;
            }
        }
        

    }





}
