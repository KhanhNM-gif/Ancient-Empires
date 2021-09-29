using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class MapManager : MonoBehaviour
{

    [SerializeField] private ThickSnow tsnow;
    [SerializeField] private ThinSnow snow;
    [SerializeField] private BorderMap border;
    [SerializeField] private Castle castle;
    [SerializeField] private House house;
    [SerializeField] private Mountain mountain;
    [SerializeField] private Forest forest;
    [SerializeField] private Way way;
    private string pathMap = "Assets/Asset/Map/map12x12.txt";
    public static MapTile map;
    private int width, height;


    [SerializeField] private Transform cam;
    // Start is called before the first frame update
    void Start()
    { 
        ReadAndAddMap();
    }

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
                y = height;
                map = new MapTile(width, height);
                continue;
            }

            int x = 0;
            foreach (String s in pos)
            {
                addMap(s, x, y);
                x++;
            }
            y--;
        }
        reader.Close();


        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }


    //Render map
    private void addMap(string value, int i, int j)
    {
        switch (value)
        {
            case "01": //ThickSnow
                tsnow.x = i;
                tsnow.y = j;
                map.arrTile[i, j] = tsnow;
                Instantiate(tsnow, new Vector3(i, j), Quaternion.identity);
                break;
            case "02": //rung cay
                forest.updateForest();
                forest.x = i;
                forest.y = j;
                map.arrTile[i, j] = forest;
                Instantiate(forest, new Vector3(i, j), Quaternion.identity);
                break;
            case "03"://nui
                mountain.x = i;
                mountain.y = j;
                map.arrTile[i, j] = mountain;
                Instantiate(mountain, new Vector3(i, j), Quaternion.identity);
                break;
            case "04"://nha dan
                house.x = i;
                house.y = j;
                map.arrTile[i, j] = house;
                Instantiate(house, new Vector3(i, j), Quaternion.identity);
                break;
            case "05":// thanh chinh
                castle.x = i;
                castle.y = j;
                map.arrTile[i, j] = castle;
                Instantiate(castle, new Vector3(i, j), Quaternion.identity);
                break;
            case "99": //vien map
                border.x = i;
                border.y = j;
                map.arrTile[i, j] = border;
                Instantiate(border, new Vector3(i, j), Quaternion.identity);
                break;
            case "00"://Snow
                snow.x = i;
                snow.y = j;
                map.arrTile[i, j] = snow;
                Instantiate(snow, new Vector3(i, j), Quaternion.identity);
                break;
            default: // duong di
                way.x = i;
                way.y = j;
                map.arrTile[i, j] = way;
                way.changeSprite(value);
                Instantiate(way, new Vector3(i, j), Quaternion.identity);
                break;
        }
    }






}