using Assets.Asset.Model;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour, MatrixCoordi
{
    //public GameObject controller;
    //private string player;
    //public Sprite unit_sheet_1_0;

    public GameObject movePlates;
    public GameObject attackPlate;
    public int move = 3;
    public int attack = 1;
    public int moveSpeed = 2;

    private Queue<MatrixCoordi> queueMove;
    private Vector3 vectorTo;
    private bool IsMoving;
    public int x { get; set; }
    public int y { get; set; }

    public void Start() { }
    public void Activate()
    {
        //controller = GameObject.FindGameObjectWithTag("GameController");
        SetWordPositon();

        /*switch (this.name)
        {
            case "player_Soldier":
                this.GetComponent<SpriteRenderer>().sprite = unit_sheet_1_0;
                break;
        }*/
    }

    public void SetWordPositon() => transform.position = MapTile.GridWordPosition(x, y, -1);
    public void SetStackMove(Queue<MatrixCoordi> queue) => queueMove = queue;

    public void OnMouseDown()
    {
        DestroyMovePlate();
        InitiateMovePlates();
    }

    public void DestroyMovePlate()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        for (int i = 0; i < movePlates.Length; i++) Destroy(movePlates[i]);
    }
    public void DestroyAttackPlate()
    {
        GameObject[] attackPlates = GameObject.FindGameObjectsWithTag("AttackPlate");

        for (int i = 0; i < attackPlates.Length; i++) Destroy(attackPlates[i]);
    }

    public void InitiateMovePlates()
    {
        MatrixCoordi matrixCoordi;
        int deep;
        bool[,] visit = new bool[MapManager.map.Width, MapManager.map.Height];
        Queue<Tuple<MatrixCoordi, int>> queue = new Queue<Tuple<MatrixCoordi, int>>();
        ConcurrentDictionary<MatrixCoordi, MatrixCoordi> wayDictionary = new ConcurrentDictionary<MatrixCoordi, MatrixCoordi>();

        visit[x, y] = true;
        wayDictionary[MapManager.map.arrTile[x, y]] = null;
        queue.Enqueue(new Tuple<MatrixCoordi, int>(MapManager.map.arrTile[x, y], 0));

        while (queue.Count > 0)
        {
            Tuple<MatrixCoordi, int> s = queue.Dequeue();
            matrixCoordi = s.Item1; deep = s.Item2;

            if (deep > move) return;

            if (deep > 0)
            {
                GetQueueWay(wayDictionary, matrixCoordi, MapManager.map.arrTile[x, y], out Queue<MatrixCoordi> way);
                MovePlateSpawn(matrixCoordi.x, matrixCoordi.y, way);
            }

            foreach (var item in Const.Unit.STEP_MOVE)
            {
                int x = matrixCoordi.x + item.Item1;
                int y = matrixCoordi.y + item.Item2;

                if (x < MapManager.map.Width && x >= 0 && y < MapManager.map.Height && y >= 0 && MapManager.map.arrTile[x, y].MoveAble && !visit[x, y])
                {
                    queue.Enqueue(new Tuple<MatrixCoordi, int>(MapManager.map.arrTile[x, y], deep + 1));
                    wayDictionary[MapManager.map.arrTile[x, y]] = matrixCoordi;
                    visit[x, y] = true;
                }
            }
        }
    }

    public void GetQueueWay(ConcurrentDictionary<MatrixCoordi, MatrixCoordi> dictionnary, MatrixCoordi p, MatrixCoordi e, out Queue<MatrixCoordi> way)
    {
        if (dictionnary[p] != null)
        {
            GetQueueWay(dictionnary, dictionnary[p], e, out way);
            way.Enqueue(p);
        }
        else way = new Queue<MatrixCoordi>();
    }

    public void MovePlateSpawn(int matrixX, int matrixY, Queue<MatrixCoordi> queue)
    {
        GameObject mp = Instantiate(movePlates, MapTile.GridWordPosition(matrixX, matrixY, -1), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(this);
        mpScript.SetCoords(matrixX, matrixY);
        mpScript.SetStackWay(queue);
        mpScript.name = $"MovePlate({matrixX},{matrixY})";
    }

    void Update()
    {
        if (queueMove != null && queueMove.Count > 0)
        {
            if (!IsMoving)
            {
                MatrixCoordi matrixCoordi = queueMove.Peek();
                vectorTo = MapTile.GridWordPosition(matrixCoordi.x, matrixCoordi.y, -1);

                IsMoving = true;
            }
            else Move();
        }

    }
    private void Move()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, vectorTo, step);

        if (Vector3.SqrMagnitude(transform.position - vectorTo) == 0)
        {
            IsMoving = false;
            queueMove.Dequeue();
        }
    }
}
