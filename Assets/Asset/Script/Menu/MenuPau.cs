using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPau : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool isPause;

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
}
