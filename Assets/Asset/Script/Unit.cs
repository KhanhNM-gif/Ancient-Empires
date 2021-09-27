using Assets.Asset.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlates;

    public int xMap = -1;
    public int yMap = -1;
    private int move = 4;
    public int moveSpeed = 2;
    private Queue<Cell> QueueMove;
    private Vector3 vectorFrom;
    private Vector3 vectorTo;
    private Vector3 change;
    private bool IsMoving;
    Rigidbody2D myRigidbody;

    public string player;

    public Sprite unit_sheet_1_0;

    public void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }
    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GetPositon();

        switch (this.name)
        {
            case "player_Soldier":
                this.GetComponent<SpriteRenderer>().sprite = unit_sheet_1_0; player = "player";
                break;
        }
    }

    public void GetPositon() => transform.position = Map.GridWordPosition(xMap, yMap);

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
        BFS();
    }
    public void BFS()
    {
        Tuple<Cell, int> c;
        bool[,] visit = new bool[GridManger.map.Size, GridManger.map.Size];
        visit[xMap, yMap] = true;

        Queue<Tuple<Tuple<Cell, int>, Tuple<Cell, int>>> queue = new Queue<Tuple<Tuple<Cell, int>, Tuple<Cell, int>>>();
        List<Tuple<Cell, Cell>> t = new List<Tuple<Cell, Cell>>();

        queue.Enqueue(new Tuple<Tuple<Cell, int>, Tuple<Cell, int>>(new Tuple<Cell, int>(GridManger.map.arrCell[xMap, yMap], 0), new Tuple<Cell, int>(GridManger.map.arrCell[xMap, yMap], 0)));

        while (queue.Count > 0)
        {
            Tuple<Tuple<Cell, int>, Tuple<Cell, int>> tuple = queue.Dequeue();
            t.Add(new Tuple<Cell, Cell>(tuple.Item1.Item1, tuple.Item2.Item1));

            c = tuple.Item2;


            if (c.Item2 > move)
            {
                string str = string.Join(", ", t.Select(x => $"({x.Item2.x};{x.Item2.y})").ToArray());
                return;
            }
            if (c.Item2 > 0)
                MovePlateSpawn(c.Item1.x, c.Item1.y);

            if (c.Item1.x + 1 < GridManger.map.Size && GridManger.map.arrCell[c.Item1.x + 1, c.Item1.y].isCome && !visit[c.Item1.x + 1, c.Item1.y])
            {
                queue.Enqueue(new Tuple<Tuple<Cell, int>, Tuple<Cell, int>>(c, new Tuple<Cell, int>(GridManger.map.arrCell[c.Item1.x + 1, c.Item1.y], c.Item2 + 1)));
                visit[c.Item1.x + 1, c.Item1.y] = true;
            }
            if (c.Item1.x - 1 >= 0 && GridManger.map.arrCell[c.Item1.x - 1, c.Item1.y].isCome && !visit[c.Item1.x - 1, c.Item1.y])
            {
                queue.Enqueue(new Tuple<Tuple<Cell, int>, Tuple<Cell, int>>(c, new Tuple<Cell, int>(GridManger.map.arrCell[c.Item1.x - 1, c.Item1.y], c.Item2 + 1)));
                visit[c.Item1.x - 1, c.Item1.y] = true;
            }
            if (c.Item1.y + 1 < GridManger.map.Size && GridManger.map.arrCell[c.Item1.x, c.Item1.y + 1].isCome && !visit[c.Item1.x, c.Item1.y + 1])
            {
                queue.Enqueue(new Tuple<Tuple<Cell, int>, Tuple<Cell, int>>(c, new Tuple<Cell, int>(GridManger.map.arrCell[c.Item1.x, c.Item1.y + 1], c.Item2 + 1)));
                visit[c.Item1.x, c.Item1.y + 1] = true;

            }
            if (c.Item1.y - 1 >= 0 && GridManger.map.arrCell[c.Item1.x, c.Item1.y - 1].isCome && !visit[c.Item1.x, c.Item1.y - 1])
            {
                queue.Enqueue(new Tuple<Tuple<Cell, int>, Tuple<Cell, int>>(c, new Tuple<Cell, int>(GridManger.map.arrCell[c.Item1.x, c.Item1.y - 1], c.Item2 + 1)));
                visit[c.Item1.x, c.Item1.y - 1] = true;
            }


        }
    }

    public string PrintWayToFile(List<Tuple<Cell, Cell>> l, Tuple<Cell, Cell> t, Cell s, Cell k)
    {
        if (s.x != t.Item1.x || s.y != t.Item1.y) return PrintWayToFile(l, l.Where(x => x.Item2 == t.Item1).FirstOrDefault(), s, k) + $" -> ({t.Item2.x }-{t.Item2.y})";
        else return $" ({t.Item1.x }-{t.Item1.y}) -> ({t.Item2.x }-{t.Item2.y})";
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        GameObject mp = Instantiate(movePlates, Map.GridWordPosition(matrixX, matrixY, -1), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
    void Update()
    {
        if (QueueMove != null && QueueMove.Count > 0 && !IsMoving)
        {
            if (!IsMoving)
            {
                Cell cell = QueueMove.Dequeue();
                vectorFrom = transform.position;
                vectorTo = Map.GridWordPosition(cell.x, cell.y);

                float x = vectorTo.x - vectorFrom.x;
                float y = vectorTo.y - vectorFrom.y;

                float distanceMax = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                change.x = x / distanceMax;
                change.y = y / distanceMax;

                IsMoving = true;
            }
            else Move();
        }

    }
    public void Move()
    {
        Vector3 vector3 = transform.position + change * moveSpeed * Time.deltaTime;
        myRigidbody.MovePosition(vector3);
        if (vector3 == vectorTo) IsMoving = false;
    }
}
