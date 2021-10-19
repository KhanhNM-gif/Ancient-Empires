using Assets.Asset.Model;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum eStatus
    {
        Turn_Player,
        Turn_Bot
    }

    private ConcurrentDictionary<string, GameObject> UnitDictionary;

    private Unit[,] PositionUnit = new Unit[100, 100];
    //private List<Unit> arrListUnit = new List<Unit>();
    public Bot bot { get; set; }
    public Player player { get; set; }
    public static GameManager Instance;
    public Unit UnitSelected;
    public static Shop shop;
    public static eStatus Status = eStatus.Turn_Player;

    //x=4-tầm đánh y=4-tầm đánh x4+tầm đánh y=4+tầm đánh  |xi-x||yi-y+|<=tầm đánh

    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        UnitDictionary = new ConcurrentDictionary<string, GameObject>();

        GameObject[] arrUnit = Resources.LoadAll<GameObject>(@"ObjectGame/Unit");
        for (int i = 0; i < arrUnit.Length; i++)
            UnitDictionary[arrUnit[i].name] = arrUnit[i];

        List<Unit> list = new List<Unit>();
        list.Add(Create(Const.NameUnit.BLUE_SOLDIER, 4, 3,false));
        player = new Player(Const.ConstGame.GOLD_START_GAME, 200, list);
        foreach (var item in list) SetPosition(item);

        list = new List<Unit>();
        list.Add(Create(Const.NameUnit.RED_SOLDIER, 8, 8, true));
        bot = new Bot(Const.ConstGame.GOLD_START_GAME, 200, list);
        foreach (var item in list) SetPosition(item);
    }
    public Unit Create(string name, int x, int y, bool isEnemy)
    {
        if (UnitDictionary.TryGetValue(name, out GameObject outGameObject))
        {
            GameObject obj = Instantiate(outGameObject, new Vector3(0, 0, -1), Quaternion.identity);
            Unit un = obj.GetComponent<Unit>();
            un.name = name;
            un.x = x;
            un.y = y;
            un.isEnemy = isEnemy;
            un.Activate();

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
        StartCoroutine(SetWaitForSeconds(3));
    }
    IEnumerator SetWaitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        SetStatus(eStatus.Turn_Player);
        StartTurn();
        SkipTurn.Instance.Notification_Show("Your turn");
    }

    public void StartTurn()
    {
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
    public bool addUnit(PlayerHandle playerHandle, string name, int x, int y,bool isEnemy)
    {
        if (playerHandle.CheckLimitUnit())
        {
            Unit newUnit = Create(name, x, y, isEnemy);
            playerHandle.AddUnit(newUnit);
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

    void Update() { }
}
