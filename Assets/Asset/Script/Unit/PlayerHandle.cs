using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHandle : MonoBehaviour
{
    public int NumberUnitLimit { get; set; } = Const.PlayerHandle.NUMBER_UNIT_LIMIT;
    public int NumberUnit { get => arrListUnit.Count; }
    public int Gold { get; set; }
    public int GoldPerTurn { get; set; }
    public List<Unit> arrListUnit = new List<Unit>();
    public int CountOccupiedHouse = 0;
    public int CountOccupiedCastle = 0;
    public List<BaseTile> listOccupied = new List<BaseTile>();
    public bool hasGeneral;

    public PlayerHandle(int gold, int goldPerTurn)
    {
        Gold = gold;
        GoldPerTurn = goldPerTurn;

    }
    public bool CheckLimitUnit() => NumberUnit < NumberUnitLimit;
    public virtual void StartTurn()
    {
        //BinhBH tinh toan lai so tien duoc cong lai vao bat dau moi turn
        GoldPerTurn = Const.ConstGame.GOLD_PER_TURN_DEFAULT + listOccupied.Count * Const.ConstGame.GOLD_PER_CONTRUCTION;
        Gold += GoldPerTurn;
        UIManager.Instance.UpdateGold(Gold);
        foreach (var item in arrListUnit)
        {
            item.SetIsAttack(true);
            item.SetIsMove(true);
            item.EnableUnit();
            item.EnableColor();
            if (item.isGeneral)
            {
                hasGeneral = item.isGeneral;
            }

            //BinhBH Hoi mau cho tuong trong thanh hoac nha.
            if (MapManager.map.arrTile[item.x, item.y].IsCastle || MapManager.map.arrTile[item.x, item.y].IsHouse)
            {
                float hp = item.CurrentHP += 20;
                if (hp > item.HP) hp = item.HP;
                item.CurrentHP = hp;
                GameObject h = Instantiate(AssetManage.i.Heal, item.firePoint.position, Quaternion.identity);
                Destroy(h, h.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
                GameObject heal = Instantiate(AssetManage.i.HealUp, item.firePoint.position, Quaternion.identity);
                Destroy(heal, heal.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

            }

        }
    }
    public virtual void AddUnit(Unit unit)
    {
        arrListUnit.Add(unit);
    }
}
