using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class MapManager : MonoBehaviour
{

    [SerializeField] private int width, height;
    [SerializeField] private ThickSnow tsnow;
    [SerializeField] private ThinSnow snow;
    [SerializeField] private BorderMap border;
    [SerializeField] private Castle castle;
    [SerializeField] private House house;
    [SerializeField] private Mountain mountain;
    [SerializeField] private Forest forest;
    [SerializeField] private Way way;
    private string path = "Assets/Asset/Map/map12x12.txt";


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
        StreamReader reader = new StreamReader(path);
        String line = "";
        int y = height;
        while ((line = reader.ReadLine()) != null)
        {
            String[] pos = line.Split(' ');
            int x = 0;
            foreach (String s in pos)
            {
                addMap(s, x, y);
                x++;
            }
            y--;
        }
        reader.Close();

        addBorderMap();

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }


    //Render map
    private void addMap(string value, int i, int j)
    {
        switch (value)
        {
            case "1": //ThickSnow
                Instantiate(tsnow, new Vector3(i, j), Quaternion.identity);
                break;
            case "2": //rung cay
                forest.updateForest();
                Instantiate(forest, new Vector3(i, j), Quaternion.identity);
                break;
            case "3"://nui
                Instantiate(mountain, new Vector3(i, j), Quaternion.identity);
                break;
            case "4"://nha dan
                Instantiate(house, new Vector3(i, j), Quaternion.identity);
                break;
            case "5":// thanh chinh
                Instantiate(castle, new Vector3(i, j), Quaternion.identity);
                break;
            case "6": //vien map
                Instantiate(border, new Vector3(i, j), Quaternion.identity);
                break;
            case "0"://Snow
                Instantiate(snow, new Vector3(i, j), Quaternion.identity);
                break;
            default: // duong di
                way.changeSprite(value);
                Instantiate(way, new Vector3(i, j), Quaternion.identity);
                break;
        }
    }

    //Them vien cho map
    private void addBorderMap()
    {
        for (int i = -1; i <= width; i++)
        {
            for (int j = -1; j <= height + 1; j++)
            {
                if ((i == -1 && j >= 0) || (j == 0 && i >= 0) || (i == width && j >= 0) || (j == height + 1 && i >= 0))
                {
                    addMap("6", i, j);
                }
            }
        }
    }




}