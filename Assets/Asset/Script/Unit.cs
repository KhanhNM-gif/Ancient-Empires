using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlates;

    public int xMap = -1;
    public int yMap = -1;
    private int move = 4;
    public int moveSpeed = 2;

    public string player;

    public Sprite unit_sheet_1_0;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case "player_Soldier":
                this.GetComponent<SpriteRenderer>().sprite = unit_sheet_1_0; player = "player";
                break;
        }
    }

    public void SetCoords()
    {
        float x = xMap;
        float y = yMap;

        x += 0.5f;
        y += 0.5f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXMap()
    {
        return xMap;
    }
    public int GetYMap()
    {
        return yMap;
    }
    public void SetXMap(int x)
    {
        xMap = x;
    }
    public void SetYMap(int y)
    {
        yMap = y;
    }

    public void OnMouseDown()
    {
        DestroyMovePlate();
        InitiateMovePlates();
    }

    public void DestroyMovePlate()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates()
    {
        for (int i = xMap - move; i <= xMap + move; i++)
        {
            for (int j = yMap - move; j <= yMap + move; j++)
            {
                int xDistance = i - xMap;
                int yDistance = j - yMap;
                if (xDistance < 0)
                {
                    xDistance = -xDistance;
                }
                if (yDistance < 0)
                {
                    yDistance = -yDistance;
                }
                if (xDistance + yDistance <= move && !(i == xMap && j == yMap))
                {
                    MovePlateSpawn(i, j);
                }
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x += 0.5f;
        y += 0.5f;

        GameObject mp = Instantiate(movePlates, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
