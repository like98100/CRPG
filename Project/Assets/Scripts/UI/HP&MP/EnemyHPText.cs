using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHPText : MonoBehaviour
{
    [SerializeField] private TMP_Text HpText;
    [SerializeField] private GameObject enemy;
    [SerializeField] private EnemyAct enemyData;

    private float Hp;

    // Start is called before the first frame update
    void Start()
    {
        Hp = 0.0f;
        enemy = GameObject.Find("Enemy");
        enemyData = enemy.GetComponent<EnemyAct>();
    }

    // Update is called once per frame
    void Update()
    {
        Hp = enemyData.monster.mons[enemyData.GetEnemyNum()].Hp;

        HpText.text = ((int)Hp).ToString();
    }
}
