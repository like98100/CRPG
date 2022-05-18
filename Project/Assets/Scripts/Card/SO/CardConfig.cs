using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;  //두트윈 에셋 사용

public class CardConfig : MonoBehaviour
{
    public int cardNum;
    [SerializeField] private SpriteRenderer card;
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text MpTMP;
    [SerializeField] private TMP_Text CooldownTMP;
    [SerializeField] private TMP_Text FunctionTMP;
    [SerializeField] private Sprite cardFront;
    [SerializeField] private Sprite cardBack;       // 적군용 카드(뒷면)

    public Card cardItem;
    bool isFront;
    public PRS originPRS;

    //public static CardConfig Inst { get; private set; }
    //void Awake() => Inst = this;

    public void Setup(Card cardItem, bool isFront)
    {
        this.cardItem = cardItem;
        this.isFront = isFront;
        
        string path = Application.dataPath + "/Sprites/CardImages/";
        if (this.isFront)
        {
            cardNum = this.cardItem.CardNum;
            Sprite sprite = Resources.Load<Sprite>("Sprites/CardImages/" + this.cardItem.Image);
            image.sprite = sprite;
            nameTMP.text = this.cardItem.CardName;
            MpTMP.text = this.cardItem.Mp.ToString();
            CooldownTMP.text = this.cardItem.Cooldown.ToString();
            FunctionTMP.text = this.cardItem.Function;
        }
        else
        {
            cardNum = this.cardItem.CardNum;
            card.sprite = cardBack;
            nameTMP.text = this.cardItem.CardName;
            //nameTMP.text = "";
            MpTMP.text = "";
            CooldownTMP.text = "";
            FunctionTMP.text = "";
           // FunctionTMP.text = this.cardItem.Function;
        }
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if(useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else   // 두트윈 미사용시
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

    private void OnMouseOver()
    {
        if (isFront)
            CardManager.Inst.CardMouseOver(this);
    }

    private void OnMouseExit()
    {
        if (isFront)
            CardManager.Inst.CardMouseExit(this);
    }

    private void OnMouseDown()
    {
        if (isFront)
            CardManager.Inst.CardMouseDown();
    }

    private void OnMouseUp()
    {
        if (isFront)
        {
            CardManager.Inst.CardMouseUp(this);
            if (CardManager.Inst.GetSelectCard() != null) CardClick(this);
        }
    }

    //public void UseEnemyCard(CardConfig EnemyCard)
    //{
    //    if (EnemyCard != null)
    //    {
    //        CardManager.Inst.UseEnemyCard(EnemyCard);
    //        DestroyEnemyCard(EnemyCard);

    //    }
    //}

    public void DestroyEnemyCard(CardConfig card)
    {
        List<CardConfig> charCards = CardManager.Inst.GetCharCards(false);
        //Debug.Log("적이 선택한 카드 : " + card.cardItem.CardName);
        for (int i = 0; i < charCards.Count; i++)
        {
            if (charCards[i].cardItem == card.cardItem)
            {
                //cardmanager에서 수행해야 됨
                //myCards.RemoveAt(i);
                //Debug.Log("리스트에서 제거");        
                //SetOriginOrder(true);
                //CardAlignment(true);
                CardManager.Inst.DestroyCards(false, i);

                // 클릭 시 이펙트 추가할 것
                Destroy(card.gameObject, 0.0f);
                //CardManager.Inst.SetOriginOrder(true);
                //CardManager.Inst.CardAlignment(true);
                //Debug.Log("적 카드 오브젝트 제거");
                break;
            }
        }
    }

    private void CardClick(CardConfig selectCard)
    {
        List<CardConfig> charCards = CardManager.Inst.GetCharCards(true);
        //Debug.Log("선택된 카드 : " + selectCard.cardItem.CardName);
        //Debug.Log("선택된 카드 : " + selectCard.cardItem.CardNum);

        for (int i = 0; i < charCards.Count; i++)
        {
            if (charCards[i].cardItem == selectCard.cardItem)
            {
                //cardmanager에서 수행해야 됨
                //myCards.RemoveAt(i);
                //Debug.Log("리스트에서 제거");        
                //SetOriginOrder(true);
                //CardAlignment(true);
                CardManager.Inst.DestroyCards(true, i);

                // 클릭 시 이펙트 추가할 것
                Destroy(this.gameObject, 0.0f);
                //CardManager.Inst.SetOriginOrder(true);
                //CardManager.Inst.CardAlignment(true);
                //Debug.Log("오브젝트 제거");
                break;
            }

        }

    }

}
