using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Windows;

public class TutorialBubble : MonoBehaviour
{
    [SerializeField] TMP_Text PanelTMP;

    [SerializeField] public GameObject bubbleObject, playerObject;
    PlayerAct player;
    [SerializeField] SpeechBubble speechBubble;
    Sequence sequence;
    public int tutoFlag;
    WaitForSeconds delay15 = new WaitForSeconds(1.5f);
    // Start is called before the first frame update
    public void Show(string message, int flag)
    {
        switch(flag)
        {
            case 1:
                {
                    if (bubbleObject.transform.localScale == Vector3.zero)
                    {
                        bubbleObject.transform.localPosition = new Vector3(-595.0f, -295.0f, 0);
                        Rotate180();
                        PanelTMP.text = message;
                        sequence = DOTween.Sequence()
                            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad));       // 1.0 ������ 
                            //.AppendInterval(1.5f)                                                       // 1.5�� ���
                            //.Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));     // 0.0 ������
                    }
                    else
                    {
                        sequence = DOTween.Sequence()
                            .AppendInterval(1.5f)
                            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));     // 0.0 ������
                        PanelTMP.text = message;
                    }
                    tutoFlag = flag;
                    break;
                }
            case 2:
                {
                    PanelTMP.text = message;

                    if(speechBubble.transform.localScale == Vector3.zero) speechBubble.ShowTutorial(message);
                    

                    tutoFlag = flag;
                    break;
                }
            case 4:     //case 3�� speech bubble ȣ���̹Ƿ� ������
                {
                    bubbleObject.transform.localPosition = new Vector3(-820.0f, -295.0f, 0);
                    Rotate180();
                    PanelTMP.text = message;

                    sequence = DOTween.Sequence()
                            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad));
                    //.AppendInterval(1.5f)
                    //.Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
                    if(tutoFlag == 3) Invoke("SetFlag", 2.0f);
                    //tutoFlag = flag;
                    break;
                }
            case 5:
                {
                    PanelTMP.text = message;

                    //if (speechBubble.transform.localScale == Vector3.zero)
                    //    sequence = DOTween.Sequence()
                    //            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad))
                    //            .AppendInterval(1.5f)
                    //            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
                    if (tutoFlag == 4) Invoke("SetFlag", 2.0f);
                    //tutoFlag = flag;
                    break;
                }
            case 6:
                {
                    PanelTMP.text = message;

                    if (speechBubble.transform.localScale == Vector3.zero)
                        sequence = DOTween.Sequence()
                                .AppendInterval(1.5f)
                                .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
                    tutoFlag = flag;
                    break;
                }
            case 7:
                {
                    if (speechBubble.transform.localScale == Vector3.zero) speechBubble.Show(message);

                    break;
                }
            default:
                { 
                Debug.Log("���� �÷����Դϴ�.");
                break;
                }
        }
       
    }
    // Start is called before the first frame update
    void Start()
    {
        ScaleZero();
        bubbleObject = this.gameObject;
        speechBubble = GameObject.Find("SpeechBubble").GetComponent<SpeechBubble>();
        playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<PlayerAct>();
    }

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if ((tutoFlag == 1 && PanelTMP.text == "�� �ϼ̽��ϴ�." && bubbleObject.transform.localScale == Vector3.zero))
        {
            Show("���콺 ��Ŭ������" + System.Environment.NewLine + "�⺻ ������ �� �� �ֽ��ϴ�.", 2);
        }
        
        if(tutoFlag == 3 && bubbleObject.transform.localScale == Vector3.zero)
        {
            Show("�̰��� ü�°� �����Դϴ�.", 4);
        }
        if(tutoFlag == 4)
        {
            Show("ü���� 0�� �Ǹ�" + System.Environment.NewLine + " ����մϴ�.", 5);
        }
        if (tutoFlag == 5)
        {
            Show("ī�带 ����ϱ� ���ؼ�" + System.Environment.NewLine + " ������ �ʿ��մϴ�.", 6);
        }
        if(tutoFlag == 6 && bubbleObject.transform.localScale == Vector3.zero)
        {
            Show("���� ���� �� �� �غ��ô�.", 7);
        }

    }

    void Rotate180()
    {
        bubbleObject.transform.localRotation = Quaternion.Euler(0, 180.0f, 0);
        PanelTMP.transform.localRotation = Quaternion.Euler(0, 180.0f, 0);
    }

    void Rotate0()
    {
        bubbleObject.transform.localRotation = Quaternion.Euler(0, 0.0f, 0);
        PanelTMP.transform.localRotation = Quaternion.Euler(0, 0.0f, 0);
    }

    void SetFlag()
    {
        if (tutoFlag == 3) tutoFlag = 4;
        else if (tutoFlag == 4) tutoFlag = 5;
    }
}
