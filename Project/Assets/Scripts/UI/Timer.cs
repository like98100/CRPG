using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    //public Text[] timeText;
    [SerializeField] private TMP_Text MinuteTMP;
    [SerializeField] private TMP_Text SecondTMP;
    //private TextMeshProUGUI minute, second;
    //public GameObject Minute, Second;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        //minute = Minute.GetComponent<TextMeshProUGUI>();
        //second = Second.GetComponent<TextMeshProUGUI>();
        //Debug.Log(minute.text);
        //Debug.Log(second.text);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //timeText[0].text = ((int)time / 60 % 60).ToString();
        //timeText[1].text = ((int)time % 60).ToString();
        //minute.text = ((int)time / 60 % 60).ToString();
        //second.text = ((int)time % 60).ToString();
        MinuteTMP.text = ((int)time / 60 % 60).ToString();
        SecondTMP.text = ((int)time % 60).ToString();
    }

    public float getTime()
    {
        return time;
    }
}
