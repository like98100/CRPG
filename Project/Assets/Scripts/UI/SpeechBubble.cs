using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] public TMP_Text PanelTMP;

    [SerializeField] GameObject playerObject, bubbleObject;
    [SerializeField] TutorialBubble tutorialBubble;
    PlayerAct player;

    // Start is called before the first frame update
    void Start()
    {
        ScaleZero();
        playerObject = GameObject.Find("Player");
        bubbleObject = this.gameObject;
        player = playerObject.GetComponent<PlayerAct>();
        tutorialBubble = GameObject.Find("TutorialPanel").GetComponent<TutorialBubble>();
        PanelTMP.fontSize = 2.75f;
    }

    // Update is called once per frame
    void Update()
    {
            if (player.transform.localScale.x == 1)
            {
                bubbleObject.transform.localPosition = new Vector3(player.player.playerPosX_f - 1.25f, player.player.playerPosY_f + 1.25f, 0);
                bubbleObject.transform.localRotation = Quaternion.Euler(0, 180.0f, 0);
                PanelTMP.transform.localRotation = Quaternion.Euler(0, 180.0f, 0);
        }
            else
            {
                bubbleObject.transform.localPosition = new Vector3(player.player.playerPosX_f + 1.25f, player.player.playerPosY_f + 1.25f, 0);
                bubbleObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            PanelTMP.transform.localRotation = Quaternion.Euler(0, 0f, 0);
        }
    }

    public void Show(string message)
    {
        PanelTMP.fontSize = 2.75f;
        PanelTMP.text = message;
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad))       // 1.0 스케일 
            .AppendInterval(1.5f)                                                       // 1.5초 대기
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));     // 0.0 스케일

        if (tutorialBubble.tutoFlag == 6) tutorialBubble.tutoFlag = 7;
    }

    public void ShowTutorial(string message)
    {
        PanelTMP.fontSize = 4.0f;
        PanelTMP.text = message;
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad));       // 1.0 스케일 
    }

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;
}
