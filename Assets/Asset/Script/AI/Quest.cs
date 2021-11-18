using Assets.Asset.Model;
using Assets.Asset.Script;
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
                    item.Captain = item.ltUnitBot.Where(x => x.unit.canOccupiedHouse).OrderBy(x => x.distance).ThenByDescending(x => x.unit.CurrentHP).FirstOrDefault();
                else
                    item.Captain = item.ltUnitBot.Where(x => x.unit.canOccupiedCastle).FirstOrDefault();

                item.Captain.SetPoitUnit(0.5f, 2, 1.25f);
                int type = 1;

                UnitDistanst player = item.ltUnitPlayer.OrderBy(x => x.distance).FirstOrDefault();
                if (player != null)
                {
                    if (player.distance == 0) type = 3;
                    else type = item.Captain.distance > player.distance ? 1 : 2;
                }


                foreach (var unit in item.ltUnitBot.Where(x => x != item.Captain))
                {
                    if (type == 1) unit.SetPoitUnit(1.25f, 0.5f, 1f);
                    else if (type == 2) unit.SetPoitUnit(1.25f, 0.5f, 0.5f);
                    else unit.SetPoitUnit(2f, 0, 0.5f);
                }
            }
            else
            {
                UnitDistanst player = item.ltUnitPlayer.OrderBy(x => x.distance).FirstOrDefault();

                item.Captain = item.ltUnitBot.OrderBy(x => x.distance).ThenByDescending(x => x.unit.CurrentHP).FirstOrDefault();

                item.Captain.SetPoitUnit(1.25f, 1.5f, 0.75f);

                foreach (var unit in item.ltUnitBot.Where(x => x != item.Captain))
                {
                    unit.SetPoitUnit(1.5f, 1f, 0.75f);
                }
            }
        }
    }

    public void Handle(out Queue<IPlateAction> outqueue)
    {
        outqueue = new Queue<IPlateAction>();
        foreach (var quest in ltQuest)
        {

            List<List<IMatrixCoordi>> outltImportantTile = null;
            if (quest.Captain != null)
                quest.Captain.unit.Dijkstra(null, quest.ConstructionTarget, out outltImportantTile, false);

            foreach (var unitDistanst in quest.ltUnitBot)
            {
                Tuple<MovePlate, float, List<List<IMatrixCoordi>>> tulpeMovePlateSelect;
                Tuple<AttackPlate, float> tulpeAttackPlateSelect;
                float pointMax, point;

                Score(unitDistanst, quest, unitDistanst.unit, outltImportantTile, out Tuple<AttackPlate, float> outAttackPlateIndie,
                    out float pointIndie, out List<List<IMatrixCoordi>> outTileConflict);
                Tuple<MovePlate, float, List<List<IMatrixCoordi>>> tulpeMovePlateIndie
                    = new Tuple<MovePlate, float, List<List<IMatrixCoordi>>>(null, pointIndie, outTileConflict);

                tulpeMovePlateSelect = tulpeMovePlateIndie;
                tulpeAttackPlateSelect = outAttackPlateIndie;
                pointMax = tulpeMovePlateIndie.Item2 + outAttackPlateIndie.Item2;

                Tuple<AttackPlate, float> outAttackPlate;
                Tuple<MovePlate, float, List<List<IMatrixCoordi>>> tulpeMovePlate;
                float pointMove;

                unitDistanst.unit.GetListMovePlates(out List<MovePlate> outlt);
                foreach (var platemove in outlt)
                {
                    Score(unitDistanst, quest, platemove, outltImportantTile, out outAttackPlate, out pointMove, out outTileConflict);
                    tulpeMovePlate = new Tuple<MovePlate, float, List<List<IMatrixCoordi>>>(platemove, pointMove, outTileConflict);

                    if (outAttackPlate.Item2 + tulpeMovePlate.Item2 > outAttackPlateIndie.Item2 + tulpeMovePlate.Item2)
                    {
                        point = outAttackPlate.Item2 + tulpeMovePlate.Item2;
                        if (point > pointMax)
                        {
                            tulpeMovePlateSelect = tulpeMovePlate;
                            tulpeAttackPlateSelect = outAttackPlate;
                            pointMax = point;
                        }
                    }
                    else
                    {
                        point = outAttackPlateIndie.Item2 + tulpeMovePlate.Item2;
                        if (point > pointMax)
                        {
                            tulpeMovePlateSelect = tulpeMovePlate;
                            tulpeAttackPlateSelect = outAttackPlateIndie;
                            pointMax = point;
                        }
                    }

                }

                if (tulpeAttackPlateSelect == outAttackPlateIndie)
                {
                    if (!Command.IsNull(tulpeAttackPlateSelect.Item1))
                    {
                        outqueue.Enqueue(tulpeAttackPlateSelect.Item1);
                        tulpeAttackPlateSelect.Item1.Prepare();
                    }
                    if (!Command.IsNull(tulpeMovePlateSelect.Item1))
                    {
                        outqueue.Enqueue(tulpeMovePlateSelect.Item1);
                        tulpeMovePlateSelect.Item1.Prepare();
                    }
                }
                else
                {
                    if (!Command.IsNull(tulpeMovePlateSelect.Item1))
                    {
                        outqueue.Enqueue(tulpeMovePlateSelect.Item1);
                        tulpeMovePlateSelect.Item1.Prepare();
                    }
                    if (!Command.IsNull(tulpeAttackPlateSelect.Item1))
                    {
                        outqueue.Enqueue(tulpeAttackPlateSelect.Item1);
                        tulpeAttackPlateSelect.Item1.Prepare();

                    }
                }

                if (tulpeMovePlateSelect.Item3 != null)
                    foreach (var item in tulpeMovePlateSelect.Item3) outltImportantTile.Remove(item);
            }
        }
    }
    private void Score(UnitDistanst obj, QuestBot quest, IMatrixCoordi position, List<List<IMatrixCoordi>> outltImportantTile, out Tuple<AttackPlate, float> outAttackPlateBest, out float outMovePlateBest, out List<List<IMatrixCoordi>> outTileConflict)
    {
        outAttackPlateBest = new Tuple<AttackPlate, float>(null, 0);
        outTileConflict = null;
        float dPlateMove = 0, d = 0, pointMaxAttackMaxMove = 0;

        obj.unit.GetAttackPlates(position, out List<AttackPlate> outLtAttackPlate);
        foreach (var attackPlate in outLtAttackPlate)
        {
            var unitplayer = quest.ltUnitPlayer.Where(x => attackPlate.target == x.unit).FirstOrDefault();
            d = unitplayer == null ? 4 : unitplayer.point;

            if (d > pointMaxAttackMaxMove)
            {
                pointMaxAttackMaxMove = d;
                outAttackPlateBest = new Tuple<AttackPlate, float>(attackPlate, d * obj.factorAttack);
            }
        }
        float pointAttackM = 0;
        float pointAttackT = 0;
        foreach (var item in quest.ltUnitPlayer)
        {
            d = Math.Abs(item.unit.x - position.x) + Math.Abs(item.unit.y - position.y);
            if (item.unit.Range >= d) pointAttackM -= (item.unit.Attack / 10) * obj.factorSafe;
            float p = (3.5f - (d - obj.unit.BaseRange) / obj.unit.Move) / 3.5f * item.point * obj.factorAttack;
            if (p > pointAttackT)
                pointAttackT = p;
        }

        dPlateMove = (float)Math.Ceiling((float)obj.unit.Dijkstra(position, quest.ConstructionTarget, out _) / obj.unit.Move);
        if (dPlateMove > 2.5) dPlateMove = 2.5f;

        if (CanBuyUnit())
        {
            if (MapManager.map.arrTile[position.x, position.y].IsHouse && ((House)MapManager.map.arrTile[position.x, position.y]).isOwnerBy == 2) dPlateMove = 3;
            if (MapManager.map.arrTile[position.x, position.y].IsCastle && ((Castle)MapManager.map.arrTile[position.x, position.y]).isOwnerBy == 2) dPlateMove = 3;

        }

        float dPlateMovePoint = (3 - dPlateMove) * 25 * obj.factorOccupy;

        if (outltImportantTile != null && obj.unit != quest.Captain.unit)
        {
            outTileConflict = outltImportantTile.Where(x => x.Exists(y => y.x == position.x && y.y == position.y)).ToList();
            if (outTileConflict.Count > outltImportantTile.Count)
                dPlateMove += 1.5f;
        }

        outMovePlateBest = dPlateMovePoint + pointAttackM + pointAttackT;
    }
    public Unit BuyUnit(IMatrixCoordi matrixCoordi)
    {
        GameManager.shop.x = matrixCoordi.x;
        GameManager.shop.y = matrixCoordi.y;

        if (!GameManager.Instance.bot.CheckLimitUnit()) return null;

        if (!GameManager.Instance.bot.hasGeneral)
        {
            if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_GENERAL) return null;
            return GameManager.shop.BuyBot(Const.NameUnit.BLUE_GENERAL);
        }
        else
        {
            if (GameManager.Instance.bot.arrListUnit.Count(x => x.labelUnit == LabelUnit.Soldier) <= 2)
            {
                if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_SOLIDER) return null;
                return GameManager.shop.BuyBot(Const.NameUnit.RED_SOLDIER);
            }
            else
            {
                if (GameManager.Instance.bot.arrListUnit.Count(x => x.labelUnit == LabelUnit.Archer) <= 2)
                    if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_ARCHER) return null;
                    else
                    if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_CAPUTAL) return null;
                return GameManager.shop.BuyBot(Const.NameUnit.RED_CATAPULT);
            }
        }

    }
    public bool CanBuyUnit()
    {
        if (!GameManager.Instance.bot.CheckLimitUnit()) return false;

        if (GameManager.Instance.bot.arrListUnit.Count(x => x.labelUnit == LabelUnit.Soldier) <= 2)
        {
            if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_SOLIDER) return false;
            return true;
        }
        else
        {
            if (GameManager.Instance.bot.arrListUnit.Count(x => x.labelUnit == LabelUnit.Archer) <= 2)
            {
                if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_ARCHER) return false;
                return true;
            }
            else
            {
                if (GameManager.Instance.bot.Gold < Const.ConstGame.COST_CAPUTAL) return false;
                return true;
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
                float coefficient = quest.TypeQuest == TypeQuest.eTypeQuest.OccupyCastle ? 1.1f : 1f;
                if (d <= 2)
                {
                    switch (item.labelUnit)
                    {
                        case LabelUnit.Soldier: quest.ltUnitPlayer.Add(new UnitDistanst(item, (int)d, Const.PowerUnit.SOLDIER_POWER * (3f - d))); ; quest.feasibility -= coefficient * 8 * (3f - d); break;
                        case LabelUnit.Archer: quest.ltUnitPlayer.Add(new UnitDistanst(item, (int)d, Const.PowerUnit.ARCHER_POWER * (3f - d))); quest.feasibility -= coefficient * 9 * (3f - d); break;
                        case LabelUnit.Catapult: quest.ltUnitPlayer.Add(new UnitDistanst(item, (int)d, Const.PowerUnit.CATAPULT_POWER * (3f - d))); quest.feasibility -= coefficient * 12 * (3f - d); break;
                        case LabelUnit.General: quest.ltUnitPlayer.Add(new UnitDistanst(item, (int)d, Const.PowerUnit.GENERAL_POWER * (3f - d))); quest.feasibility -= coefficient * 12 * (3f - d); break;
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
                if (unit)
                {
                    switch (unit.labelUnit)
                    {
                        case LabelUnit.Soldier: item.ltUnitBot.Add(new UnitDistanst(unit, 0, Const.PowerUnit.SOLDIER_POWER * 3.5f, true)); item.feasibility += 8 * 3.5f; break;
                        case LabelUnit.Archer: item.ltUnitBot.Add(new UnitDistanst(unit, 0, Const.PowerUnit.ARCHER_POWER * 3.5f, true)); item.feasibility += 9 * 3.5f; break;
                        case LabelUnit.Catapult: item.ltUnitBot.Add(new UnitDistanst(unit, 0, Const.PowerUnit.CATAPULT_POWER * 3.5f, true)); item.feasibility += 12 * 3.5f; break;
                        case LabelUnit.General: item.ltUnitBot.Add(new UnitDistanst(unit, 0, Const.PowerUnit.GENERAL_POWER * 3.5f, true)); item.feasibility += 12 * 3.5f; break;
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
        while (true)
        {
            foreach (var item in listUnitBot)
            {
                int minD = 99;

                QuestBot questSelect = null;
                if (item.isGeneral && ltQuest.Count(x => x.TypeQuest == TypeQuest.eTypeQuest.OccupyCastle) > 0)
                {
                    questSelect = ltQuest.Where(x => x.TypeQuest == TypeQuest.eTypeQuest.OccupyCastle).First();
                    minD = (int)Math.Ceiling((float)item.Dijkstra(null, questSelect.ConstructionTarget, out _) / item.Move);
                }
                else
                {
                    foreach (var quest in ltQuest.Where(x => !x.isEligible))
                    {
                        int d = (int)Math.Ceiling((float)item.Dijkstra(null, quest.ConstructionTarget, out _) / item.Move);
                        if (d >= 3) d = 3;
                        if (minD > d)
                        {
                            minD = d;
                            questSelect = quest;
                        }
                    }
                }

                if (questSelect != null)
                {
                    if (((questSelect.TypeQuest == eTypeQuest.OccupyEmptyHouse || questSelect.TypeQuest == eTypeQuest.OccupyEnemyHouse) && item.canOccupiedHouse) || (questSelect.TypeQuest == eTypeQuest.OccupyCastle && item.canOccupiedCastle))
                        questSelect.NumberUnitOrCanOccupied++;

                    switch (item.labelUnit)
                    {
                        case LabelUnit.Soldier: questSelect.ltUnitBot.Add(new UnitDistanst(item, minD, Const.PowerUnit.SOLDIER_POWER * (3.5f - minD))); ; questSelect.feasibility += 8 * (3.5f - minD); break;
                        case LabelUnit.Archer: questSelect.ltUnitBot.Add(new UnitDistanst(item, minD, Const.PowerUnit.ARCHER_POWER * (3.5f - minD))); questSelect.feasibility += 9 * (3.5f - minD); break;
                        case LabelUnit.Catapult: questSelect.ltUnitBot.Add(new UnitDistanst(item, minD, Const.PowerUnit.CATAPULT_POWER * (3.5f - minD))); questSelect.feasibility += 12 * (3.5f - minD); break;
                        case LabelUnit.General: questSelect.ltUnitBot.Add(new UnitDistanst(item, minD, Const.PowerUnit.GENERAL_POWER * (3.5f - minD))); questSelect.feasibility += 12 * (3.5f - minD); break;
                    }
                }
            }

            if (ltQuest.Count(x => x.feasibility <= 0) == 0 || ltQuest.Count() == 1) return;

            listUnitBot = new List<Unit>();
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
                        listUnitBot.Add(unitDistanstRemove.unit);
                    }

                    quest.isEligible = true;
                }
            }

            ltQuest = new LinkedList<QuestBot>(ltQuest.OrderByDescending(x => x.feasibility).ToList());
            List<Unit> unitFromQuestRemove = new List<Unit>(ltQuest.Last.Value.ltUnitBot.Select(x => x.unit));
            if (ltQuest.Count(x => !x.isEligible) == 1 && unitFromQuestRemove.Count() > 0) return;
            else
            {
                listUnitBot.AddRange(unitFromQuestRemove);
                ltQuest.RemoveLast();
            }
        }
    }
}