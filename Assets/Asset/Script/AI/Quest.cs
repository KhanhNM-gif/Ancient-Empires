using Assets.Asset.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
}
public class UnitDistanst
{
    public Unit unit;
    public int distance;

    public UnitDistanst(Unit unit, int distance)
    {
        this.unit = unit;
        this.distance = distance;
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


    public void Handle2()
    {
        CalcularFeasibility();
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
                double d = Math.Round((float)item.Dijkstra(null, quest.ConstructionTarget) / item.Move);
                if (d <= 2)
                {
                    quest.ltUnitPlayer.Add(new UnitDistanst(item, (int)d));
                    switch (item.labelUnit)
                    {
                        case LabelUnit.Soldier: quest.feasibility -= 8; break;
                        case LabelUnit.Archer: quest.feasibility -= 10; break;
                        case LabelUnit.Catapult: quest.feasibility -= 14; break;
                        case LabelUnit.General: quest.feasibility -= 20; break;
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



        ltQuest = new LinkedList<QuestBot>(ltQuest.OrderBy(x => x.feasibility).ToList());
        while (ltQuest.Count > 4)
        {
            foreach (var item in listUnitBot)
            {
                int minD = 99;

                QuestBot questSelect = null;
                foreach (var quest in ltQuest)
                {


                    int d = (int)Math.Round((float)item.Dijkstra(null, quest.ConstructionTarget) / item.Move);
                    if (minD > d)
                    {
                        minD = d;
                        questSelect = quest;
                    }
                }

                if (questSelect != null)
                {
                    questSelect.ltUnitBot.Add(new UnitDistanst(item, minD));
                    if (((questSelect.TypeQuest == eTypeQuest.OccupyEmptyHouse || questSelect.TypeQuest == eTypeQuest.OccupyEnemyHouse) && item.canOccupiedHouse) || (questSelect.TypeQuest == eTypeQuest.OccupyCastle && item.canOccupiedCastle)) questSelect.NumberUnitOrCanOccupied++;

                    switch (item.labelUnit)
                    {
                        case LabelUnit.Soldier: questSelect.feasibility += 8; break;
                        case LabelUnit.Archer: questSelect.feasibility += 10; break;
                        case LabelUnit.Catapult: questSelect.feasibility += 14; break;
                        case LabelUnit.General: questSelect.feasibility += 20; break;
                    }
                }
            }

            ltQuest = new LinkedList<QuestBot>(ltQuest.OrderBy(x => x.feasibility).ToList());
            listUnitBot = new List<Unit>(ltQuest.Last.Value.ltUnitBot.Select(x => x.unit));
            ltQuest.RemoveLast();

            bool canComplete = true;
            UnitDistanst unitDistanstRemove;
            foreach (var quest in ltQuest)
            {
                if (quest.TypeQuest == eTypeQuest.OccupyEmptyHouse || quest.TypeQuest == eTypeQuest.OccupyEnemyHouse || quest.TypeQuest == eTypeQuest.OccupyCastle)
                    if (quest.NumberUnitOrCanOccupied == 0) canComplete = false;

                if (quest.feasibility > 10)
                {
                    if (quest.NumberUnitOrCanOccupied > 1) unitDistanstRemove = quest.ltUnitBot.OrderBy(x => x.distance).First();
                    else unitDistanstRemove = quest.ltUnitBot.Where(x => x.unit.canOccupiedCastle).OrderBy(x => x.distance).First();
                    listUnitBot.Add(unitDistanstRemove.unit);
                    quest.ltUnitBot.Remove(unitDistanstRemove);
                }

                if (quest > 0)
            }
        }


        //loại bỏ các nhiệm vụ bất khả thi 
    }
    private void AddToListTile(IMatrixCoordi matrixCoordi, List<BaseTile> matrixCoordis, List<TileDistance> add)
    {
        foreach (var item in matrixCoordis)
        {
            int d = item.Distance(matrixCoordi);
            if (d > 3) add.Add(new TileDistance(item, d));
        }
    }

    private void AddToListUnit(IMatrixCoordi matrixCoordi, List<Unit> matrixCoordis, List<UnitDistanst> add)
    {
        foreach (var item in matrixCoordis)
        {
            int d = item.Distance(matrixCoordi);
            if (d > 3) add.Add(new UnitDistanst(item, d));
        }
    }
    private void CalcularFeasibility()
    {
        foreach (var item in ltQuest)
        {
            //Tính mức độ khả thi tương đối
        }
    }

    private void DivisionUnit()
    {
        //Với các nv có độ khả thì 'quá cao' phân chia lại quân vào các nhiệm vụ khác
        //với các nhiệm vụ bất khả thi bỏ nhiệm vụ
        //Phân chia đến khi đạt độ ổn định
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