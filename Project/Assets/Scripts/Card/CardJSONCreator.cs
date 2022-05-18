using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.IO;
using System.Text;

public class CardJSONCreator : MonoBehaviour
{

    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }

    public class DataManager
    {
        public Dictionary<int, CardClass> CardDict { get; private set; } = new Dictionary<int, CardClass>();

    }

    [System.Serializable]
    public class CardClass
    {
        public int CardNum;
        public string CardName;
        public float Mp;
        public float Cooldown;
        public float currCooldown;
        public string Function;
        public string Image;

        public CardClass()
        {
            CardNum = -999;
            CardName = "NULL";
            Mp = -9.0f;
            Cooldown = -9.0f;
            currCooldown = 0f;
            Function = "NULL";
            Image = "NULL";
        }

        public void Log()
        {
            //Debug.Log(CardNum);
            //Debug.Log(CardName);
            //Debug.Log(Mp);
            //Debug.Log(Cooldown);
            //Debug.Log(currCooldown);
            //Debug.Log(Function);
            //Debug.Log(Image);
        }
    }

    [System.Serializable]
    public class CardDataClass : ILoader<int, CardClass>
    {
        public List<CardClass> cards = new List<CardClass>();   //json 파일에서 이 리스트에 데이터가 옮겨질 예정

        public Dictionary<int, CardClass> MakeDict()        //오버라이딩
        {
            Dictionary<int, CardClass> dict = new Dictionary<int, CardClass>();
            foreach (CardClass cardClass in cards)       // 리스트에서 dictionary로 옮기는 작업
                dict.Add(cardClass.CardNum, cardClass);      // cardnum을 키값으로 설정
                    return dict;
        }

        public CardDataClass()
        {
            CardClass nullCard = new CardClass();
            cards.Add(nullCard);
        }
    }

    public GameObject playObject;
    public CardDataClass player, player2;
    // Start is called before the first frame update
    void Start()
    {
        //CardClass[] CardInfo = new CardClass[2];
        //CardInfo[0] = new CardClass();
        //CardInfo[0].CardNum = 0;
        //CardInfo[0].CardName = "기본 카드 1";
        //CardInfo[0].Mp = 1.0f;
        //CardInfo[0].Cooldown = 5.0f;
        //CardInfo[0].currCooldown = 5.0f;
        //CardInfo[0].Function = "기본 카드 1입니다.";
        //CardInfo[0].Image = "Spel41";

        //CardInfo[1] = new CardClass();
        //CardInfo[1].CardNum = 0;
        //CardInfo[1].CardName = "기본 카드 2";
        //CardInfo[1].Mp = 1.0f;
        //CardInfo[1].Cooldown = 5.0f;
        //CardInfo[1].currCooldown = 5.0f;
        //CardInfo[1].Function = "기본 카드 2입니다.";
        //CardInfo[1].Image = "Spel42";

        //player = new CardDataClass();
        //for(int i = 0; i < player.cards.Count; i++)
        //{
        //    player.cards[i].Log();
        //}

        //CreateCharJSON();
        //Debug.Log("불러오기");
        //BringCharJSON();

        //CardClass alphaCard = new CardClass();
        //alphaCard.CardName = "두번째 테스트카드";
        //for (int i = 0; i < 19; i++) player2.cards.Add(alphaCard);
        //OverwriteCharJSON();
        BringCharJSON();

        //for (int i = 0; i < 19; i++)
        //{
        //    player2.cards[i].Log();
        //}

        //player2.cards[0] = player2.cards[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public CardDataClass getCardPlayer()
    {
        return player2;
    }

    public int getCardNum(int idx)
    {
        return player2.cards[idx].CardNum;
    }

    public string getCardName(int idx)
    {
        return player2.cards[idx].CardName;
    }

    public float getCardMp(int idx)
    {
        return player2.cards[idx].Mp;
    }

    public float getCardCooldown(int idx)
    {
        return player2.cards[idx].Cooldown;
    }

    public float getCardcurrCooldown(int idx)
    {
        return player2.cards[idx].currCooldown;
    }

    public string getCardFunction(int idx)
    {
        return player2.cards[idx].Function;
    }

    public string getCardImage(int idx)
    {
        return player2.cards[idx].Image;
    }

    public void CreateCharJSON()
    {
        string playerToJsonData = playObject.GetComponent<ControlJSON>().ObjectToJson(player);      // player 클래스 내부 변수와 값들을 string으로 변환
        playObject.GetComponent<ControlJSON>().CreateJsonFile(Application.dataPath, "cardtest", playerToJsonData); // data 폴더에 json 생성
        //Debug.Log("Create Player Json File.");
    }

    public void BringCharJSON()
    {
        string data = playObject.GetComponent<ControlJSON>().LoadJsonFile<CardDataClass>(Application.dataPath, "cardtest");
        //Debug.Log(data);
        player2 = JsonUtility.FromJson<CardDataClass>(data);
        //player = playObject.GetComponent<ControlJSON>().JsonToObjectOverwrite<CharClass_Playable>(data, player);
        //Debug.Log("Bring data from Player Json File.");
    }

    public void OverwriteCharJSON()
    {
        string playerToJsonData = playObject.GetComponent<ControlJSON>().ObjectToJson(player2);
        playObject.GetComponent<ControlJSON>().OverWriteJsonFile(Application.dataPath, "cardtest", playerToJsonData);
        //Debug.Log("Overwrite Player Json File.");
    }
}
