using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupDamage : MonoBehaviour
{
    public static PopupDamage CreatePopupDamage(float Damage,Vector3 position)
    {
        GameObject clone = Instantiate( AssetManage.i.PopupDamage,position, Quaternion.identity);

        PopupDamage popupDamage = clone.GetComponent<PopupDamage>();
        popupDamage.Setup(Damage);

        return null;
    }

    private TextMeshPro textMesh;
    private Color textColor;
    private float disapperTimer=1f;

    void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Setup(float Damage)
    {
        textMesh.SetText(((int)Damage).ToString());
        textColor = textMesh.color;
        disapperTimer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        float moveYSpeedTime = 1f;
        transform.position += new Vector3(0, moveYSpeedTime) * Time.deltaTime;
        disapperTimer -= Time.deltaTime;
        if (disapperTimer <= 0)
        {
            float disapperSpeed = 3f;
            textColor.a -= disapperSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject); 
            }
        }

    }
}
