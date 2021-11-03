using Assets.Asset.Model;
using System.Collections.Generic;
using static TypeQuest;

public class TypeQuest
{
    public enum eTypeQuest
    {
        OccupyCastle,
        OccupyEmptyHouse,
        OccupyEnemyHouse,
        GuardEmptyHouse,
        GuardEnemyHouse,
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
public class Quest
{
    //public List<Quest> ltChildQuest;
    public eTypeQuest TypeQuest;
    public List<TileDistance> ltConstructionBot;
    public List<TileDistance> ltConstructionPlayer;
    public BaseTile ConstructionTarget;
    public List<UnitDistanst> ltUnitPlayer;
    public List<UnitDistanst> ltUnitBot;
    public float feasibility;

    public Quest()
    {

    }

    public void Handle()
    {

    }


}

public class QuestManage
{
    List<Quest> ltQuest;


    public void Handle2()
    {
        GetListQuest();
        CalcularFeasibility();
    }

    public void Handle()
    {
        foreach (var item in ltQuest)
        {
            float distanceMin = 99;
            Unit unitChiem;
            foreach (var unitbot in item.ltUnitBot)
            {
                if (unitbot.unit.canOccupiedHouse)
                    if (unitbot.distance / unitbot.unit.Move < distanceMin)
                    {
                        distanceMin = unitbot.distance;
                        unitChiem = unitbot.unit;
                    }
                    else if (unitbot.distance / unitbot.unit.Move == distanceMin)
                    {

                    }
            }
        }
    }
    public void GetListQuest()
    {
        List<BaseTile> listTileBot = GameManager.Instance.bot.listOccupied;
        List<BaseTile> listTilePlayer = GameManager.Instance.player.listOccupied;
        List<Unit> listUnitBot = GameManager.Instance.bot.arrListUnit;
        List<Unit> listUnitPlayer = GameManager.Instance.player.arrListUnit;
        List<House> listHouse = MapManager.map.GetListHouse();

        foreach (var item in listHouse)
        {
            Quest quest = new Quest();
            AddToListTile(item, listTileBot, quest.ltConstructionBot);
            AddToListTile(item, listTilePlayer, quest.ltConstructionPlayer);

            AddToListUnit(item, listUnitBot, quest.ltUnitBot);
            AddToListUnit(item, listUnitPlayer, quest.ltUnitPlayer);
        }

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