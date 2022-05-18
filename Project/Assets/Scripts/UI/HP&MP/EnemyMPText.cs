using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyMPText : MonoBehaviour
{
    [SerializeField] private TMP_Text MpText;
    [SerializeField] private GameObject enemy;
    [SerializeField] private EnemyAct enemyData;

    private float Mp;

    // Start is called before the first frame update
    void Start()
    {
        Mp = 0.0f;
        enemy = GameObject.Find("Enemy");
        enemyData = enemy.GetComponent<EnemyAct>();
    }

    // Update is called once per frame
    void Update()
    {
        Mp = enemyData.monster.mons[enemyData.GetEnemyNum()].Mp;

        MpText.text = ((int)Mp).ToString();
    }
}
