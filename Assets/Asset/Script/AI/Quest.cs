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
    public bool isNew;

    public UnitDistanst(Unit unit, int distance, float point, bool isNew = false)
    {
        this.unit = unit;
        this.distance = distance;
        this.point = point;
        this.isNew = isNew;
    }

    public void SetPoitUnit(float factorAttack, float factorOccupy, float factorSafe)
    {
        this.factorAttack = factorAttack;
        this.factorOccupy = factorOccupy;
        this.factorSafe = factorSafe;
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
    public UnitDistanst Captain;
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

    public void SetWeightListQuest()
    {
        foreach (var item in ltQuest)
        {
            if (isOccupyQuest(item.TypeQuest))
            {
                if (isOccupyHouseQuest(item.TypeQuest))
                {
                    item.Captain = item.ltUnitBot.Where(x => x.unit.canOccupiedHouse).OrderBy(x => x.distance).ThenByDescending(x => x.unit.CurrentHP).FirstOrDefault();
                    item.Captain.SetPoitUnit(0.5f, 2, 1.25f);
                }
                else
                {
                    item.Captain = item.ltUnitBot.Where(x => x.unit.canOccupiedCastle).First();
                    item.Captain.SetPoitUnit(0.5f, 2, 1.25f);
                }

                bool first = item.Captain.distance > item.ltUnitPlayer.OrderBy(x => x.distance).First().distance;
                foreach (var unit in item.ltUnitBot.Where(x => x != item.Captain))
                {
                    if (first) unit.SetPoitUnit(1.25f, 0.5f, 1f);
                    else unit.SetPoitUnit(2f, 0.5f, 05f);
                }

            }
            else
            {
                foreach (var unit in item.ltUnitBot.Where(x => x != item.Captain))
                    unit.SetPoitUnit(1.25f, 2f, 0.75f);
            }

            /*OccupyCastle,
            GuardCastle,
            OccupyEmptyHouse,
            OccupyEnemyHouse,
            GuardHouse,*/
        }
    }


    public void Handle(out Queue<Unit> outqueue)
    {
        outqueue = new Queue<Unit>();
        foreach (var quest in ltQuest)
        {
            List<List<IMatrixCoordi>> outltImportantTile = null;
            if (quest.Captain != null)
                quest.Captain.unit.Dijkstra(quest.ConstructionTarget, quest.ConstructionTarget, out outltImportantTile, false);

            foreach (var unitDistanst in quest.ltUnitBot)
            {

                float pointMax = -99, point;

                MovePlate movePlateBest = null;
                List<List<IMatrixCoordi>> ltImportantTile;

                AttackPlate attackPlateBest = null, attackPlateBestMove = null, attackPlateBestStand = null;
                float pointMaxAttackMaxMove = 0, pointMaxAttackMaxStand = 0;

                float dPlateMove, dPlateMoveIndie, d = 0;


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

                dPlateMoveIndie = (float)Math.Ceiling((float)unitDistanst.unit.Dijkstra(unitDistanst.unit, quest.ConstructionTarget, out _) / unitDistanst.unit.Move);
                if (outltImportantTile != null && unitDistanst != quest.Captain)
                {
                    ltImportantTile = outltImportantTile.Where(x => x.Exists(y => y.x == unitDistanst.unit.x && y.y == unitDistanst.unit.y)).ToList();
                    if (ltImportantTile.Count > outltImportantTile.Count)
                        dPlateMoveIndie += 1.5f;
                }

                point = Mathf.Max(pointMaxAttackMaxMove, pointMaxAttackMaxStand) * unitDistanst.factorAttack + (3 - dPlateMove) * 25 * unitDistanst.factorOccupy;

                unitDistanst.unit.GetListMovePlates(out List<MovePlate> outlt);
                foreach (var platemove in outlt)
                {
                    dPlateMove = 0; d = 0; pointMaxAttackMaxMove = 0;

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

                    dPlateMove = (float)Math.Ceiling((float)unitDistanst.unit.Dijkstra(platemove, quest.ConstructionTarget, out _) / unitDistanst.unit.Move);
                    if (outltImportantTile != null && unitDistanst != quest.Captain)
                    {
                        ltImportantTile = outltImportantTile.Where(x => x.Exists(y => y.x == platemove.x && y.y == platemove.y)).ToList();
                        if (ltImportantTile.Count > outltImportantTile.Count)
                            dPlateMove += 1.5f;
                    }

                    point = Mathf.Max(pointMaxAttackMaxMove, pointMaxAttackMaxStand) * unitDistanst.factorAttack + (3 - dPlateMove) * 25 * unitDistanst.factorOccupy;
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
                if (movePlateBest != null)
                {
                    unitDistanst.unit.x = movePlateBest.x;
                    unitDistanst.unit.y = movePlateBest.y;
                }

                unitDistanst.unit.actions = plateActions;

                outltImportantTile.RemoveAll(x => x.Exists(y => y.x == movePlateBest.x && y.y == movePlateBest.y));

                outqueue.Enqueue(unitDistanst.unit);

                return;
            }
        }
    }
    public Unit BuyUnit(IMatrixCoordi matrixCoordi)
    {
        BaseTile bt = GameManager.Instance.bot.listOccupied.FirstOrDefault();
        GameManager.shop.x = matrixCoordi.x;
        GameManager.shop.y = matrixCoordi.y;

        while (true)
        {
            if (!GameManager.Instance.bot.CheckLimitUnit()) return null;

            if (GameManager.Instance.bot.arrListUnit.Count(x => x.labelUnit == LabelUnit.Soldier) <= 3)
            {
                if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_SOLIDER) return null;
                return GameManager.shop.Buy(Const.NameUnit.RED_SOLDIER);
            }
            else
            {
                if (GameManager.Instance.bot.arrListUnit.Count(x => x.labelUnit == LabelUnit.Archer) <= 3)
                {
                    if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_ARCHER) return null;
                    return GameManager.shop.Buy(Const.NameUnit.RED_ARCHER);
                }
                else
                {
                    if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_CAPUTAL) return null;
                    return GameManager.shop.Buy(Const.NameUnit.RED_CATAPULT);
                }
            }
        }
    }

    public void BuyUnitAuto()
    {
        Unit unit;
        foreach (var item in GameManager.Instance.bot.listOccupied)
        {
            if (item.MoveAble)
            {
                unit = BuyUnit(item);
                if (unit == null) return;
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
                float d = (float)Math.Ceiling((float)item.Dijkstra(null, quest.ConstructionTarget, out _) / item.Move);
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

        Unit unit;
        foreach (var item in ltQuest.Where(x => x.TypeQuest == eTypeQuest.GuardCastle || x.TypeQuest == eTypeQuest.GuardHouse))
        {
            if (item.feasibility < 0)
            {
                unit = BuyUnit(item.ConstructionTarget);
                if (!unit)
                {
                    switch (unit.labelUnit)
                    {
                        case LabelUnit.Soldier: item.ltUnitBot.Add(new UnitDistanst(unit, 0, 8 * 3.5f, true)); item.feasibility += 8 * 3.5f; break;
                        case LabelUnit.Archer: item.ltUnitBot.Add(new UnitDistanst(unit, 0, 10 * 3.5f, true)); item.feasibility += 10 * 3.5f; break;
                        case LabelUnit.Catapult: item.ltUnitBot.Add(new UnitDistanst(unit, 0, 14 * 3.5f, true)); item.feasibility += 14 * 3.5f; break;
                        case LabelUnit.General: item.ltUnitBot.Add(new UnitDistanst(unit, 0, 20 * 3.5f, true)); item.feasibility += 20 * 3.5f; break;
                    }
                }

            }
        }



        List<QuestBot> questBotRemove = new List<QuestBot>();
        foreach (var quest in ltQuest)
            if ((quest.TypeQuest == eTypeQuest.GuardCastle || quest.TypeQuest == eTypeQuest.GuardHouse) && quest.feasibility >= 0)
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
                    int d = (int)Math.Ceiling((float)item.Dijkstra(null, quest.ConstructionTarget, out _) / item.Move);
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