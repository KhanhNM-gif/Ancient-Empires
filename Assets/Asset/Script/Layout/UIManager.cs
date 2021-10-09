using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Slider healthBar;
    public Text HPText;

    public Text Damage;
    public Text Defense;

    public PlayerStatus Status;

    public Text Gold;

    public Text Lv;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowStatus(Unit unit)
    {
        healthBar.maxValue = unit.HP;
        healthBar.value = unit.CurrentHP;
        HPText.text = $"{unit.CurrentHP} / {unit.HP}";

        Damage.text = unit.Attack.ToString();
        Defense.text = unit.Armor.ToString();

        Lv.text = unit.Lv.ToString();
    }

    public void ShowStatus(Gold gold)
    {
        Gold.text = gold.ToString();
    }
}
