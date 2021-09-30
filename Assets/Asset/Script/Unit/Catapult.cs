using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public RockMove rockmove;
    public GameObject rock;
    public void OnMouseDown()
    {
        rock.SetActive(true);
        rockmove.move();
    }
}
