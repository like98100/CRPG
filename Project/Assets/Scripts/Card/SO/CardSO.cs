using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[System.Serializable]
public class Card
{
    public int CardNum;
    public string CardName;
    public int Mp;
    public float Cooldown;
    public float currCooldown;
    public string Function;
    public string Image;
}

[CreateAssetMenu(fileName = "CardSO", menuName = "Scriptable Object/CardSO")]
public class CardSO : ScriptableObject
{
    public Card[] cards = new Card[18];        //0~8은 플레이어, 9~17은 적 카드로 배치 예정

    public void StartCardDraw(CardJSONCreator manager, PlayerAct player, EnemyAct enemy)
    {
        //Debug.Log("경로 이미지");
        //Debug.Log(Application.dataPath + "/Sprites/CardImages/" + manager.getCardImage(3));
        //Debug.Log(cards.Length);
        try
        {
            Debug.Log("cardlength : " + cards.Length);
            for (int i = 0; i < cards.Length; i++)
            {

                /*
                //JsonUtility.FromJsonOverwrite();
                //Debug.Log(GameObject.Find("CardManager").GetComponent<CardJSONCreator>().player2.cards[i + 1].CardNum);
                //Debug.Log(GameObject.Find("CardManager").GetComponent<CardJSONCreator>().player2.cards[i + 1].CardName);
                //Debug.Log(GameObject.Find("CardManager").GetComponent<CardJSONCreator>().player2.cards[i + 1].Mp);
                //Debug.Log(GameObject.Find("CardManager").GetComponent<CardJSONCreator>().player2.cards[i + 1].Cooldown);
                //Debug.Log(GameObject.Find("CardManager").GetComponent<CardJSONCreator>().player2.cards[i + 1].currCooldown);
                //Debug.Log(GameObject.Find("CardManager").GetComponent<CardJSONCreator>().player2.cards[i + 1].Function);
                //Debug.Log(GameObject.Find("CardManager").GetComponent<CardJSONCreator>().player2.cards[i + 1].Image);
                */

                if(i < 9)
                {
                    cards[i].CardNum = manager.getCardNum(player.player.CharDeckList[i]);
                    cards[i].CardName = manager.getCardName(player.player.CharDeckList[i]);
                    cards[i].Mp = (int)manager.getCardMp(player.player.CharDeckList[i]);
                    cards[i].Cooldown = manager.getCardCooldown(player.player.CharDeckList[i]);
                    cards[i].currCooldown = manager.getCardcurrCooldown(player.player.CharDeckList[i]);
                    cards[i].Function = manager.getCardFunction(player.player.CharDeckList[i]);
                    cards[i].Image = manager.getCardImage(player.player.CharDeckList[i]);
                }
                else
                {
                    //Debug.Log("EnemyCardNum : " + enemy.monster.mons[enemy.GetEnemyNum()].MonDeckList[i - 9]);
                    cards[i].CardNum = manager.getCardNum(enemy.monster.mons[enemy.GetEnemyNum()].MonDeckList[i - 9]);
                    cards[i].CardName = manager.getCardName(enemy.monster.mons[enemy.GetEnemyNum()].MonDeckList[i - 9]);
                    cards[i].Mp = (int)manager.getCardMp(enemy.monster.mons[enemy.GetEnemyNum()].MonDeckList[i - 9]);
                    cards[i].Cooldown = manager.getCardCooldown(enemy.monster.mons[enemy.GetEnemyNum()].MonDeckList[i - 9]);
                    cards[i].currCooldown = manager.getCardcurrCooldown(enemy.monster.mons[enemy.GetEnemyNum()].MonDeckList[i - 9]);
                    cards[i].Function = manager.getCardFunction(enemy.monster.mons[enemy.GetEnemyNum()].MonDeckList[i - 9]);
                    cards[i].Image = manager.getCardImage(enemy.monster.mons[enemy.GetEnemyNum()].MonDeckList[i - 9]);
                }

                //cards[i].CardNum = 123;
                //cards[i].CardName = "테스트";
                //cards[i].Mp = 10;
                //cards[i].Cooldown = 0;
                //cards[i].currCooldown = 0;
                //cards[i].Function = "임시입니다";
                //cards[i].Image = Resources.Load<Sprite>("./Spel41.png");

            }
        }
        catch(System.ArgumentOutOfRangeException exception)
        {
            Debug.Log(exception.Message);
        }

    }
}