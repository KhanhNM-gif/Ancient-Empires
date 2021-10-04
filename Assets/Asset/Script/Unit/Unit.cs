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

    public int x { get; set; }
    public int y { get; set; }

    public float CurrentHP;
    public float HP;
    public float Attack;
    public float Armor;
    public int Range;
    public int Move;
    public int MoveSpeed;
    public bool isEnemy;
    public GameObject movePlates;
    private bool isAttack = false;
    private bool isMove = false;
    private bool isDisable = false;


    private Queue<MatrixCoordi> queueMove;
    private Vector3 vectorTo;
    private bool IsMoving;


    public void Start()
    {
        MapManager.map.arrTile[x, y].MoveAble = false;
    }
    public void Activate()
    {
        SetWordPositon();
        isAttack = false;
    }

    public void SetWordPositon() => transform.position = MapTile.GridWordPosition(x, y, -1);
    public void SetStackMove(Queue<MatrixCoordi> queue)
    {
        queueMove = queue;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player && this.isMove)
        {
            DestroyMovePlate();
            InitiateMovePlates();
            this.isMove = false;
        }
        if (GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player && this.isAttack)
        {
            //DestroyAttackPlate
            //InitiateMovePlates tao o tan cong // x = 4-tầm đánh y = 4-tầm đánh x4 + tầm đánh y = 4 + tầm đánh  |xi-x||yi-y+| <= tầm đánh
            this.isAttack = false;
        }

    }

    public void DestroyMovePlate()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        for (int i = 0; i < movePlates.Length; i++) Destroy(movePlates[i]);
    }

    /// <summary>
    /// Tìm đường và Spawn ô di chuyển
    /// </summary>
    public void InitiateMovePlates()
    {
        MatrixCoordi matrixCoordi;
        int cost;
        Queue<Tuple<MatrixCoordi, int>> queue = new Queue<Tuple<MatrixCoordi, int>>();
        ConcurrentDictionary<MatrixCoordi, Tuple<MatrixCoordi, int>> wayDictionary = new ConcurrentDictionary<MatrixCoordi, Tuple<MatrixCoordi, int>>();

        queue.Enqueue(new Tuple<MatrixCoordi, int>(MapManager.map.arrTile[x, y], 0));

        while (queue.Count > 0)
        {
            Tuple<MatrixCoordi, int> s = queue.Dequeue();
            matrixCoordi = s.Item1; cost = s.Item2;

            if (cost > 0)
            {
                GetQueueWay(wayDictionary, matrixCoordi, MapManager.map.arrTile[x, y], out Queue<MatrixCoordi> way);
                MovePlateSpawn(matrixCoordi.x, matrixCoordi.y, way);
            }

            foreach (var item in Const.Unit.STEP_MOVE)
            {
                int x = matrixCoordi.x + item.Item1;
                int y = matrixCoordi.y + item.Item2;

                if (x >= MapManager.map.Width || x < 0 || y >= MapManager.map.Height || y < 0) continue;

                int newCost = cost + MapManager.map.arrTile[x, y].MoveRange;
                BaseTile tile = MapManager.map.arrTile[x, y];

                if (newCost <= this.Move && !(x == this.x && y == this.y) && (!wayDictionary.ContainsKey(tile) || wayDictionary[tile].Item2 > newCost) && tile.MoveAble)//Check dk ra khoi map
                {
                    queue.Enqueue(new Tuple<MatrixCoordi, int>(MapManager.map.arrTile[x, y], newCost));
                    wayDictionary[tile] = new Tuple<MatrixCoordi, int>(MapManager.map.arrTile[matrixCoordi.x, matrixCoordi.y], newCost);
                }
            }
        }
    }
    // tấn công

    // chiếm thành
    public void GetQueueWay(ConcurrentDictionary<MatrixCoordi, Tuple<MatrixCoordi, int>> dictionnary, MatrixCoordi p, MatrixCoordi e, out Queue<MatrixCoordi> way)
    {
        if (dictionnary.ContainsKey(p))
        {
            GetQueueWay(dictionnary, dictionnary[p].Item1, e, out way);
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
    public virtual void Update()
    {
        UpdatePossion();
    }

    protected void UpdatePossion()
    {
        if (queueMove != null && queueMove.Count > 0)
        {
            if (!IsMoving)
            {
                MatrixCoordi matrixCoordi = queueMove.Peek();
                vectorTo = MapTile.GridWordPosition(matrixCoordi.x, matrixCoordi.y, -1);
                //BinhBH chiem Thanh
                if (MapManager.map.arrTile[matrixCoordi.x, matrixCoordi.y].IsCastle)
                {
                    ((Castle)MapManager.map.arrTile[matrixCoordi.x, matrixCoordi.y]).changeOwner(1);
                }
                if (MapManager.map.arrTile[matrixCoordi.x, matrixCoordi.y].IsHouse)
                {
                    ((House)MapManager.map.arrTile[matrixCoordi.x, matrixCoordi.y]).changeOwner(1);
                }
                IsMoving = true;
            }
            else MoveMap();
        }
    }
    protected void MoveMap()
    {
        float step = MoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, vectorTo, step);

        if (Vector3.SqrMagnitude(transform.position - vectorTo) == 0)
        {
            IsMoving = false;
            queueMove.Dequeue();
            if (queueMove.Count == 0)
            {
                if (CheckDisable()) DisableUnit();
            }
        }
    }

    private bool CheckDisable() => !isAttack && !isMove;

    private void DisableUnit()
    {
        isDisable = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(80, 80, 71);
    }
    private void EnableUnit()
    {
        isDisable = false;
        isAttack = isMove = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(225, 225, 255);
    }

}
