using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damage// : MonoBehaviour
{
    public static float SetDamage(float constantDmg, bool isPlayer)
    {
        float sum = constantDmg;    // 받아온 상수 대미지를 저장
        // 현재는 피해량 증가 버프가 존재하지 않으므로 사용하지 않지만 추후 기능을 위해 구현함.
        int burfCnt = GameObject.Find("CardManager").GetComponent<CardFunction>().GetBurfEntityCount();
        for(int i = 0; i < burfCnt; i++)
        {
            if(GameObject.Find("CardManager").GetComponent<CardFunction>().GetBurfEntity(burfCnt) == isPlayer)  // 만약 공격/치유 객체가 버프를 소지하고 있을 때
            {
                switch(GameObject.Find("CardManager").GetComponent<CardFunction>().GetBurfList(burfCnt))    // 버프 종류에 따라 피해량 조정
                {
                    case "더미용 피해량 10% 증가":
                        if(sum < 0) sum *= 1.1f;
                        break;
                    case "더미용 모든 치유량 10% 증가":   // 체력과 마나를 포함
                        if (sum > 0) sum *= 1.1f;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (GameObject.Find("CardManager").GetComponent<CardFunction>().GetBurfList(burfCnt))    // 피해량 감소와 같은 방어 버프, 치유 감소와 같은 디버프가 존재할 경우 추가
                {
                    case "더미용 피해량 10% 감소":
                        if (sum < 0) sum /= 1.1f;
                        break;
                    default:
                        break;
                }
            }
        }

        // 만약 sum이 음수 값이라면 절댓값으로 변경
        // 음수의 경우를 만든 이유는 치유량을 음수, 피해량을 양수로 표현해서 버프를 구별하기 위함
        if (sum < 0) sum *= -1; 

        return sum;
    }
}
