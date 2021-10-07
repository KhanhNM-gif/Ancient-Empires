using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float HP;
    public float CurrentHP;
    public float Armor;
    void Start()
    {
        CurrentHP = HP;
    }

    public void TakeDame(float damage)
    {
        HP -= damage * (100/(100+Armor));
        if (CurrentHP <= 0 )
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Died");
    }

   
}
