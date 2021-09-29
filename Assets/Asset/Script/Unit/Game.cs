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
    private GameObject[] Unit = new GameObject[8];

    // Start is called before the first frame update
    void Start()
    {
        UnitDictionary = new ConcurrentDictionary<string, GameObject>();

        GameObject[] arrUnit = Resources.LoadAll<GameObject>(@"ObjectGame/Unit");
        for (int i = 0; i < arrUnit.Length; i++)
            UnitDictionary[arrUnit[i].name] = arrUnit[i];

        Unit = new GameObject[]
        {
            Create(Const.NameUnit.BLUE_ARCHER,4,4)
        };
        for (int i = 0; i < Unit.Length; i++)
        {
            SetPosition(Unit[i]);
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
}
