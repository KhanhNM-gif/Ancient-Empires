using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPau : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool isPause;
    [SerializeField] private GameObject ruleUI;
    [SerializeField] private bool isOpen;

    private void Update()
    {
        if (isPause)
        {
            ActivateMenu();
        }
        else {
            DeactivateMenu();
        }
    }
    private void Start()
    {
        if (isOpen)
        {
            ActiveRuleUI();
        }
        else
        {
            DeactiveRuleUI();
        }
    }
    public void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenuUI.SetActive(true);
        isPause = true;
    }
    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenuUI.SetActive(false);
        isPause = false;
    }
    public void ActiveRuleUI()
    {
        ruleUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        isOpen = false;
        isPause = false;
    }
    public void DeactiveRuleUI()
    {
        ruleUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        isOpen = false;
        isPause = false;

    }
}
