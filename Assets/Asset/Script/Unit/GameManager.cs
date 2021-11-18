using Assets.Asset.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Unit;

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
    public Queue<IPlateAction> queue;
    QuestManage quest = new QuestManage();
    public string MapName;

    private void Awake()
    {
        Instance = this;
        block = true;
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
        player.arrListUnit.Add(Create(Const.NameUnit.BLUE_GENERAL, 5, 4, false));
        player.hasGeneral = true;
        foreach (var item in player.arrListUnit) SetPosition(item);

        bot = new Bot(Const.ConstGame.GOLD_START_GAME, 200);
        bot.arrListUnit.Add(Create(Const.NameUnit.RED_GENERAL, 10, 8, true));
        bot.hasGeneral = true;
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
            if (name == Const.NameUnit.BLUE_SOLDIER) un.labelUnit = LabelUnit.Soldier;
            if (name == Const.NameUnit.RED_SOLDIER) un.labelUnit = LabelUnit.Soldier;
            if (name == Const.NameUnit.RED_GENERAL) un.labelUnit = LabelUnit.General;
            if (name == Const.NameUnit.BLUE_GENERAL) un.labelUnit = LabelUnit.General;

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

    public async void EndTurn()
    {
        foreach (var item in player.arrListUnit)
        {
            item.EnableColor();
        }
        Unit.DisablePlate();
        quest.SetQuest();
        quest.BuyUnitAuto();
        await Task.Run(() => AI(quest));
    }
    public void AI(QuestManage quest)
    {
        quest.SetWeightListQuest();
        quest.Handle(out queue);
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
    public bool addUnit(PlayerHandle playerHandle, string name, int x, int y, bool isEnemy, out Unit outUnit)
    {
        outUnit = null;

        if (playerHandle.CheckLimitUnit())
        {
            outUnit = Create(name, x, y, isEnemy);
            if (outUnit.isGeneral)
            {
                player.hasGeneral = true;
            }
            playerHandle.AddUnit(outUnit);
            if (outUnit)
                SetPosition(outUnit);

            outUnit.DisableUnit2();
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
        if (!block)
        {
            block = true;
            if (queue.Count == 0)
            {
                quest.BuyUnitAuto();
                StartTurn();
            }
            else queue.Dequeue().Handle();
        }
    }

    public void EndGame()
    {
        if (player.CountOccupiedCastle == MapManager.map.castles.Count+ MapManager.map.houses.Count)
        {
            UIManager.Instance.ShowEndGame(true);
            DisableAll();
        }
        else if (bot.CountOccupiedCastle == MapManager.map.castles.Count + MapManager.map.houses.Count)
        {
            UIManager.Instance.ShowEndGame(false);
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
