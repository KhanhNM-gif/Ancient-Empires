using Assets.Asset.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject _unit;

    private GameObject[,] position = new GameObject[100, 100];
    private GameObject[] player = new GameObject[8];

    // Start is called before the first frame update
    void Start()
    {
        player = new GameObject[]
        {
            Create("player_Soldier",4,4)
        };
        for (int i = 0; i < player.Length; i++)
        {
            SetPosition(player[i]);
        }
    }
    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(_unit, new Vector3(0, 0, -1), Quaternion.identity);
        Unit un = obj.GetComponent<Unit>();
        un.name = name;
        un.SetXMap(x);
        un.SetYMap(y);
        un.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {

        Unit unit = obj.GetComponent<Unit>();
        int x = unit.GetXMap();
        int y = unit.GetYMap();
        if (x < 0)
        {
            x = 0;
        }
        if (y < 0)
        {
            y = 0;
        }
        position[x, y] = obj;

    }

    public void SetPositionEmpty(int x, int y)
    {
        if (x < 0)
        {
            x = 0;
        }
        if (y < 0)
        {
            y = 0;
        }
        position[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return position[x, y];
    }

    public bool PositionOnMap(int x, int y)
    {
        // if(x<0 || y<0 || x>= position.GetLength(0) || y >= position.GetLength(1)) return false;
        return true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
