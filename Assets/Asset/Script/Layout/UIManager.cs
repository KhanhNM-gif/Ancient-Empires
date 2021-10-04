using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Slider healthBar;
    public Text HPText;
    public PlayerHealthManager playerHealth;

    public Text Damage;
    public Text Defense;

    public PlayerStatus Status;

    public Text Gold;
    public Gold coin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.maxValue = playerHealth.playerMaxHealth;
        healthBar.value = playerHealth.playerCurrentHealth;
        HPText.text = "" + playerHealth.playerCurrentHealth + "/" + playerHealth.playerMaxHealth;

        Damage.text = "" + Status.damage ;
        Defense.text = "" + Status.defense ;

        Gold.text = "" + coin.gold ;
    }
}
