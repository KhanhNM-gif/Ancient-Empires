using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{

    public GameObject controller;
    public GameObject attackPlates;

    public int xMap = 1;
    public int yMap = 1;
    private int attackPlate = 4;
    public int moveSpeed = 2;
    public float launchForce = 13;

    
    public string player;
    public bool attack = false;
    public Transform firePoint;
    public GameObject arrow;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();
    }

    public void SetCoords()
    {
        float x = xMap;
        float y = yMap;

        x += 0.5f;
        y += 0.5f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXMap()
    {
        return xMap;
    }
    public int GetYMap()
    {
        return yMap;
    }
    public void SetXMap(int x)
    {
        xMap = x;
    }
    public void SetYMap(int y)
    {
        yMap = y;
    }

    public void OnMouseDown()
    {
        DestroyattackPlate();
        InitiateattackPlates();
        attack = true;
    }

    public void DestroyattackPlate()
    {
        GameObject[] attackPlates = GameObject.FindGameObjectsWithTag("AttackPlate");
        for (int i = 0; i < attackPlates.Length; i++)
        {
            Destroy(attackPlates[i]);
        }
    }
    public void DestroyRock()
    {
        GameObject[] arrow = GameObject.FindGameObjectsWithTag("Arrow");
        for (int i = 0; i < arrow.Length; i++)
        {
            Destroy(arrow[i]);
        }
    }

    public void InitiateattackPlates()
    {
        for (int i = xMap - attackPlate; i <= xMap + attackPlate; i++)
        {
            for (int j = yMap - attackPlate; j <= yMap + attackPlate; j++)
            {
                int xDistance = i - xMap;
                int yDistance = j - yMap;
                if (xDistance < 0)
                {
                    xDistance = -xDistance;
                }
                if (yDistance < 0)
                {
                    yDistance = -yDistance;
                }
                if ((xDistance + yDistance <= attackPlate && !(i == xMap && j == yMap)) && xDistance + yDistance != 1)
                {
                    attackPlateSpawn(i, j);
                }
            }
        }
    }

    public void attackPlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x += 0.5f;
        y += 0.5f;

        GameObject mp = Instantiate(attackPlates, new Vector3(x, y, -3.0f), Quaternion.identity);
        AttackPlate mpScript = mp.GetComponent<AttackPlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
    public void attackPlateDestroy(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x += 0.5f;
        y += 0.5f;

        GameObject mp = Instantiate(attackPlates, new Vector3(x, y, -3.0f), Quaternion.identity);

        AttackPlate mpScript = mp.GetComponent<AttackPlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
    public void Shoot()
    {
        GameObject arrowClone = Instantiate(arrow, firePoint.position, firePoint.rotation);
        RockMove rm = arrowClone.GetComponent<RockMove>();
        rm.SetlastClickedPos(Input.mousePosition);
        rm.SetMoving(true);
    }
}