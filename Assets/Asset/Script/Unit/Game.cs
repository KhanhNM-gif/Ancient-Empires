using Assets.Asset.Model;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    private ConcurrentDictionary<string, GameObject> UnitDictionary;

    private GameObject[,] PositionUnit = new GameObject[100, 100];
    private List<GameObject> arrListUnit = new List<GameObject>();
    public static Game Instance;
    public static Shop shop;
    public Unit UnitSelected;



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

        arrListUnit.Add(Create(Const.NameUnit.BLUE_ARCHER, 5, 4));
        arrListUnit.Add(Create(Const.NameUnit.BLUE_ARCHER, 6, 4));
        for (int i = 0; i < arrListUnit.Count; i++)
        {
            SetPosition(arrListUnit.ElementAt(i));
        }
    }
    public GameObject Create(string name, int x, int y)
    {
        if (UnitDictionary.TryGetValue(name, out GameObject outGameObject))
        {
            GameObject obj = Instantiate(outGameObject, new Vector3(0, 0, -1), Quaternion.identity);
            Unit un = obj.GetComponent<Unit>();
            un.name = name;
            un.x = x;
            un.y = y;
            un.Activate();

            return obj;
        }
        return null;
    }

    public void SetPosition(GameObject obj)
    {
        Unit unit = obj.GetComponent<Unit>();
        int x = unit.x;
        int y = unit.y;
        if (x < 0) x = 0;
        if (y < 0) y = 0;

        PositionUnit[x, y] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        if (x < 0 || y < 0) return;
        PositionUnit[x, y] = null;
    }

    public GameObject GetPosition(int x, int y) => PositionUnit[x, y];

    void Update(){ }

    public void addUnit(string name, int x, int y)
    {
        if (arrListUnit.Count == Const.ConstGame.MAX_UNIT) return;
        arrListUnit.Add(Create(name, x, y));
        SetPosition(arrListUnit.ElementAt(arrListUnit.Count-1));
    }

    /// <summary>
    /// BinhBH da co bao nhieu unit
    /// </summary>
    /// <returns></returns>
    public int getNumberUnit()
    {
        return arrListUnit.Count;
    }
}
