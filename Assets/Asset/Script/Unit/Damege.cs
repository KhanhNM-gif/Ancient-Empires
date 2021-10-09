using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damege : MonoBehaviour
{

    public float MaxHP;
    public float CurrentHP;
    public float Armor;
    void Start()
    {
        CurrentHP = MaxHP; 
    }

    public void TakeDame(float damage)
    {
        CurrentHP -= damage * (100/(100 + Armor));

        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Died");
    }
}
