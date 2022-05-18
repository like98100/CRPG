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

    // �ν����Ϳ��� ���� ���� ���̹Ƿ� �ʿ� �������� �Ǵ� �� ��ȯ�� ��
    public float SlotSize;
    public float SlotGapX;
    public float SlotGapY;

    public float RewardWidth;
    public float RewardHeight;
    float EmptySlot;

    // ī�� ����
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

                // ������ ���� ����
                GameObject slot = Instantiate(OriginCardSlot) as GameObject;
                // �������� RectTransform ����
                RectTransform slotRect = slot.GetComponent<RectTransform>();

                // ������Ʈ �̸�, �θ� ����
                slot.name = "rewardSlot(" + slotCount + ")";
                slot.transform.parent = RewardContent.transform;

                // ��ġ ����(y�� -�� ���� ������ �κ��丮 ĭ�� ������ �������� ����)
                //slotRect.localPosition = new Vector3((SlotSize * x) + (SlotGapX * (x + 1)), -((SlotSize * y) + (SlotGapY * (y + 1))), 0f);
                // ���� ������ ����
                slotRect.localScale = Vector3.one;

                // ����Ʈ�� ���� �߰�
                CardSlot.Add(slot);
                cnt++;
            }

            // �� ���� = ������ ����
            EmptySlot = CardSlot.Count;

            // ī�� ������ �����´�
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


            // Key���� stage value�̹Ƿ�             //stage�� �����      //�� ������ ID��
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

            // ó�� ȹ���ϴ� ī���� 0���� ��ȯ
            if (invenJson.itemData.items[rewardId].itemCount == -1) invenJson.itemData.items[rewardId].itemCount = 0;
            // ���� ���� ������ŭ �߰�(ȹ�� ���θ� -1�� ������ ������ �״�� ���ϸ� ������ �ϳ� ������)
            invenJson.itemData.items[rewardId].itemCount += rewardJson.GetRewardData().rewards[rewardStage].itemCnt[rewardCnt];

            rewardCnt++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
