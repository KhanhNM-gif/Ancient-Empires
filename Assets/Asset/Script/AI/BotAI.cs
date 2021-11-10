using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAI : MonoBehaviour
{
    // Start is called before the first frame update
    public void SetQuest()
    {
        List<BaseTile> listOccupied = new List<BaseTile>();

        foreach(var item in listOccupied)
        {
            foreach(var enemy in GameManager.Instance.player.arrListUnit)
            {

            }

            foreach (var tunnen in GameManager.Instance.player.listOccupied)
            {

            }
        }
    }



}

class Quest
{
    public List<BaseTile> listOccupied;
    public List<Unit>    arrListUnit;
}