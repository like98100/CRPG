using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    static bool isVictory;
    public static bool IsVictory { get { return isVictory; } set { isVictory = value; } }
    void Awake() => Inst = this;

    [Header("Develop")]
    [SerializeField] [Tooltip("시작 카드 개수")] int startCardCount;

    //[Header("Properties")]
    //public bool isLoading;      // 로딩 턴 true일 때 카드 클릭 방지(현재 사용 안함)

    WaitForSeconds delay5 = new WaitForSeconds(5.0f);
    WaitForSeconds delay4 = new WaitForSeconds(4.0f);
    WaitForSeconds delayRandom = new WaitForSeconds(3.0f);
    WaitForSeconds delay2 = new WaitForSeconds(2.0f);
    WaitForSeconds delay05 = new WaitForSeconds(0.05f);
    WaitForSeconds delay01 = new WaitForSeconds(1.0f);

    public static Action<bool> OnAddCard;

    [SerializeField] Panel panel;
    //[SerializeField] TutorialBubble turorialBubble;
    [SerializeField] Timer time;
    [SerializeField] CardManager cardmanage;
    [SerializeField] CardFunction cardFunc;
    [SerializeField] EnemyAct enemy;
    public GameObject cardFuncObject, enemyObject;
    public int currTime;

    // Start is called before the first frame update
    void Start()
    {
        panel = GameObject.Find("StartPanel").GetComponent<Panel>();
        //turorialBubble = GameObject.Find("TutorialPanel").GetComponent<TutorialBubble>();
        cardmanage = GameObject.Find("CardManager").GetComponent<CardManager>();
        cardFuncObject = GameObject.Find("CardManager");

        cardFunc = cardFuncObject.GetComponent<CardFunction>();
        enemyObject = GameObject.FindWithTag("Enemy");
        enemy = enemyObject.GetComponent<EnemyAct>();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        // 폐기( 카드 복사됨)
      //currTime = (int)GameManager.Inst.time.getTime();

      //  if (currTime != 0 && currTime % 5 == 0)
      //  {
      //      Debug.Log("드로우");
      //      OnAddCard?.Invoke(false);
      //      OnAddCard?.Invoke(true);
      //  }
#if UNITY_EDITOR
        InputCheatKey();
        if (cardFunc.drawFlag != 0)
        {
            for (int i = 0; i < cardFunc.drawCnt; i++)
            {
                if (cardFunc.drawFlag == 1) OnAddCard?.Invoke(true);
                else OnAddCard?.Invoke(false);
            }

            cardFunc.drawFlag = 0;
            cardFunc.drawCnt = 0;
        }

#endif
    }

    public IEnumerator StartGameCo()        // 최초 게임 시작
    {
        GameManager.Inst.StartPanel("시작");

        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            OnAddCard?.Invoke(false);
            yield return delay05;
            OnAddCard?.Invoke(true);
        }

        //여기다 튜토리얼 패널을 붙일 것
        Debug.Log(SceneManager.GetActiveScene().name);

        //yield return delay2;
        //GameManager.Inst.ShowTutorialBubble("카드를 클릭하여" + System.Environment.NewLine + "스킬을 사용하십시오.", 1);
        yield return delay2;
        StartCoroutine(UpdateGameCo());
        while (true)
        {
                yield return delay5;
                StartCoroutine(UpdateGameCo());

        }

    }

    IEnumerator UpdateGameCo()  // 진행
    {
        yield return delay01;
        if(GameManager.Inst.cardmanage.getCardCount(false) < 9)OnAddCard?.Invoke(false);
        //yield return delay01;
        if (GameManager.Inst.cardmanage.getCardCount(true) < 9) OnAddCard?.Invoke(true);
    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) && GameManager.Inst.cardmanage.getCardCount(true) < 9)
        {
            OnAddCard?.Invoke(true);
            //Debug.Log(currTime);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2) && GameManager.Inst.cardmanage.getCardCount(false) < 9)
        {
            OnAddCard?.Invoke(false);
        }
    }

    public void StartGame()
    {
        StartCoroutine(Inst.StartGameCo());
    }

    public void StartPanel(string message)
    {
        panel.Show(message);
    }

    public void ShowTutorialBubble(string message, int flag)
    {
        //turorialBubble.Show(message, flag);
    }

    public void GoResultScene(bool isVictory)
    {
        //if (isVictory) LoadingSceneManager.LoadScene("ResultScene");
        //else LoadingSceneManager.LoadScene("TitleScene");
        LoadingSceneManager.LoadScene("ResultScene");
    }
}
