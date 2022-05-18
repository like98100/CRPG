using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Reward
{
    int itemID;
    int itemCnt;
}

public class RewardSystem : MonoBehaviour
{
    public List<GameObject> CardSlot;
    public RectTransform CardRect;
    public GameObject OriginCardSlot;

    // 인스펙터에서 조종 위한 것이므로 필요 없어졌다 판단 시 변환할 것
    public float SlotSize;
    public float SlotGapX;
    public float SlotGapY;

    public float RewardWidth;
    public float RewardHeight;
    float EmptySlot;

    // 카드 정보
    CardJSONCreator cardData;

    Image rewardImage;
    TMP_Text rewardName;
    TMP_Text rewardFunc;
    TMP_Text rewardMp;
    TMP_Text rewardCooldown;

    TMP_Text rewardCount;
    GameObject rewardManager;
    RewardJsonCreator rewardJson;

    InventoryJsonCreator invenJson;

    public GameObject RewardContent;
    int rewardStage;
    // Start is called before the first frame update
    void Start()
    {
        //rewardStage = 1;
        rewardStage = SelectStage.GetStageNum();
        rewardManager = GameObject.Find("RewardManager");
        rewardJson = rewardManager.GetComponent<RewardJsonCreator>();

        invenJson = rewardManager.GetComponent<InventoryJsonCreator>();

        if(GameManager.IsVictory)
        {
            int cnt = 0;
            for (int slotCount = 0; slotCount < SlotSize; slotCount++)
            {
                if (rewardJson.GetRewardData().rewards[rewardStage].itemID.Count <= slotCount) break;

                // 프리팹 슬롯 복사
                GameObject slot = Instantiate(OriginCardSlot) as GameObject;
                // 프리팹의 RectTransform 복사
                RectTransform slotRect = slot.GetComponent<RectTransform>();

                // 오브젝트 이름, 부모 설정
                slot.name = "rewardSlot(" + slotCount + ")";
                slot.transform.parent = RewardContent.transform;

                // 위치 설정(y에 -를 넣은 이유는 인벤토리 칸이 밑으로 내려가기 때문)
                //slotRect.localPosition = new Vector3((SlotSize * x) + (SlotGapX * (x + 1)), -((SlotSize * y) + (SlotGapY * (y + 1))), 0f);
                // 슬롯 사이즈 설정
                slotRect.localScale = Vector3.one;

                // 리스트에 슬롯 추가
                CardSlot.Add(slot);
                cnt++;
            }

            // 빈 슬롯 = 슬롯의 숫자
            EmptySlot = CardSlot.Count;

            // 카드 정보를 가져온다
            cardData = rewardManager.GetComponent<CardJSONCreator>();

            InitRewards();
            invenJson.OverwriteItemJSON();
        }

    }

    void InitRewards()
    {
        int rewardCnt = 0;
        for (int slotCount = 0; slotCount < EmptySlot; slotCount++)
        {
            GameObject slot = GameObject.Find("rewardSlot(" + slotCount + ")");


            // Key값이 stage value이므로             //stage의 보상들      //그 보상의 ID들
            int rewardId = rewardJson.GetRewardData().rewards[rewardStage].itemID[rewardCnt];

            rewardImage = slot.transform.GetChild(0).GetComponent<Image>();
            rewardName = slot.transform.GetChild(1).GetComponent<TMP_Text>();
            rewardFunc = slot.transform.GetChild(2).GetComponent<TMP_Text>();
            rewardMp = slot.transform.GetChild(3).GetComponent<TMP_Text>();
            rewardCooldown = slot.transform.GetChild(4).GetComponent<TMP_Text>();
            rewardCount = slot.transform.GetChild(5).GetComponent<TMP_Text>();

            rewardImage.sprite = Resources.Load<Sprite>("Sprites/CardImages/" + cardData.getCardImage(rewardId)) as Sprite;
            rewardName.text = cardData.getCardName(rewardId);
            rewardFunc.text = cardData.getCardFunction(rewardId);
            rewardMp.text = cardData.getCardMp(rewardId).ToString();
            rewardCooldown.text = cardData.getCardCooldown(rewardId).ToString();

            rewardCount.text = rewardJson.GetRewardData().rewards[rewardStage].itemCnt[rewardCnt].ToString();

            // 처음 획득하는 카드라면 0으로 변환
            if (invenJson.itemData.items[rewardId].itemCount == -1) invenJson.itemData.items[rewardId].itemCount = 0;
            // 이후 보상 개수만큼 추가(획득 여부를 -1로 따지기 때문에 그대로 더하면 보상이 하나 누락됨)
            invenJson.itemData.items[rewardId].itemCount += rewardJson.GetRewardData().rewards[rewardStage].itemCnt[rewardCnt];

            rewardCnt++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
