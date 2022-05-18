using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MPText : MonoBehaviour
{
    [SerializeField] private TMP_Text MpText;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerAct playerData;

    private float Mp;
    // Start is called before the first frame update
    void Start()
    {
        Mp = 0.0f;
        player = GameObject.Find("Player");
        playerData = player.GetComponent<PlayerAct>();
    }

    // Update is called once per frame
    void Update()
    {
        Mp = playerData.player.Mp;

        MpText.text = ((int)Mp).ToString();
    }
}
