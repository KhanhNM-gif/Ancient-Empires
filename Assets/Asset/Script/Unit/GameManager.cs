using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum eStatus
    {
        Turn_Player,
        Turn_Bot
    }

    private ConcurrentDictionary<string, GameObject> UnitDictionary;
    public bool block;
    private Unit[,] PositionUnit = new Unit[100, 100];
    //private List<Unit> arrListUnit = new List<Unit>();
    public Bot bot { get; set; }
    public Player player { get; set; }
    public static GameManager Instance;
    public Unit UnitSelected;
    public static Shop shop;
    public static eStatus Status = eStatus.Turn_Player;
    public Queue<Unit> queue;


    public string MapName;

    //x=4-tầm đánh y=4-tầm đánh x4+tầm đánh y=4+tầm đánh  |xi-x||yi-y+|<=tầm đánh

    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        MapName = "MoveMap";
        UnitDictionary = new ConcurrentDictionary<string, GameObject>();

        GameObject[] arrUnit = Resources.LoadAll<GameObject>(@"ObjectGame/Unit");
        for (int i = 0; i < arrUnit.Length; i++)
            UnitDictionary[arrUnit[i].name] = arrUnit[i];
        player = new Player(Const.ConstGame.GOLD_START_GAME, 200);
        player.arrListUnit.Add(Create(Const.NameUnit.BLUE_GENERAL, 5, 3, false));
        player.hasGeneral = true;
        player.arrListUnit.Add(Create(Const.NameUnit.BLUE_SOLDIER, 2, 3, false));
        foreach (var item in player.arrListUnit) SetPosition(item);

        bot = new Bot(Const.ConstGame.GOLD_START_GAME, 200);
        bot.arrListUnit.Add(Create(Const.NameUnit.RED_GENERAL, 8, 8, true));
        bot.hasGeneral = true;
        bot.arrListUnit.Add(Create(Const.NameUnit.RED_SOLDIER, 6, 8, true));
        foreach (var item in bot.arrListUnit) SetPosition(item);


        if (MapManager.map.arrTile[4, 4].IsCastle)
        {
            ((Castle)MapManager.map.arrTile[4, 4]).changeOwner(1);
        }
        if (MapManager.map.arrTile[4, 4].IsCastle)
        {
            ((Castle)MapManager.map.arrTile[10, 9]).changeOwner(2);
        }
    }
    public Unit Create(string name, int x, int y, bool isEnemy)
    {

        if (UnitDictionary.TryGetValue(name, out GameObject outGameObject))
        {

            GameObject obj = Instantiate(outGameObject, new Vector3(0, 0, -1), Quaternion.identity);
            Unit un = obj.GetComponent<Unit>();
            un.name = name;
            if (name.Contains("General"))
            {
                un.isGeneral = true;
            }
            un.x = x;
            un.y = y;
            un.isEnemy = isEnemy;
            un.Activate();
            MapManager.map.arrTile[x, y].MoveAble = false;
            return un;
        }
        return null;
    }

    public void SetPosition(Unit obj)
    {
        Unit unit = obj.GetComponent<Unit>();
        int x = unit.x;
        int y = unit.y;
        if (x < 0) x = 0;
        if (y < 0) y = 0;
        PositionUnit[x, y] = obj;
    }

    public void EndTurn()
    {
        foreach (var item in player.arrListUnit)
        {
            item.EnableColor();
        }
        Unit.DisablePlate();
        block = false;
    }


    public void StartTurn()
    {
        Status = eStatus.Turn_Player;
        bot.StartTurn();
        player.StartTurn();
    }


    public eStatus GetStatus() => Status;
    public void SetStatus(eStatus status) => Status = status;

    public void SetPositionEmpty(int x, int y)
    {
        if (x < 0 || y < 0) return;
        PositionUnit[x, y] = null;
    }
    public bool addUnit(PlayerHandle playerHandle, string name, int x, int y, bool isEnemy)
    {
        if (playerHandle.CheckLimitUnit())
        {
            Unit newUnit = Create(name, x, y, isEnemy);
            if (newUnit.isGeneral)
            {
                player.hasGeneral = true;
            }
            playerHandle.AddUnit(newUnit);
            if (newUnit)
                SetPosition(newUnit);
            return true;
        }
        else
        {
            SkipTurn.Instance.Notification_Show("Number unit max limit");
            return false;
        }
    }

    /// <summary>
    /// BinhBH da co bao nhieu unit
    /// </summary>
    /// <returns></returns>
    public int getNumberUnit()
    {
        return player.NumberUnit;
    }

    public Unit GetPosition(int x, int y) => PositionUnit[x, y];

    void Update()
    {
        if (Status == eStatus.Turn_Bot && bot.rest && !block)
        {
            block = true;
            queue = new Queue<Unit>();
            if (!bot.arrListUnit.Exists(x => x.isMove == true))
            {
                StartTurn();
                return;
            }

            foreach (var item in bot.arrListUnit)
            {
                if (!item.isMove) continue;
                item.GetListMovePlates(out List<MovePlate> outlt);
                item.SetTagetBot();
                float pointMax = -99;
                float point;
                MovePlate movePlateBest = new MovePlate();
                foreach (var platemove in outlt)
                {
                    int d1 = 0, d2 = 0, d3 = 0;

                    if (item.UnitTarget != null)
                    {
                        d1 = Math.Abs(item.UnitTarget.x - platemove.x) + Math.Abs(item.UnitTarget.y - platemove.y) - item.Range;
                    }
                    if (item.HouseTarget != null)
                    {
                        d2 = item.Dijkstra(platemove, item.HouseTarget);
                    }
                    if (item.CastleTarget != null)
                    {
                        d3 = item.Dijkstra(platemove, item.HouseTarget);
                    }

                    point = d1 <= 0 ? (10 + d1) : (10 - d1) + d2 == 0 ? 20 : (20 - (int)Math.Ceiling((double)d2 / item.Move) * 5);
                    //+ d3 == 0 ? 30 : (15 - d3 / item.Move * 5);
                    if (point > pointMax)
                    {
                        pointMax = point;
                        movePlateBest = platemove;
                    }
                }

                bot.rest = false;
                item.isMove = false;
                movePlateBest.Click();
                block = false;
                return;
            }
        }
    }
    
    public void EndGame()
    {
        if (!player.hasGeneral && player.CountOccupiedCastle == 0)
        {
            UIManager.Instance.ShowEndGame(false);
            DisableAll();
        }
        else if (!bot.hasGeneral && bot.CountOccupiedCastle == 0)
        {
            UIManager.Instance.ShowEndGame(true);
            DisableAll();
        }
    }

    private void DisableAll()
    {
        foreach (var item in bot.arrListUnit) item.DisableUnit();
        foreach (var item in player.arrListUnit) item.DisableUnit();
        Unit.DisablePlate();
        SkipTurn.Instance.HideNotification();
    }
}

class SX
{
    public MovePlate movePlate { get; set; }
    public int cost { get; set; }
    public int MinDestinationFinish { get; set; }
}