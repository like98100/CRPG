using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{
    static int stageNum;
    // Start is called before the first frame update
    void Start()
    {
        stageNum = -1;
    }

    public static int GetStageNum()
    {
        return stageNum;
    }
    void RayStage()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        
        if (Physics.Raycast(ray, out hit) && !InventoryButton.GetIsActivate() && !MenualButton.GetIsActivate())   // ray�� ���� �浹�� �����͸� hit�� �����Ѵ�, hit�� null�� �ƴϸ� �Ʒ� ����� �����Ѵ�.
        {
            Debug.Log(hit.transform.gameObject.name);

            switch (hit.transform.gameObject.name)
            {
                case "Stage01":
                    Debug.Log("Stage01�� ���ϴ�.");
                    LoadingSceneManager.LoadScene("IngameScene");
                    stageNum = 1;
                    break;
                case "Stage02":
                    Debug.Log("Stage02�� ���ϴ�.");
                    LoadingSceneManager.LoadScene("IngameScene");
                    stageNum = 2;
                    break;
                case "Stage03":
                    Debug.Log("Stage03�� ���ϴ�.");
                    LoadingSceneManager.LoadScene("IngameScene");
                    stageNum = 3;
                    break;
                default:
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RayStage();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // ī�� �κ� ��Ȱ��ȭ
            if (InventoryButton.GetIsActivate())
                InventoryButton.SetIsActivate(false);
            // �޴��� ��Ȱ��ȭ
            else if (MenualButton.GetIsActivate())
                MenualButton.SetIsActivate(false);
        }
    }
}
