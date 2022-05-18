using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HPText : MonoBehaviour
{
    [SerializeField] private TMP_Text HpText;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerAct playerData;

    private float Hp;

    // Start is called before the first frame update
    void Start()
    {
        Hp = 0.0f;
        player = GameObject.Find("Player");
        playerData = player.GetComponent<PlayerAct>();
    }

    // Update is called once per frame
    void Update()
    {
        Hp = playerData.player.Hp;

        HpText.text = ((int)Hp).ToString();
    }
}
