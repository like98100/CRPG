using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    [SerializeField] Renderer[] backRenderers;
    [SerializeField] Renderer[] middleRenderers;
    [SerializeField] string sortingLayerName;
    int originOrder;
    private void Start()
    {
        SetOrder(0);
    }

    public void SetOriginOrder(int originOrder) // 카드가 확장될 때 맨 앞에 오도록 설정
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }
    
    public void SetMostFrontOrder(bool isMostFront)
    {
        SetOrder(isMostFront ? 100  : originOrder); // 트루면 100, 펄스면 originOrder
    }

    public void SetOrder(int order)
    {
        int mulOrder = order * 10;  // 복수의 카드 생성 시 레이어 간격 벌림
        
        foreach(var renderer in backRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder;
        }

        foreach(var renderer in middleRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder + 1;
        }
    }
}
