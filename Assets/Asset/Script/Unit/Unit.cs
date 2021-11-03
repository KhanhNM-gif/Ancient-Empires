using Assets.Asset.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IMatrixCoordi
{

    public enum LabelUnit
    {
        Archer,
        General,
        Soldier,
        Catapult
    }

    //public GameObject controller;
    //private string player;
    //public Sprite unit_sheet_1_0;

    public int x { get; set; }
    public int y { get; set; }

    public LabelUnit labelUnit;
    public float CurrentHP;
    public float HP;
    public float Attack;
    public float Armor;
    public float BaseArmor;
    public int Range;
    public int BaseRange;
    public int Move;
    public int MoveSpeed;
    public int Lv;
    private float exp;
    private float expRequired;
    //float time = 0;
    public bool isEnemy;

    private bool isAttack;
    private bool isMove;
    //private bool isDisable;
    public bool canOccupiedCastle;
    public bool canOccupiedHouse;
    public bool isGeneral;

    private Queue<IMatrixCoordi> queueMove;
    private Vector3 vectorTo;
    private bool IsMoving;
    public Transform firePoint;
    public Transform DustPoint;

    public ActUnit Act;
    public bool isAct;

    public virtual void Start()
    {
        BaseArmor = Armor;
        BaseRange = Range;
        MapManager.map.arrTile[x, y].MoveAble = false;
        MapManager.map.arrTile[x, y].AttackAble = false;

        isAttack = true;
        isMove = true;

        //Delay time spawn smoke
        InvokeRepeating("MoveEffect", 0.2f, 0.2f);
        //ExpRequired to Lv2
        expRequired = 100;

    }
    public void Activate()
    {
        SetWordPositon();
    }

    public void SetWordPositon() => transform.position = MapTile.GridWordPosition(x, y, -1);
    public Vector3 GetWordPositon() => MapTile.GridWordPosition(x, y, -1);
    public void SetStackMove(Queue<IMatrixCoordi> queue)
    {
        queueMove = queue;
    }

    private void OnMouseDown()
    {
        DisablePlate();
        GameManager.Instance.UnitSelected = this;
        if (!isEnemy && GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player && this.isMove)
        {
            InitiateMovePlatesDelegate dlg = delegate (int x, int y, Queue<IMatrixCoordi> way) { MovePlateSpawn(x, y, way); };
            InitiateMovePlates(dlg);
        }
        if (!isEnemy && GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player && this.isAttack)
        {
            InitiateAttackPlates();
        }
        if (!isEnemy && GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player && this.isAttack == false && this.isMove == false)
        {
            DisableUnit();
        }
        UIManager.Instance.UpdateStatus(this);
    }
    public virtual void AnimationAttack(Unit unit, float damage)
    {

    }

    public static void DestroyAttackPlate()
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
        if (isAttack && !(Math.Abs(x - this.x) + Math.Abs(y - this.y) <= Range)) isAttack = false;
    }

    public void AttackPlateSpawn(int matrixX, int matrixY, Unit unit)
    {
        GameObject mp = Instantiate(AssetManage.i.attackPlates, MapTile.GridWordPosition(matrixX, matrixY, -2), Quaternion.identity);

        AttackPlate apScript = mp.GetComponent<AttackPlate>();
        apScript.SetReference(this);
        apScript.SetCoords(matrixX, matrixY);
        apScript.SetTarget(unit);
        apScript.name = $"MovePlate({matrixX},{matrixY})";
    }

    public static void DestroyMovePlate()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        for (int i = 0; i < movePlates.Length; i++) Destroy(movePlates[i]);
    }


    public delegate void InitiateMovePlatesDelegate(int x, int y, Queue<IMatrixCoordi> way);
    /// <summary>
    /// Tìm đường và Spawn ô di chuyển
    /// </summary>
    public void InitiateMovePlates(InitiateMovePlatesDelegate impd)
    {
        IMatrixCoordi matrixCoordi;
        int cost;
        Queue<Tuple<IMatrixCoordi, int>> queue = new Queue<Tuple<IMatrixCoordi, int>>();
        ConcurrentDictionary<IMatrixCoordi, Tuple<IMatrixCoordi, int>> wayDictionary = new ConcurrentDictionary<IMatrixCoordi, Tuple<IMatrixCoordi, int>>();

        queue.Enqueue(new Tuple<IMatrixCoordi, int>(MapManager.map.arrTile[x, y], 0));

        while (queue.Count > 0)
        {
            Tuple<IMatrixCoordi, int> s = queue.Dequeue();
            matrixCoordi = s.Item1; cost = s.Item2;

            if (cost > 0)
            {
                GetQueueWay(wayDictionary, matrixCoordi, MapManager.map.arrTile[x, y], out Queue<IMatrixCoordi> way);
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
                    queue.Enqueue(new Tuple<IMatrixCoordi, int>(MapManager.map.arrTile[x, y], newCost));
                    wayDictionary[tile] = new Tuple<IMatrixCoordi, int>(MapManager.map.arrTile[matrixCoordi.x, matrixCoordi.y], newCost);
                }
            }
        }
    }
    // tấn công

    // chiếm thành
    public void GetQueueWay(ConcurrentDictionary<IMatrixCoordi, Tuple<IMatrixCoordi, int>> dictionnary, IMatrixCoordi p, IMatrixCoordi e, out Queue<IMatrixCoordi> way)
    {
        if (dictionnary.ContainsKey(p))
        {
            GetQueueWay(dictionnary, dictionnary[p].Item1, e, out way);
            way.Enqueue(p);
        }
        else way = new Queue<IMatrixCoordi>();
    }

    public void MovePlateSpawn(int matrixX, int matrixY, Queue<IMatrixCoordi> queue)
    {
        GameObject mp = Instantiate(AssetManage.i.movePlates, MapTile.GridWordPosition(matrixX, matrixY, -1), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(this);
        mpScript.SetCoords(matrixX, matrixY);
        mpScript.SetStackWay(queue);
        mpScript.name = $"MovePlate({matrixX},{matrixY})";
    }

    public virtual void Update()
    {
        UpdatePossion();
        if (isAct)
        {
            Act.queueActs.Dequeue().Handle();
        }
    }

    protected void UpdatePossion()
    {
        if (queueMove != null && queueMove.Count > 0)
        {
            if (!IsMoving)
            {
                IMatrixCoordi matrixCoordi = queueMove.Peek();
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
                Occupied();
                Armor = BaseArmor + MapManager.map.arrTile[x, y].Ammor;
                Range = BaseRange + MapManager.map.arrTile[x, y].ShootingRange;
                UIManager.Instance.UpdateStatus(this);
                if (CheckDisable()) DisableUnit();
            }
        }
    }

    /// <summary>
    /// BinhBH Chiem thanh, nha
    /// </summary>
    private void Occupied()
    {
        Unit u = this;
        if (u != null)
        {
            bool playerOccupied = GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player;
            if (MapManager.map.arrTile[u.x, u.y].IsCastle && u.x == this.x && u.y == this.y && u.canOccupiedCastle)
            {
                ((Castle)MapManager.map.arrTile[x, y]).changeOwner(playerOccupied ? 1 : 0);
                if (playerOccupied)
                {
                    GameManager.Instance.player.listOccupied.Add(MapManager.map.arrTile[x, y]);
                }
                else
                {
                    GameManager.Instance.bot.listOccupied.Add(MapManager.map.arrTile[x, y]);
                }

                SkipTurn.Instance.Notification_Show("Occupied Castle");
            }
            else if (MapManager.map.arrTile[u.x, u.y].IsHouse && u.x == this.x && u.y == this.y && u.canOccupiedHouse)
            {
                ((House)MapManager.map.arrTile[x, y]).changeOwner(playerOccupied ? 1 : 0);
                if (playerOccupied)
                {
                    GameManager.Instance.player.listOccupied.Add(MapManager.map.arrTile[x, y]);
                }
                else
                {
                    GameManager.Instance.bot.listOccupied.Add(MapManager.map.arrTile[x, y]);
                }
                SkipTurn.Instance.Notification_Show("Occupied House");
            }
        }
    }

    public void AttackToUnit(Unit unitTarget, out float damage)
    {
        damage = (float)Math.Round(Attack * (100f / (100 + unitTarget.Armor)));
        //unitTarget.TakeDame(damage);
        AddExp(damage);
        if (CheckDisable()) DisableUnit();
    }

    public void TakeDame(float damage)
    {
        CurrentHP -= damage;
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
            InvokeRepeating("Death", 0.5f, 0.2f);
        }
    }

    public void Death()
    {
        Destroy(gameObject);
        GameObject f = Instantiate(AssetManage.i.Explo, firePoint.position, Quaternion.identity);
        Destroy(f, f.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
    public void showPopupDameTake()
    {
        PopupDamage.CreatePopupDamage(100f, transform.position);
    }
    private bool CheckDisable() => !isAttack && !isMove;

    private void DisableUnit()
    {
        //isDisable = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3137255f, 0.3137255f, 0.2784314f, 1f);
        GetComponent<Animator>().enabled = false;
    }
    public void EnableUnit()
    {
        //isDisable = false;
        isAttack = isMove = true;
    }
    public void EnableColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(225, 225, 255);
        GetComponent<Animator>().enabled = true;
    }
    public void MoveEffect()
    {

        if (IsMoving)
        {
            GameObject d = Instantiate(AssetManage.i.dust, DustPoint.position, DustPoint.rotation);
            Destroy(d, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
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
        expRequired = 1.25f * expRequired;
        Attack += 5;
        Armor += 2;
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
    public static void DisablePlate()
    {
        DestroyAttackPlate();
        DestroyMovePlate();
    }

    public int Distance(IMatrixCoordi mc)
    {
        return Math.Abs(mc.x - x) + Math.Abs(mc.y - y);
    }
}
