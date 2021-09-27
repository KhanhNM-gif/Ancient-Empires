using Assets.Asset.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    Unit reference = null;

    int matrixX;
    int matrixY;
    Queue<Cell> queueWay;

    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseDown()
    {
        /*controller = GameObject.FindGameObjectWithTag("GameController");
        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            Destroy(cp);
        }*/

        //controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Unit>().GetXMap(), reference.GetComponent<Unit>().GetYMap());

        reference.SetXMap(matrixX);
        reference.SetYMap(matrixY);
        reference.SetStackMove(queueWay);

        reference.DestroyMovePlate();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }
    public void SetReference(Unit obj)
    {
        reference = obj;
    }
    public void SetStackWay(Queue<Cell> queue)
    {
        queueWay = queue;
    }
    public Unit GetReference()
    {
        return reference;
    }
}
