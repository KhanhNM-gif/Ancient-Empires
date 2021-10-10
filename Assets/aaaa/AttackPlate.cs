using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    int matrixX;
    int matrixY;

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
        controller = GameObject.FindGameObjectWithTag("GameController");
        attack = true;
        // if (attack)
        // {
        //     GameObject cp = controller.GetComponent<G>().GetPosition(matrixX, matrixY);

        //     Destroy(cp);
        // }

        // controller.GetComponent<G>().SetPositionEmpty(reference.GetComponent<Catapult>().GetXMap(), reference.GetComponent<Catapult>().GetYMap());

        // reference.GetComponent<Catapult>().SetXMap(matrixX);
        // reference.GetComponent<Catapult>().SetYMap(matrixY);
        // reference.GetComponent<Catapult>().SetCoords();

        // controller.GetComponent<G>().SetPosition(reference);

        reference.GetComponent<Catapult>().DestroyattackPlate();
        reference.GetComponent<Catapult>().Shoot();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }
    public void SetReference(GameObject obj)
    {
        reference = obj;
    }
    public GameObject GetReference()
    {
        return reference;
    }
}
