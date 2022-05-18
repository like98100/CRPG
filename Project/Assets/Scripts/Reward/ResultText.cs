using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResultText : MonoBehaviour
{
    [SerializeField] TMP_Text resultText;

    string result;
    // Start is called before the first frame update
    void Start()
    {
        result = "Stage ";
        result += SelectStage.GetStageNum().ToString();
        if (GameManager.IsVictory) result += " 성공";
        else result += " 실패";
        resultText.text = result;
    }

    // Update is called once per frame
    void Update()
    {



    }
}
