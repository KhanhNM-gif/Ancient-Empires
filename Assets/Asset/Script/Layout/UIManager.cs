using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pnStatus;
    public Slider healthBar;
    public Text HPText;

    public Text Damage;
    public Text Defense;
    public Text AtkRange;
    public Text MoveStep;

    public static UIManager Instance;

    public Text Gold;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    public void UpdateStatus(Unit unit)
    {
        pnStatus.SetActive(true);
        healthBar.maxValue = unit.HP;
        healthBar.value = unit.CurrentHP;
        HPText.text = "" + unit.CurrentHP + "/" + unit.HP;

        Damage.text = unit.Attack.ToString() + " Atk";
        Defense.text = unit.Armor.ToString() + " Def";
        AtkRange.text = unit.Range.ToString() + " Square";
        MoveStep.text = unit.Move.ToString() + " Square";
    }
    public void SetActivePnStatus(bool s)
    {
        pnStatus.SetActive(s);
    }
    public void UpdateGold(int gold)
    {
        Gold.text = Gold.ToString();
    }
}
