using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] public GameObject EndGameUI;
    [SerializeField] public Button ButtonNewGame;
    [SerializeField] public Button ButtonPause;
    [SerializeField] public GameObject SkipTurn;
    [SerializeField] public Text EndText;
    [SerializeField] public Text NextLevelText;
    public GameObject pnStatus;
    public Slider healthBar;
    public Text HPText;

    public Text Damage;
    public Text Defense;
    public Text AtkRange;
    public Text MoveStep;
    public Text Level;
    public static UIManager Instance;

    public Text Gold;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Gold.text = Const.ConstGame.GOLD_START_GAME + "";
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
        Level.text = "Lv " + unit.Lv.ToString();
    }
    public void SetActivePnStatus(bool s)
    {
        pnStatus.SetActive(s);
    }
    public void UpdateGold(int gold)
    {
        Gold.text = gold + "";
    }

    public void ShowEndGame(bool win)
    {

        if (win)
        {
            ButtonNewGame.onClick.AddListener(() => {
                NextLevel();
            });
            EndText.text = "You Win";
            NextLevelText.text = "Next Level";
        }
        else
        {
            ButtonNewGame.onClick.AddListener(() => {
                ReStartGame();
            });
            EndText.text = "Defeated";
            NextLevelText.text = "Play again";
        }
        ButtonPause.gameObject.SetActive(false);
        SkipTurn.SetActive(false);
        pnStatus.SetActive(false);
        EndGameUI.SetActive(true);
    }

    private void ReStartGame()
    {
        SceneManager.LoadScene(GameManager.Instance.MapName);
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(GameManager.Instance.MapName);
    }
}
