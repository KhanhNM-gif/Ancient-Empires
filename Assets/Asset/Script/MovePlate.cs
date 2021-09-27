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
    Stack<Cell> stackWay;

    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        stackWay = new Stack<Cell>();
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
        reference.SetXMap(matrixY);
        reference.SetStackMove(stackWay);

        //controller.GetComponent<Game>().SetPosition(reference);

//        reference.DestroyMovePlate();
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
    public void SetStackWay(Stack<Cell> stack)
    {
        stackWay = stack;
    }
    public Unit GetReference()
    {
        return reference;
    }
}
