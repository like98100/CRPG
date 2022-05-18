//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Text;

//[System.Serializable]
//public class CharClass : MonoBehaviour
//{
//    public string CharTypeName;
//    public string CharName;
//    public float Hp;
//    public float Mp;
//    public string Weapon;
//    public string Equipment;
//    public List<string> CharDeckList = new List<string>();

//    public float playerPosX_f, playerPosY_f;
//    public int playerPosX_i, playerPosY_i;

//    public CharClass()
//    {
//        CharTypeName = string.Empty;
//        CharName = string.Empty;
//        Hp = -1.1f;
//        Mp = -1.1f;
//        Weapon = "none";
//        Equipment = "none";
//        for (int i = 0; i < 9; i++)
//        {
//            int sum = i - 10;
//            CharDeckList.Add(sum.ToString());
//        }
//        playerPosX_f = 0.0f; playerPosY_f = 0.0f;
//        playerPosX_i = 0; playerPosY_i = 0;
//    }

//    public virtual void Log()
//    {
//        Debug.Log("CharTypeName : " + CharTypeName);
//        Debug.Log("Charname : " + CharName);
//        Debug.Log("Hp : " + Hp);
//        Debug.Log("Mp : " + Mp);
//        Debug.Log("Weapon : " + Weapon);
//        Debug.Log("Equipment : " + Equipment);
//        Debug.Log("PositionF : " + playerPosX_f + ", " + playerPosY_f);
//        Debug.Log("PositionINT : " + playerPosX_i + ", " + playerPosY_i);

//        //foreach (string Deck in CharDeckList) Debug.Log("Deck : " + Deck);
//        for (int i = 0; i < CharDeckList.Count; i++) Debug.Log("Deck : " + CharDeckList[i]);
//    }
//}

//[System.Serializable]
//public class CharClass_Playable : CharClass
//{
//    public float nAttackDmg;
//    public float nAttackSpd;

//    public CharClass_Playable()
//    {
//        CharTypeName = typeof(CharClass_Playable).ToString();
//        nAttackDmg = 0.1f;
//        nAttackSpd = 0.1f;
//    }

//    public override void Log()
//    {
//        base.Log();
//        Debug.Log("nAttackDmg : " + nAttackDmg);
//        Debug.Log("nAttackSpd : " + nAttackSpd);
//    }
//}

//[System.Serializable]
//public class CharClass_Monster : CharClass
//{
//    public override void Log()
//    {
//        base.Log();
//    }
//}
