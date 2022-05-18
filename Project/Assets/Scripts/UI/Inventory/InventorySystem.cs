using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//using UnityEngine.EventSystems;

public class Item
{
    public enum ItemType
    {
        Weapon,
        Equipment,
        Card
    };

    int itemID;
    int itemCount;
    string itemName;

}


[System.Serializable]
public class CharClass
{
    public string CharTypeName;
    public string CharName;
    public float MaxHp;
    public float MaxMp;
    public float Hp;
    public float Mp;
    public string Weapon;
    public string Equipment;
    public List<int> CharDeckList = new List<int>();

    public float playerPosX_f, playerPosY_f;
    public int playerPosX_i, playerPosY_i;
    public float movementspeed;

    public CharClass()
    {
        CharTypeName = string.Empty;
        CharName = string.Empty;
        MaxHp = 10.0f;
        MaxMp = 10.0f;
        Hp = 2.0f;
        Mp = 2.0f;
        Weapon = "none";
        Equipment = "none";
        for (int i = 0; i < 9; i++)
        {
            int sum = i;
            //CharDeckList.Add(sum.ToString());
            CharDeckList.Add(sum);
        }
        playerPosX_f = 0.0f; playerPosY_f = 0.0f;
        playerPosX_i = 0; playerPosY_i = 0;
        movementspeed = 5.0f;
    }

    public virtual void Log()
    {
        Debug.Log("CharTypeName : " + CharTypeName);
        Debug.Log("Charname : " + CharName);
        Debug.Log("Hp : " + Hp);
        Debug.Log("Mp : " + Mp);
        Debug.Log("Weapon : " + Weapon);
        Debug.Log("Equipment : " + Equipment);
        Debug.Log("PositionF : " + playerPosX_f + ", " + playerPosY_f);
        Debug.Log("PositionINT : " + playerPosX_i + ", " + playerPosY_i);
        Debug.Log("MovementSpeed : " + movementspeed);
        //foreach (string Deck in CharDeckList) Debug.Log("Deck : " + Deck);
        for (int i = 0; i < CharDeckList.Count; i++) Debug.Log("Deck : " + CharDeckList[i]);
    }
}

[System.Serializable]
public class CharClass_Playable : CharClass
{
    public float nAttackDmg;
    public float nAttackSpd;

    public CharClass_Playable()
    {
        CharTypeName = typeof(CharClass_Playable).ToString();
        nAttackDmg = 1.0f;
        nAttackSpd = 6.5f;
    }

    public override void Log()
    {
        base.Log();
        Debug.Log("nAttackDmg : " + nAttackDmg);
        Debug.Log("nAttackSpd : " + nAttackSpd);
    }
}

public class InventorySystem : MonoBehaviour
//    , IPointerClickHandler      // 클릭
 //   , IPointerEnterHandler      // 호버링
 //   , IPointerExitHandler       // 호버링 해제
{
    public List<GameObject> CardSlot;
    public RectTransform CardRect;
    public GameObject OriginCardSlot;

    public float slotSize;
    public float slotGapX;
    public float slotGapY;
    //public float slotCountX;
    //public float slotCountY;
    // 가로 세로 사이즈(Deck과 같은 스크립트 사용을 위한 public화)
    public float invenWidth;
    public float invenHeight;
    float EmptySlot;

    //ScrollRect scrollRect;
    
    public CharClass_Playable player;
    public CardJSONCreator cardData;

    Image itemImage;
    TMP_Text itemName;
    TMP_Text itemFunc;
    TMP_Text itemMp;
    TMP_Text itemCooldown;

    TMP_Text itemCount;
    GameObject gameRoot;
    InventoryJsonCreator invenJson;

    ClickUICard isClicked;

    public GameObject PlayerInfoContent;
    public GameObject InventoryContent;

    // Start is called before the first frame update
    void Start()
    {
        gameRoot = GameObject.Find("GameRoot");
        invenJson = gameRoot.GetComponent<InventoryJsonCreator>();
        
        isClicked = null;

        // RectTransform에 설정
        CardRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, invenWidth);
        CardRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, invenHeight);

        InitInventory();

        // 플레이어의 정보를 가져온다
        //switch(this.gameObject.name)
        //{
        //case "DeckInfo":
        BringPlayerJSON(true);
            //    //player.Log();
            //    break;
            //case "Inventory":
            //    BringPlayerJSON(true);
            //    break;
            //default:
            //    break;
        //}

        // 카드 정보를 가져온다
        cardData = GameObject.Find("GameRoot").GetComponent<CardJSONCreator>();

    }


    // Update is called once per frame
    void Update()
    {
        BringPlayerJSON(true);
        SetDeck(this.gameObject.name, CardSlot);
    }

    void InitInventory()
    {
        for (int size = 0; size < slotSize; size++)
        {
            // 프리팹 슬롯 복사
            GameObject slot = Instantiate(OriginCardSlot) as GameObject;
            // 프리팹의 RectTransform 복사
            RectTransform slotRect = slot.GetComponent<RectTransform>();

            // 오브젝트 이름, 부모 설정
            switch (this.gameObject.name)
            {
                case "DeckInfo":
                    slot.name = "deckSlot(" + size + ")";

                    slot.transform.parent = PlayerInfoContent.transform;

                    // 슬롯 사이즈 설정
                    slotRect.localScale = Vector3.one;

                    // 리스트에 슬롯 추가
                    CardSlot.Add(slot);
                    break;
                case "Inventory":
                    // 획득하지 못한 카드 순번 슬롯이라면
                    if(invenJson.GetItemData().items[size].itemCount <= -1)
                    {
                        Destroy(slot);
                        break;
                    }
                    else
                    {
                        slot.name = "cardSlot(" + size + ")";

                        slot.transform.parent = InventoryContent.transform;

                        // 슬롯 사이즈 설정
                        slotRect.localScale = Vector3.one;

                        // 리스트에 슬롯 추가
                        CardSlot.Add(slot);
                        break;
                    }
                default:
                    break;
            }
        }

        // 빈 슬롯 = 슬롯의 숫자
        EmptySlot = CardSlot.Count;

        Debug.Log(this.gameObject.name + "'s emptyslot : " + EmptySlot);
    }

    int CheckPlayerDeckCnt(int idx)
    {
        int index = 0;
        for(int i = 0; i < 9; i++)
        {
            if (player.CharDeckList[i] == idx)
            {
                index++;
            }

        }

        return index;
    }

    public void SetDeck(string name, List<GameObject> slotList)
    {
        switch (name)
        {
            case "DeckInfo":
                int deckCnt = 0;
                for (int size = 0; size < EmptySlot; size++)
                {

                        GameObject slot = GameObject.Find("deckSlot(" + size + ")");
                        
                        // 빈 카드일 때 아예 안 보이게 하는 스크립트
                        // 현재는 제거 후 카드 추가 때 아예 오브젝트를 지워버리기 때문에 Null Reference 발생
                        // x/y 계산이 아닌 size로 연산하는 형태(RewardSystem 참고)로 변환 후, 덱 리스트 재배열하여 구현할 것
                        //if (player.CharDeckList[deckCnt] == 0)
                        //{
                        //    Destroy(slot);
                        //    deckCnt++;
                        //    continue;
                        //}


                        isClicked = slot.GetComponent<ClickUICard>();
                        //if (isClicked.GetEventObject() != 0) Debug.Log("클릭? : " + isClicked.GetEventObject());
                        // 플레이어 덱에 카드 추가
                        if (isClicked.GetEventObject() == 1) UndoCard(deckCnt);

                        // 카드 자식들 필요 내용 선언
                        itemImage = slot.transform.GetChild(0).GetComponent<Image>();
                        itemName = slot.transform.GetChild(1).GetComponent<TMP_Text>();
                        itemFunc = slot.transform.GetChild(2).GetComponent<TMP_Text>();
                        itemMp = slot.transform.GetChild(3).GetComponent<TMP_Text>();
                        itemCooldown = slot.transform.GetChild(4).GetComponent<TMP_Text>();

                        itemImage.sprite = Resources.Load<Sprite>("Sprites/CardImages/" + cardData.getCardImage(player.CharDeckList[deckCnt])) as Sprite;
                        itemName.text = cardData.getCardName(player.CharDeckList[deckCnt]);
                        itemFunc.text = cardData.getCardFunction(player.CharDeckList[deckCnt]);
                        itemMp.text = cardData.getCardMp(player.CharDeckList[deckCnt]).ToString();
                        itemCooldown.text = cardData.getCardCooldown(player.CharDeckList[deckCnt]).ToString();

                        deckCnt++;
                }
                break;

            case "Inventory":
                int invenCnt = 0;
                
                for (int size = 0; size < EmptySlot; size++)
                {
                        // itemData JSON 파일에 0번째 아이템이 테스트용 더미 아이템이기 때문에 1부터 시작한다.
                        GameObject slot = GameObject.Find("cardSlot(" + (size + 1) + ")");
                        while (true)
                        {
                            // 아이템Json에서 카드가 아닌 아이템이거나 획득하지 못한 카드일 때 다음 cnt로 넘어간다.
                            if (invenJson.GetItemData().items[invenCnt].itemType != InventoryJsonCreator.ItemType.Card 
                                || invenJson.GetItemData().items[invenCnt].itemCount == -1)
                                invenCnt++;
                            else break;
                        }
                        int itemId = invenJson.GetItemData().items[invenCnt].itemID;

                        isClicked = slot.GetComponent<ClickUICard>();
                        //if (isClicked.GetEventObject() != 0) Debug.Log("클릭? : " + isClicked.GetEventObject());
                        // 플레이어 덱에 카드 추가
                        if (isClicked.GetEventObject() == 2) AddCard(itemId);

                        // 카드 자식들 필요 내용 선언
                        itemImage = slot.transform.GetChild(0).GetComponent<Image>();
                        itemName = slot.transform.GetChild(1).GetComponent<TMP_Text>();
                        itemFunc = slot.transform.GetChild(2).GetComponent<TMP_Text>();
                        itemMp = slot.transform.GetChild(3).GetComponent<TMP_Text>();
                        itemCooldown = slot.transform.GetChild(4).GetComponent<TMP_Text>();
                        itemCount = slot.transform.GetChild(5).GetComponent<TMP_Text>();

                        itemImage.sprite = Resources.Load<Sprite>("Sprites/CardImages/" + cardData.getCardImage(itemId)) as Sprite;
                        itemName.text = cardData.getCardName(itemId);
                        itemFunc.text = cardData.getCardFunction(itemId);
                        itemMp.text = cardData.getCardMp(itemId).ToString();
                        itemCooldown.text = cardData.getCardCooldown(itemId).ToString();

                        itemCount.text = (invenJson.GetItemData().items[invenCnt].itemCount - CheckPlayerDeckCnt(itemId)).ToString();

                        invenCnt++;
                }
                break;
            default:
                break;
        }
    }

    void AddCard(int cardId)
    {
        Debug.Log("AddCard");
        if(CheckPlayerDeckCnt(0) != 0)  // 비어있는 카드가 없을 때
        {
            int idx;
            for(idx = 0; idx < 9; idx++)
            {
                if (player.CharDeckList[idx] == 0) break;
            }

            player.CharDeckList[idx] = cardId;
            OverWritePlayerJSON();


        }
        else
        {
            Debug.Log("더 이상 카드를 장비하실 수 없습니다.");
        }
    }

    void UndoCard(int cardIdx)
    {
        Debug.Log("UndoCard");
        player.CharDeckList[cardIdx] = 0;
        OverWritePlayerJSON();
    }

    void BringPlayerJSON(bool isPlayer)
    {
        if(isPlayer)
        {
            string data = this.gameObject.GetComponent<ControlJSON>().LoadJsonFile<CharClass_Playable>(Application.dataPath, "Player");

            player = JsonUtility.FromJson<CharClass_Playable>(data);
            //JsonUtility.FromJsonOverwrite(data, player);
        }
    }

    void OverWritePlayerJSON()
    {
        string playerToJsonData = this.GetComponent<ControlJSON>().ObjectToJson(player);
        this.GetComponent<ControlJSON>().OverWriteJsonFile(Application.dataPath, "Player", playerToJsonData);
    }
}
