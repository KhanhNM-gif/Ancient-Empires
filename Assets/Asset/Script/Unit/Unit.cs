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
    public float Lv;
    private float exp;
    private float expRequired;
    public bool isEnemy;
    public GameObject movePlates;
    public GameObject attackPlates;
    public GameObject dust;
    public GameObject Explo;
    private bool isAttack;
    private bool isMove;
    private bool isDisable = false;
    public bool canOccupiedCastle;
    public bool canOccupiedHouse;
    public bool isGeneral;

    private Queue<MatrixCoordi> queueMove;
    private Vector3 vectorTo;
    private bool IsMoving;
    public Transform firePoint;
    public Transform DustPoint;

    public virtual void Start()
    {
        MapManager.map.arrTile[x, y].MoveAble = false;
        MapManager.map.arrTile[x, y].AttackAble = false;
        isAttack = true;
        isMove = true;
        //Delay time spawn smoke
        InvokeRepeating("MoveEffect",0.2f,0.2f);
    }
    public void Activate()
    {
        SetWordPositon();
    }

    public void SetWordPositon() => transform.position = MapTile.GridWordPosition(x, y, -1);
    public void SetStackMove(Queue<MatrixCoordi> queue)
    {
        queueMove = queue;
    }

    private void OnMouseDown()
    {
        GameManager.Instance.UnitSelected = this;
        if (!isEnemy && GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player && this.isMove)
        {
            DestroyMovePlate();
            InitiateMovePlatesDelegate dlg = delegate (int x, int y, Queue<MatrixCoordi> way) { MovePlateSpawn(x, y, way); };
            InitiateMovePlates(dlg);
        }
        if (!isEnemy && GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player && this.isAttack)
        {
            DestroyAttackPlate();
            InitiateAttackPlates();
        }
        if(!isEnemy && GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player && this.isAttack == false && this.isMove == false)
        {
            DisableUnit();
        }
        UIManager.Instance.UpdateStatus(this);
    }
    public virtual void AnimationAttack()
    {

    }
    public void DestroyAttackPlate()
    {
        GameObject[] attackPlates = GameObject.FindGameObjectsWithTag("AttackPlate");

        for (int i = 0; i < attackPlates.Length; i++)
            Destroy(attackPlates[i]);
    }


    public void InitiateAttackPlates()
    {
        foreach (var item in GameManager.Instance.bot.arrListUnit)
        {
            if (Math.Abs(x - item.x) + Math.Abs(y - item.y) <= Range)
            {
                AttackPlateSpawn(item.x, item.y, item);
            }
        }
    }
    public void SetAttack()
    {
        foreach (var item in GameManager.Instance.bot.arrListUnit)
        {
            if (Math.Abs(x - item.x) + Math.Abs(y - item.y) <= Range)
            {
                isAttack = true;
            }
            else
            {
                isAttack = false;
            }          
        }
    }

    public void AttackPlateSpawn(int matrixX, int matrixY, Unit unit)
    {
        GameObject mp = Instantiate(attackPlates, MapTile.GridWordPosition(matrixX, matrixY, -2), Quaternion.identity);

        AttackPlate apScript = mp.GetComponent<AttackPlate>();
        apScript.SetReference(this);
        apScript.SetCoords(matrixX, matrixY);
        apScript.SetTarget(unit);
        apScript.name = $"MovePlate({matrixX},{matrixY})";
    }

    public void DestroyMovePlate()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        for (int i = 0; i < movePlates.Length; i++) Destroy(movePlates[i]);
    }


    public delegate void InitiateMovePlatesDelegate(int x, int y, Queue<MatrixCoordi> way);
    /// <summary>
    /// Tìm đường và Spawn ô di chuyển
    /// </summary>
    public void InitiateMovePlates(InitiateMovePlatesDelegate impd)
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
                impd(matrixCoordi.x, matrixCoordi.y, way);
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

    public void AttackToUnit(Unit unitTarget)
    {
        float damage = Attack * (100f / (100 + Armor));
        unitTarget.TakeDame(damage);
        AddExp(damage);
        if (CheckDisable()) DisableUnit();
    }

    public void TakeDame(float damage)
    {
        CurrentHP -= damage ;
        if (CurrentHP <= 0)
        {
            if (this.isEnemy)
            {
                if (isGeneral) GameManager.Instance.bot.hasGeneral = false;
                GameManager.Instance.bot.arrListUnit.Remove(this);
            }
            else
            {
                if (isGeneral) GameManager.Instance.player.hasGeneral = false;
                GameManager.Instance.player.arrListUnit.Remove(this);
            }
            MapManager.map.arrTile[this.x, this.y].MoveAble = true;
            Destroy(gameObject);
            GameObject f = Instantiate(Explo, firePoint.position, Quaternion.identity);
            Destroy(f, f.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }


    private bool CheckDisable() => !isAttack && !isMove;

    private void DisableUnit()
    {
        isDisable = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3137255f, 0.3137255f, 0.2784314f, 1f);
        GetComponent<Animator>().enabled = false;
    }
    public void EnableUnit()
    {
        isDisable = false;
        isAttack = isMove = true;  
    }
    public void EnableColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(225, 225, 255);
        GetComponent<Animator>().enabled = true;
    }
    public void MoveEffect()
    {     
        
        if(IsMoving )
        {           
            GameObject d = Instantiate(dust, DustPoint.position, DustPoint.rotation);
            Destroy (d, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public bool GetIsAttack() => isAttack;
    public void SetIsAttack(bool isAttack) => this.isAttack = isAttack;
    public bool GetIsMove() => isMove;
    public void SetIsMove(bool isMove) => this.isMove = isMove;

    void LvUp()
    {
        Lv += 1;
        exp = exp - expRequired;
        Attack += 5;
        Armor += 2;
        expRequired = 1.25f * expRequired;
    }

    void Exp()
    {
        if (exp >= expRequired)
            LvUp();
    }

    void AddExp(float damage)
    {
        exp += damage;
        Exp();
    }

}
