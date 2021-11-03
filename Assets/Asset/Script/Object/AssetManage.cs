using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManage : MonoBehaviour
{
    public GameObject PopupDamage;
    public GameObject DestroyEffect;
    public GameObject movePlates;
    public GameObject attackPlates;
    public GameObject dust;
    public GameObject Explo;
    public GameObject LeverUp;
    public GameObject Flame;
    public GameObject Heal;
    public GameObject HealUp;
    public static AssetManage i;

    // Start is called before the first frame update
    void Awake()
    {
        i = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
