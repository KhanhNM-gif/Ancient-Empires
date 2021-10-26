using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManage : MonoBehaviour
{
    public GameObject PopupDamage;
    public GameObject DestroyEffect;
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
