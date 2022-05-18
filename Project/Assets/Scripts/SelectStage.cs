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

        
        if (Physics.Raycast(ray, out hit) && !InventoryButton.GetIsActivate() && !MenualButton.GetIsActivate())   // ray를 쏴서 충돌한 데이터를 hit에 저장한다, hit가 null이 아니면 아래 기능을 실행한다.
        {
            Debug.Log(hit.transform.gameObject.name);

            switch (hit.transform.gameObject.name)
            {
                case "Stage01":
                    Debug.Log("Stage01로 들어갑니다.");
                    LoadingSceneManager.LoadScene("IngameScene");
                    stageNum = 1;
                    break;
                case "Stage02":
                    Debug.Log("Stage02로 들어갑니다.");
                    LoadingSceneManager.LoadScene("IngameScene");
                    stageNum = 2;
                    break;
                case "Stage03":
                    Debug.Log("Stage03로 들어갑니다.");
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
            // 카드 인벤 비활성화
            if (InventoryButton.GetIsActivate())
                InventoryButton.SetIsActivate(false);
            // 메뉴얼 비활성화
            else if (MenualButton.GetIsActivate())
                MenualButton.SetIsActivate(false);
        }
    }
}
