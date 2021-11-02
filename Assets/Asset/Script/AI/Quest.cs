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
public class Quest
{
    public List<Quest> ListChildQuest;
    public eTypeQuest TypeQuest;
    public List<BaseTile> ListConstructionBot;
    public List<BaseTile> ListConstructionPlayer;
    public BaseTile ConstructionTarget;
    public List<Unit> ltUnitPlayer;
    public List<Unit> ltUnitBot;
    public float feasibility;

    public Quest()
    {

    }
}

public class QuestManage
{
    List<Quest> ltQuest;

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

        }

    }


}