using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G : MonoBehaviour
{
    public GameObject Catapult;

    private GameObject[,] position = new GameObject[100, 100];
    private GameObject[] player = new GameObject[8];

    // Start is called before the first frame update
    void Start()
    {
        player = new GameObject[]
        {
            CreateCatapult("player_Soldier",0,0)

        };
        for (int i = 0; i < player.Length; i++)
        {
            SetPosition(player[i]);
        }
    }
    public GameObject CreateCatapult(string name, int x, int y)
    {
        GameObject obj = Instantiate(Catapult, new Vector3(0, 0, -1), Quaternion.identity);
        Catapult un = obj.GetComponent<Catapult>();
        un.name = name;
        un.SetXMap(x);
        un.SetYMap(y);
        un.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {

        Catapult Catapult = obj.GetComponent<Catapult>();
        int x = Catapult.GetXMap();
        int y = Catapult.GetYMap();
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

