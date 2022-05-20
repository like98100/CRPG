using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damage// : MonoBehaviour
{
    public static float SetDamage(float constantDmg, bool isPlayer)
    {
        float sum = constantDmg;    // �޾ƿ� ��� ������� ����
        // ����� ���ط� ���� ������ �������� �����Ƿ� ������� ������ ���� ����� ���� ������.
        int burfCnt = GameObject.Find("CardManager").GetComponent<CardFunction>().GetBurfEntityCount();
        for(int i = 0; i < burfCnt; i++)
        {
            if(GameObject.Find("CardManager").GetComponent<CardFunction>().GetBurfEntity(burfCnt) == isPlayer)  // ���� ����/ġ�� ��ü�� ������ �����ϰ� ���� ��
            {
                switch(GameObject.Find("CardManager").GetComponent<CardFunction>().GetBurfList(burfCnt))    // ���� ������ ���� ���ط� ����
                {
                    case "���̿� ���ط� 10% ����":
                        if(sum < 0) sum *= 1.1f;
                        break;
                    case "���̿� ��� ġ���� 10% ����":   // ü�°� ������ ����
                        if (sum > 0) sum *= 1.1f;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (GameObject.Find("CardManager").GetComponent<CardFunction>().GetBurfList(burfCnt))    // ���ط� ���ҿ� ���� ��� ����, ġ�� ���ҿ� ���� ������� ������ ��� �߰�
                {
                    case "���̿� ���ط� 10% ����":
                        if (sum < 0) sum /= 1.1f;
                        break;
                    default:
                        break;
                }
            }
        }

        // ���� sum�� ���� ���̶�� �������� ����
        // ������ ��츦 ���� ������ ġ������ ����, ���ط��� ����� ǥ���ؼ� ������ �����ϱ� ����
        if (sum < 0) sum *= -1; 

        return sum;
    }
}
