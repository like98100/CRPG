using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenualButton : MonoBehaviour
{
    static bool isActivate;
    Vector3[] menualScale = { new Vector3(0,0,0), new Vector3(1,1,1) };
    GameObject menual;

    PRS originPRS;

    public static bool GetIsActivate()
    {
        return isActivate;
    }

    public static void SetIsActivate(bool idx)
    {
        isActivate = idx;
    }

    // Start is called before the first frame update
    void Start()
    {
        isActivate = false;
        menual = GameObject.Find("Menual");
        menual.transform.localScale = menualScale[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivate && menual.transform.localScale.x == 0) // 메뉴얼 활성화 이후 현재 스케일이 0일 때
        {
            menual.transform.DOScale(menualScale[1], 0.25f);
        }
        else if(!isActivate && menual.transform.localScale.x == 1) // 메뉴얼 비활성화 이후 현재 스케일이 1일 때
        {
            menual.transform.DOScale(menualScale[0], 0.25f);
        }
    }

    public void IsActivateMenual()
    {
        if (!GetIsActivate() && !InventoryButton.GetIsActivate()) SetIsActivate(true);
    }
}
