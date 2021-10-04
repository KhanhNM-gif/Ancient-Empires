using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStatus : MonoBehaviour
{
    public GameObject Status;

    public void OpenStatus()
    {
        if(Status != null)
        {
            bool isActive = Status.activeSelf;

            Status.SetActive(!isActive);
        }
    }
    
}
