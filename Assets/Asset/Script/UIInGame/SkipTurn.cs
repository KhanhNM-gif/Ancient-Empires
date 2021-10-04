using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkipTurn : MonoBehaviour
{
    public GameObject btn_SkipTurn;
    public GameObject pnNotification;
    public static SkipTurn Instance;

    private void Awake()
    {
        Instance = this;
        pnNotification.SetActive(false);
    }
    public void SkipTurn_OnClick()
    {
        Time.timeScale = 1;
        GameManager.Status = GameManager.eStatus.Turn_Bot;
        btn_SkipTurn.SetActive(false);
        Notification_Show("Enemy turn");
        GameManager.Instance.EndTurn();
    }
    public void Notification_Show(string msg)
    {
        GameObject nof = pnNotification.transform.GetChild(0).gameObject;
        nof.GetComponent<UnityEngine.UI.Text>().text = msg;
        pnNotification.SetActive(true);
        StartCoroutine(WaitForSeconds(pnNotification, 2));
    }
    IEnumerator WaitForSeconds(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        pnNotification.SetActive(false);
    }
}
