using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // 싱글톤(매니저는 하나만 존재하므로)
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] private CardSO cardSO;
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private List<CardConfig> myCards;
    [SerializeField] public List<CardConfig> EnemyCards;

    [SerializeField] private Transform cardSpawnPoint;
    [SerializeField] private Transform enemyCardSpawnPoint;

    [SerializeField] private Transform myCardLeft;
    [SerializeField] private Transform myCardRight;
    [SerializeField] private Transform enemyCardLeft;
    [SerializeField] private Transform enemyCardRight;

    [SerializeField] SpeechBubble speechBubble;
    [SerializeField] TutorialBubble tutorialBubble;


    private List<Card> myCardBuffer, enemyCardBuffer;       //플레이어, 적
    public float time;
    CardConfig selectCard;
    CardConfig enemyCard;
    public CardJSONCreator data;
    public PlayerAct playerData;
    public EnemyAct enemyData;
    public GameObject soManager, player, enemy;
    private bool cardClicked;

    public CardFunction cardFunc;
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemy = GameObject.FindWithTag("Enemy");
        cardFunc = this.GetComponent<CardFunction>();
        //GameObject.Find("CardManager").GetComponent<CardSO>().Start();
        //Debug.Log(cardSO.cards.Length);
        data = soManager.GetComponent<CardJSONCreator>();
        data.BringCharJSON();

        playerData = player.GetComponent<PlayerAct>();
        enemyData = enemy.GetComponent<EnemyAct>();
        //Debug.Log(playerData.player.CharTypeName + "이 이름입니다.");
        //Debug.Log(data.player2.cards[1].CardName);
        cardSO.StartCardDraw(data, playerData, enemyData);
        SetUpCardBuffer(true);
        SetUpCardBuffer(false);
        time = 0.0f;


        GameManager.OnAddCard = AddCard;
        cardClicked = false;
    }

    private void OnDestroy()
    {
        //GameManager.OnAddCard -= AddCard;
    }
    private void Update()
    {
        time += Time.deltaTime;
        //if ((Input.GetKeyDown(KeyCode.Keypad1) || time % 5.0f == 0.0f ) && myCards.Count < 9)
        //{
        //    AddCard(true);
        //}

        //if ((Input.GetKeyDown(KeyCode.Keypad2) || time % 5.0f == 0.0f) && EnemyCards.Count < 9)
        //{
        //    AddCard(false);
        //}
    }

    public int getCardCount(bool isMine)
    {
        if (isMine) return myCards.Count;
        else return EnemyCards.Count;
    }

    void SetUpCardBuffer(bool isMine)
    {
        //myCardBuffer = new List<Card>(100);
        //for(int i = 0; i < cardSO.cards.Length; i++)
        //{
        //    Card card = cardSO.cards[i];
        //    for (int j = 0; j < 1; j++) myCardBuffer.Add(card);   // 버퍼에 카드 별 아이템 1개씩 추가 (카드마다 확률이 바뀌게 된다면 카드 클래스에 확률 변수를 만들고 card.(확률변수)로 바꿀 것
        //}

        //for(int i = 0; i < myCardBuffer.Count; i++)   // 10개씩 정렬되어 있는 카드 아이템들을 무작위로 셔플
        //{
        //    int rand = Random.Range(i, myCardBuffer.Count);

        //    Card temp = myCardBuffer[i];
        //    myCardBuffer[i] = myCardBuffer[rand];
        //    myCardBuffer[rand] = temp;
        //}
        if(isMine)
        {
            myCardBuffer = new List<Card>(50);
            for (int i = 0; i < /*cardSO.cards.Length / 2*/9; i++)
            {
                Card card = cardSO.cards[i];
                for (int j = 0; j < 1; j++) myCardBuffer.Add(card); // 버퍼에 카드 별 아이템 1개씩 추가 (카드마다 확률이 바뀌게 된다면 카드 클래스에 확률 변수를 만들고 card.(확률변수)로 바꿀 것(버퍼 개수에 따라 반복문이 필요해서 반복문으로 만듬)

                for(int k = 0; k < myCards.Count; k++)
                {
                    if(myCardBuffer[i].CardNum == myCards[k].cardItem.CardNum)
                    {
                        //Debug.Log("이미 손에 존재하는 카드 : " + myCardBuffer[i].CardName);
                    }
                }
            }

            for (int i = 0; i < myCardBuffer.Count; i++)   // 1개씩 정렬되어 있는 카드 아이템들을 무작위로 셔플
            {
                int rand = Random.Range(i, myCardBuffer.Count);

                Card temp = myCardBuffer[i];
                myCardBuffer[i] = myCardBuffer[rand];
                myCardBuffer[rand] = temp;
            }
        }
        else
        {
            enemyCardBuffer = new List<Card>(50);
            for (int i = 9; i < cardSO.cards.Length; i++)
            {
                Card card = cardSO.cards[i];
                for (int j = 0; j < 1; j++)   // 버퍼에 카드 별 아이템 1개씩 추가 (카드마다 확률이 바뀌게 된다면 카드 클래스에 확률 변수를 만들고 card.(확률변수)로 바꿀 것
                {
                    enemyCardBuffer.Add(card);
                }
            }

            for (int i = 0; i < enemyCardBuffer.Count; i++)   // 1개씩 정렬되어 있는 카드 아이템들을 무작위로 셔플(적 버전)
            {
                int rand = Random.Range(i, enemyCardBuffer.Count);

                Card temp = enemyCardBuffer[i];
                enemyCardBuffer[i] = enemyCardBuffer[rand];
                enemyCardBuffer[rand] = temp;
            }
        }

    }

    public Card PopCard(bool isMine)
    {
        if(isMine)
        {
            if (myCardBuffer.Count == 0) SetUpCardBuffer(true);   // 버퍼 개수가 0개(아래의 리무브로 초기화)일 때 다시 버퍼 생성

            Card card = myCardBuffer[0];  // 셔플한 버퍼 중 가장 앞의 것을 가져옴
            myCardBuffer.RemoveAt(0);     // 남은 버퍼 아이템 제거

            return card;
        }
        else
        {
            if (enemyCardBuffer.Count == 0) SetUpCardBuffer(false);   // 버퍼 개수가 0개(아래의 리무브로 초기화)일 때 다시 버퍼 생성

            Card card = enemyCardBuffer[0];  // 셔플한 버퍼 중 가장 앞의 것을 가져옴
            enemyCardBuffer.RemoveAt(0);     // 남은 버퍼 아이템 제거

            return card;
        }

    }

    public void AddCard(bool isMine)   // 트루면 내거, 펄스면 남의거
    {
        var cardObject = isMine ? Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI) : Instantiate(cardPrefab, enemyCardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<CardConfig>();
        card.Setup(PopCard(isMine), isMine);

        if (isMine) myCards.Add(card);
        else EnemyCards.Add(card);

        SetOriginOrder(isMine);
        CardAlignment(isMine);

        //if (isMine) Debug.Log("플레이어가 드로우한 카드 : " + card.cardItem.CardName);
        //else Debug.Log("적군이 드로우한 카드 : " + card.cardItem.CardName);
    }

    public void SetOriginOrder(bool isMine)
    {
        int count;
        if (isMine) count = myCards.Count;
        else count = EnemyCards.Count;

        for(int i = 0; i < count; i++)
        {
            var targetCard = isMine ? myCards[i] : EnemyCards[i];       // 내 덱과 적의 덱 선택
            if (targetCard != null) targetCard?.GetComponent<Order>().SetOriginOrder(i);        //? 는 타겟 카드가 존재할 시 실행한다는 뜻.  Order에 접근해서 SetOriginOrder 실행(전체 카드 정렬)
            else { }
        }
    }

    public void CardAlignment(bool isMine) // 정렬(SetOriginOrder 이후 사용 예정)
    {
        List<PRS> originCardPRSs = new List<PRS>();
        if (isMine) originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 0.1f);
        else originCardPRSs = RoundAlignment(enemyCardLeft, enemyCardRight, EnemyCards.Count, 0.5f, Vector3.one * 0.1f);

        var targetCards = isMine ? myCards : EnemyCards;        // 누구 카드인지 확인
        for(int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];                    // 카드 하나 선택

            //targetCard.originPRS = new PRS(Vector3.zero, Utils.QI, Vector3.one * 0.1f); // 위치 지정
            if(targetCard != null)
            {
                targetCard.originPRS = originCardPRSs[i]; // 위치 지정
                targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
            }
         // 이동
        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)    // 원형정렬(height는 원의 반지름(0.5f))
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);        // 용량 미리 잡아놓기

        switch(objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1.0f / (objCount - 1);
                for (int i = 0; i < objCount; i++) objLerps[i] = interval * i;
                break;
        }

        for(int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.Euler(30.0f, 0f, 0f);
            if(objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                //targetPos.y += curve;
                //targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]); //ui 변경으로 폐기
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }

        return results;
    }

    #region MyCard

    public void CardMouseOver(CardConfig card)
    {
        //print("CardMouseOver");
        selectCard = card;
        EnlargeCard(true, card);
    }

    public void CardMouseExit(CardConfig card)
    {
        //print("CardMouseExit");
        selectCard = null;
        EnlargeCard(false, card);
    }

    public void CardMouseDown()
    {
        cardClicked = true;
    }

    public void CardMouseUp(CardConfig card)    // 마우스 뗐을 때 전용
    {
        cardClicked = false;
        if (card != null)
        {
            //카드 능력
            if(speechBubble.PanelTMP.text == "마우스 우클릭으로" + System.Environment.NewLine + "기본 공격을 할 수 있습니다."
                || speechBubble.PanelTMP.text == "지금은" + System.Environment.NewLine + "기본 공격을 사용해 봅시다.")
            {
                speechBubble.PanelTMP.text = "지금은" + System.Environment.NewLine + "기본 공격을 사용해 봅시다.";
                selectCard = null;
            }
            else 
            {
                playerData.player.Mp -= card.cardItem.Mp;
                if (playerData.player.Mp < 0)
                {
                    playerData.player.Mp += card.cardItem.Mp;
                    selectCard = null;
                    Debug.Log("마나가 부족합니다.");
                    Debug.Log("사용이 취소되었습니다.");

                    speechBubble.Show("마나가" + System.Environment.NewLine + "부족합니다.");

                }
                else if (!cardFunc.RunCardFunction(card.cardItem.CardNum, true))
                {
                    playerData.player.Mp += card.cardItem.Mp;
                    selectCard = null;
                    Debug.Log("사용이 취소되었습니다.");

                    speechBubble.Show("손패에" + System.Environment.NewLine + "공간이 없습니다.");
                }
                else
                {
                    if (tutorialBubble.bubbleObject.transform.localScale == Vector3.one && tutorialBubble.tutoFlag == 1) tutorialBubble.Show("잘 하셨습니다.", 1);    // 말풍선 최소화
                }
            }
            
        }
    }

    public CardConfig GetSelectCard()
    {
        return selectCard;
    }

    public List<CardConfig> GetCharCards(bool isMine)
    {
        if (isMine) return myCards;
        else return EnemyCards;
    }

    //private void CardClick(CardConfig card)
    //{
    //    //Destroy(card, 0.0f);
    //    Debug.Log("선택된 카드 : " + card.cardItem.CardName);
    //    for(int i = 0; i < myCards.Count; i++)
    //    {
    //        if(myCards[i].cardItem == card.cardItem)
    //        {
    //            myCards.RemoveAt(i);
    //            Debug.Log("리스트에서 제거");
    //            SetOriginOrder(true);
    //            CardAlignment(true);

    //            Destroy(this, 0.0f);

    //            Debug.Log("오브젝트 제거");
    //        }
    //    }
    //}

    public void DestroyCards(bool isMine, int index)
    {
        if(isMine)
        {
            myCards.RemoveAt(index);
            //for(int i = 0; i < myCards.Count; i++) Debug.Log("myCardName : " + myCards[i].cardItem.CardName);
            //Debug.Log("리스트에서 제거");
            SetOriginOrder(true);
            CardAlignment(true);
        }
        else
        {
            EnemyCards.RemoveAt(index);
            //for (int i = 0; i < EnemyCards.Count; i++) Debug.Log("EnemyCardName : " + EnemyCards[i].cardItem.CardName);
            //Debug.Log("리스트에서 제거");
            SetOriginOrder(false);
            CardAlignment(false);
        }
    }

    void EnlargeCard(bool isEnlarge, CardConfig card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, card.originPRS.pos.y + 2.0f, card.originPRS.pos.z + 1.25f);        //z축은 마우스 포인터 오류를 막기 위해 카메라에 가깝게 당기는 용도
            
            card.MoveTransform(new PRS(enlargePos, card.originPRS.rot, /*Vector3.one * 0.15f*/card.originPRS.scale), true, 0.1f);
        }
        else card.MoveTransform(card.originPRS, true, 0.1f);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    #endregion


    #region EnemyCard

    public void UseEnemyCard(CardConfig card)
    {
        if(card != null)
        {
            enemyCard = card;

            //카드 능력
            enemyData.monster.mons[enemyData.GetEnemyNum()].Mp -= card.cardItem.Mp;
            if (enemyData.monster.mons[1].Mp < 0)
            {
                enemyData.monster.mons[1].Mp += card.cardItem.Mp;
                //selectCard = null;
                Debug.Log("적의 마나가 부족합니다.");
                Debug.Log("적의 카드 사용이 취소되었습니다.");

            }
            else if (!cardFunc.RunCardFunction(card.cardItem.CardNum, false))
            //if (!cardFunc.RunCardFunction(card.cardItem.CardNum, false))
            {
                    enemyData.monster.mons[enemyData.GetEnemyNum()].Mp += card.cardItem.Mp;
                    //selectCard = null;
                    Debug.Log("적의 카드 사용이 취소되었습니다.");
            }
                else enemyCard.DestroyEnemyCard(enemyCard);

        }
    }
    #endregion
}
