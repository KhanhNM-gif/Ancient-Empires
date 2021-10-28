using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class Map1Manager : MonoBehaviour
{

    [SerializeField] private ThickSnow tsnow;
    [SerializeField] private ThinSnow snow;
    [SerializeField] private BorderMap border;
    [SerializeField] private Castle castle;
    [SerializeField] private House house;
    [SerializeField] private Mountain mountain;
    [SerializeField] private Forest forest;
    [SerializeField] private Way way;
    private string pathMap = "Assets/Asset/Map/map16x16.txt";
    public static MapTile map;
    private static int width, height;
    private static int size = 1;


    //[SerializeField] private Transform cam;

    public int GetWidth() => width;
    public int GetHeight() => height;
    public int GetSize() => size;

    //Doc map tu file text va render map
    public void ReadAndAddMap()
    {
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(pathMap);
        String line = "";
        int y = 0;
        while ((line = reader.ReadLine()) != null)
        {
            String[] pos = line.Split(' ');
            if (line.Length < 10)
            {
                height = int.Parse(pos[1]);
                width = int.Parse(pos[0]);
                y = height - 1;
                map = new MapTile(width, height);
                continue;
            }

            int x = 0;
            foreach (String s in pos)
            {
                NewMap(s, x, y);
                x++;
            }
            y--;
        }
        reader.Close();


        //cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }


    //Render map
    private void NewMap(string value, int i, int j)
    {
        switch (value)
        {
           
            case "01": //ThickSnow
                ThickSnow cloneThickSnow = Instantiate(tsnow, new Vector3(i, j), Quaternion.identity);
                cloneThickSnow.x = i;
                cloneThickSnow.y = j;
                cloneThickSnow.name = $"tile({i},{j})";
                map.arrTile[i, j] = cloneThickSnow;
                break;
            case "02": //rung cay
                Forest cloneForest = Instantiate(forest, new Vector3(i, j), Quaternion.identity);
                cloneForest.updateForest();
                cloneForest.x = i;
                cloneForest.y = j;
                cloneForest.name = $"tile({i},{j})";

                map.arrTile[i, j] = cloneForest;
                break;
            case "03"://nui
                Mountain cloneMountain = Instantiate(mountain, new Vector3(i, j), Quaternion.identity);
                cloneMountain.x = i;
                cloneMountain.y = j;
                cloneMountain.name = $"tile({i},{j})";

                map.arrTile[i, j] = cloneMountain;
                break;
            case "04"://nha dan
                House cloneHouse = Instantiate(house, new Vector3(i, j), Quaternion.identity);
                cloneHouse.x = i;
                cloneHouse.y = j;
                cloneHouse.name = $"tile({i},{j})";

                map.arrTile[i, j] = cloneHouse;
                break;
            case "05":// thanh chinh
                Castle cloneCastle = Instantiate(castle, new Vector3(i, j), Quaternion.identity);
                cloneCastle.x = i;
                cloneCastle.y = j;
                cloneCastle.name = $"tile({i},{j})";

                map.arrTile[i, j] = cloneCastle;
                break;
            case "99": //vien map
                BorderMap cloneBorderMap = Instantiate(border, new Vector3(i, j), Quaternion.identity);
                cloneBorderMap.x = i;
                cloneBorderMap.y = j;
                cloneBorderMap.name = $"tile({i},{j})";
                cloneBorderMap.changeBorderMap(i, j, width, height);
                map.arrTile[i, j] = cloneBorderMap;
                break;
            case "00"://Snow
                ThinSnow cloneThinSnow = Instantiate(snow, new Vector3(i, j), Quaternion.identity);
                cloneThinSnow.x = i;
                cloneThinSnow.y = j;
                cloneThinSnow.name = $"tile({i},{j})";

                map.arrTile[i, j] = cloneThinSnow;
                break;
            default: // duong di
                Way cloneWay = Instantiate(way, new Vector3(i, j), Quaternion.identity);
                cloneWay.x = i;
                cloneWay.y = j;
                cloneWay.name = $"tile({i},{j})";
                cloneWay.changeSprite(value);

                map.arrTile[i, j] = cloneWay;
                break;
            case "21":
                GameManager.Instance.player.arrListUnit.Add(GameManager.Instance.Create(Const.NameUnit.BLUE_GENERAL, i, j, false));
                break;
            case "22":
                GameManager.Instance.player.arrListUnit.Add(GameManager.Instance.Create(Const.NameUnit.BLUE_SOLDIER, i, j, false));
                break;
            case "23":
                GameManager.Instance.player.arrListUnit.Add(GameManager.Instance.Create(Const.NameUnit.BLUE_ARCHER, i, j, false));
                break;
            case "24":
                GameManager.Instance.player.arrListUnit.Add(GameManager.Instance.Create(Const.NameUnit.BLUE_CATAPULT, i, j, false));
                break;
            case "31":
                GameManager.Instance.bot.arrListUnit.Add(GameManager.Instance.Create(Const.NameUnit.RED_GENERAL, i, j, true));
                break;
            case "32":
                GameManager.Instance.bot.arrListUnit.Add(GameManager.Instance.Create(Const.NameUnit.RED_SOLDIER, i, j, true));
                break;
            case "33":
                GameManager.Instance.bot.arrListUnit.Add(GameManager.Instance.Create(Const.NameUnit.RED_ARCHER, i, j, true));
                break;
            case "34":
                GameManager.Instance.bot.arrListUnit.Add(GameManager.Instance.Create(Const.NameUnit.RED_CATAPULT, i, j, true));
                break;
        }
    }








}