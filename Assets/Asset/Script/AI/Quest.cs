using Assets.Asset.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TypeQuest;
using static Unit;

public class TypeQuest
{
    public enum eTypeQuest
    {
        OccupyCastle,
        GuardCastle,
        OccupyEmptyHouse,
        OccupyEnemyHouse,
        GuardHouse,
        AttackEnemy,
    }
    public static bool isOccupyQuest(eTypeQuest eTypeQuest)
    {
        return eTypeQuest == eTypeQuest.OccupyCastle || eTypeQuest == eTypeQuest.OccupyEmptyHouse || eTypeQuest == eTypeQuest.OccupyEnemyHouse;
    }
    public static bool isOccupyHouseQuest(eTypeQuest eTypeQuest)
    {
        return eTypeQuest == eTypeQuest.OccupyEmptyHouse || eTypeQuest == eTypeQuest.OccupyEnemyHouse;
    }
    public static bool isOccupyCastleQuest(eTypeQuest eTypeQuest)
    {
        return eTypeQuest == eTypeQuest.OccupyCastle;
    }
}
public class UnitDistanst
{
    public Unit unit;
    public int distance;
    public float point;
    public float factorAttack;
    public float factorOccupy;
    public float factorSafe;

    public UnitDistanst(Unit unit, int distance, float point)
    {
        this.unit = unit;
        this.distance = distance;
        this.point = point;
    }
}
public class TileDistance
{
    public BaseTile tile;
    public int distance;

    public TileDistance(BaseTile tile, int distance)
    {
        this.tile = tile;
        this.distance = distance;
    }
}
public class QuestBot
{
    //public List<Quest> ltChildQuest;
    public eTypeQuest TypeQuest;    //loại quest
    public List<TileDistance> ltConstructionBot;    // danh sách thành địch tham gia nv
    public List<TileDistance> ltConstructionPlayer; // danh sách thành ta tham gia nv
    public BaseTile ConstructionTarget;             // Đối tượng nhiệm vụ
    public List<UnitDistanst> ltUnitPlayer;         // List quân tham gia cuộc chiến
    public List<UnitDistanst> ltUnitBot;            // List quân địch tham gia cuộc chiến
    public float feasibility;                       // Độ khả thi thành công 
    public bool isEligible;
    public int NumberUnitOrCanOccupied;

    public QuestBot()
    {
        ltConstructionBot = new List<TileDistance>();
        ltConstructionPlayer = new List<TileDistance>();
        ltUnitPlayer = new List<UnitDistanst>();
        ltUnitBot = new List<UnitDistanst>();
    }
}

public class QuestManage
{
    LinkedList<QuestBot> ltQuest;

    public void Handle(Queue<Unit> outqueue)
    {
        outqueue = new Queue<Unit>();
        foreach (var quest in ltQuest)
        {
            foreach (var unitDistanst in quest.ltUnitBot)
            {
                unitDistanst.unit.GetListMovePlates(out List<MovePlate> outlt);
                float pointMax = -99;

                float point;

                MovePlate movePlateBest = null;
                AttackPlate attackPlateBest = null;

                AttackPlate attackPlateBestMove = null;
                AttackPlate attackPlateBestStand = null;
                float pointMaxAttackMaxMove = 0;
                float pointMaxAttackMaxStand = 0;

                float pointPlateMove, d = 0;

                unitDistanst.unit.GetAttackPlates(unitDistanst.unit, out List<AttackPlate> outAttackPlate);
                foreach (var attackPlate in outAttackPlate)
                {
                    var unitplayer = quest.ltUnitPlayer.Where(x => attackPlate.reference == x.unit).FirstOrDefault();
                    d = unitplayer == null ? 4 : unitplayer.point;

                    if (d > pointMaxAttackMaxStand)
                    {
                        pointMaxAttackMaxStand = d;
                        attackPlateBestStand = attackPlate;
                    }
                }

                foreach (var platemove in outlt)
                {
                    pointPlateMove = 0; d = 0; pointMaxAttackMaxMove = 0;

                    unitDistanst.unit.GetAttackPlates(platemove, out outAttackPlate);
                    foreach (var attackPlate in outAttackPlate)
                    {
                        var unitplayer = quest.ltUnitPlayer.Where(x => attackPlate.reference == x.unit).FirstOrDefault();
                        d = unitplayer == null ? 4 : unitplayer.point;

                        if (d > pointMaxAttackMaxMove)
                        {
                            pointMaxAttackMaxMove = d;
                            attackPlateBestMove = attackPlate;
                        }
                    }

                    pointPlateMove = unitDistanst.unit.Dijkstra(platemove, quest.ConstructionTarget);

                    point = Mathf.Max(pointMaxAttackMaxMove, pointMaxAttackMaxStand) * unitDistanst.factorAttack + pointPlateMove * unitDistanst.factorOccupy;
                    if (point > pointMax)
                    {
                        pointMax = point;
                        movePlateBest = platemove;
                        attackPlateBest = pointMaxAttackMaxMove > pointMaxAttackMaxStand ? attackPlateBestMove : attackPlateBestStand;
                    }
                }

                Queue<IPlateAction> plateActions = new Queue<IPlateAction>();
                if (attackPlateBest == attackPlateBestMove)
                {
                    plateActions.Enqueue(attackPlateBest);
                    plateActions.Enqueue(movePlateBest);
                }
                else
                {
                    plateActions.Enqueue(movePlateBest);
                    plateActions.Enqueue(attackPlateBest);
                }
                unitDistanst.unit.actions = plateActions;

                outqueue.Enqueue(unitDistanst.unit);

                return;
            }
        }
    }
    public void BuyUnit()
    {
        BaseTile bt = GameManager.Instance.bot.listOccupied.FirstOrDefault();
        GameManager.shop.x = bt.x;
        GameManager.shop.y = bt.y;

        while (true)
        {
            if (!GameManager.Instance.bot.CheckLimitUnit()) return;
            if (GameManager.Instance.bot.arrListUnit.Count(x => x.labelUnit == LabelUnit.Soldier) <= 3)
            {
                if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_SOLIDER) return;
                GameManager.shop.Buy(Const.NameUnit.RED_SOLDIER);
            }
            else
            {
                if (GameManager.Instance.bot.arrListUnit.Count(x => x.labelUnit == LabelUnit.Archer) <= 3)
                {
                    if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_ARCHER) return;
                    GameManager.shop.Buy(Const.NameUnit.RED_ARCHER);
                }
                else
                {
                    if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_CAPUTAL) return;
                    GameManager.shop.Buy(Const.NameUnit.RED_CATAPULT);
                }
            }
        }
    }

    public void SetQuest()
    {
        List<BaseTile> listTileBot = GameManager.Instance.bot.listOccupied;
        List<BaseTile> listTilePlayer = GameManager.Instance.player.listOccupied;
        List<Unit> listUnitPlayer = GameManager.Instance.player.arrListUnit;
        List<House> listHouse = MapManager.map.GetListHouse();
        List<Castle> listCastle = MapManager.map.GetListCastle();
        List<Unit> listUnitBot = new List<Unit>(GameManager.Instance.bot.arrListUnit);


        //set ConstructionTarget House
        ltQuest = new LinkedList<QuestBot>();
        foreach (var item in listHouse)
        {
            QuestBot quest = new QuestBot() { ConstructionTarget = item };
            if (item.isOwnerBy == 0) quest.TypeQuest = eTypeQuest.OccupyEmptyHouse;
            if (item.isOwnerBy == 1) quest.TypeQuest = eTypeQuest.OccupyEnemyHouse;
            if (item.isOwnerBy == 2) quest.TypeQuest = eTypeQuest.GuardHouse;

            ltQuest.AddFirst(quest);
        }
        foreach (var item in listCastle)
        {
            QuestBot quest = new QuestBot() { ConstructionTarget = item };
            if (item.isOwnerBy == 1) quest.TypeQuest = eTypeQuest.OccupyCastle;
            if (item.isOwnerBy == 2) quest.TypeQuest = eTypeQuest.GuardCastle;

            ltQuest.AddFirst(quest);
        }

        //set listUnitPlayer
        foreach (var item in listUnitPlayer)
        {
            foreach (var quest in ltQuest)
            {
                float d = (float)Math.Ceiling((float)item.Dijkstra(null, quest.ConstructionTarget) / item.Move);
                if (d <= 2)
                {
                    switch (item.labelUnit)
                    {
                        case LabelUnit.Soldier: quest.ltUnitPlayer.Add(new UnitDistanst(item, (int)d, 8 * (3 - d))); ; quest.feasibility -= 8 * (3 - d); break;
                        case LabelUnit.Archer: quest.ltUnitPlayer.Add(new UnitDistanst(item, (int)d, 10 * (3 - d))); quest.feasibility -= 10 * (3 - d); break;
                        case LabelUnit.Catapult: quest.ltUnitPlayer.Add(new UnitDistanst(item, (int)d, 14 * (3 - d))); quest.feasibility -= 14 * (3 - d); break;
                        case LabelUnit.General: quest.ltUnitPlayer.Add(new UnitDistanst(item, (int)d, 20 * (3 - d))); quest.feasibility -= 20 * (3 - d); break;
                    }
                }
            }
        }

        List<QuestBot> questBotRemove = new List<QuestBot>();
        foreach (var quest in ltQuest)
            if ((quest.TypeQuest == eTypeQuest.GuardCastle || quest.TypeQuest == eTypeQuest.GuardHouse) && quest.feasibility == 0)
                questBotRemove.Add(quest);
        foreach (var item in questBotRemove)
            ltQuest.Remove(item);

        ltQuest = new LinkedList<QuestBot>(ltQuest.OrderByDescending(x => x.feasibility).ToList());
        while (ltQuest.Count > 1)
        {
            foreach (var item in listUnitBot)
            {
                int minD = 99;

                QuestBot questSelect = null;
                foreach (var quest in ltQuest.Where(x => !x.isEligible))
                {
                    int d = (int)Math.Ceiling((float)item.Dijkstra(null, quest.ConstructionTarget) / item.Move);
                    if (minD > d)
                    {
                        minD = d;
                        questSelect = quest;
                    }
                }

                if (questSelect != null)
                {
                    if (((questSelect.TypeQuest == eTypeQuest.OccupyEmptyHouse || questSelect.TypeQuest == eTypeQuest.OccupyEnemyHouse) && item.canOccupiedHouse) || (questSelect.TypeQuest == eTypeQuest.OccupyCastle && item.canOccupiedCastle))
                        questSelect.NumberUnitOrCanOccupied++;

                    switch (item.labelUnit)
                    {
                        case LabelUnit.Soldier: questSelect.ltUnitBot.Add(new UnitDistanst(item, minD, 8 * (3.5f - minD))); ; questSelect.feasibility += 8 * (3.5f - minD); break;
                        case LabelUnit.Archer: questSelect.ltUnitBot.Add(new UnitDistanst(item, minD, 10 * (3.5f - minD))); questSelect.feasibility += 10 * (3.5f - minD); break;
                        case LabelUnit.Catapult: questSelect.ltUnitBot.Add(new UnitDistanst(item, minD, 14 * (3.5f - minD))); questSelect.feasibility += 14 * (3.5f - minD); break;
                        case LabelUnit.General: questSelect.ltUnitBot.Add(new UnitDistanst(item, minD, 20 * (3.5f - minD))); questSelect.feasibility += 20 * (3.5f - minD); break;
                    }
                }
            }

            if (ltQuest.Count(x => x.feasibility < 0) == 0) return;

            ltQuest = new LinkedList<QuestBot>(ltQuest.OrderByDescending(x => x.feasibility).ToList());
            listUnitBot = new List<Unit>(ltQuest.Last.Value.ltUnitBot.Select(x => x.unit));
            ltQuest.RemoveLast();

            UnitDistanst unitDistanstRemove;
            foreach (var quest in ltQuest)
            {
                if (quest.feasibility > 0)
                {
                    if (isOccupyQuest(quest.TypeQuest) && quest.NumberUnitOrCanOccupied <= 1)
                    {
                        if (isOccupyCastleQuest(quest.TypeQuest))
                        {
                            unitDistanstRemove = quest.ltUnitBot.Where(x => !x.unit.canOccupiedCastle && x.point < quest.feasibility).OrderByDescending(x => x.distance).FirstOrDefault();
                            if (unitDistanstRemove != null && unitDistanstRemove.unit.canOccupiedCastle) quest.NumberUnitOrCanOccupied--;
                        }
                        else
                        {
                            unitDistanstRemove = quest.ltUnitBot.Where(x => !x.unit.canOccupiedHouse && x.point < quest.feasibility).OrderByDescending(x => x.distance).FirstOrDefault();
                            if (unitDistanstRemove != null && unitDistanstRemove.unit.canOccupiedHouse) quest.NumberUnitOrCanOccupied--;
                        }
                    }
                    else unitDistanstRemove = quest.ltUnitBot.Where(x => x.point < quest.feasibility).OrderByDescending(x => x.distance).FirstOrDefault();

                    if (unitDistanstRemove != null)
                    {
                        quest.feasibility -= unitDistanstRemove.point;
                        quest.ltUnitBot.Remove(unitDistanstRemove);
                    }
                    quest.isEligible = true;
                }
            }
        }
    }

    public class ActUnit
    {
        public Unit unit;
        public Queue<Act> queueActs;

        public enum eAct
        {
            Move,
            Attack
        }
    }
    public class Act
    {
        public void Handle()
        {

        }
    }
    public class ActMove : Act
    {

    }
    public class ActAttack : Act
    {

    }
}