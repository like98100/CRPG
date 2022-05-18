using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Panel : MonoBehaviour
{
    [SerializeField] TMP_Text PanelTMP;

    public void Show(string message)
    {
        PanelTMP.text = message;
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad))       // 1.0 스케일 
            .AppendInterval(1.5f)                                                       // 1.5초 대기
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));     // 0.0 스케일
    }
    // Start is called before the first frame update
    void Start() => ScaleZero();

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        
    }
}
