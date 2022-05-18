using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInventory : MonoBehaviour
{
    public RectTransform invenTransform;

    // 우측 ui는 비활성화/활성화
    // 좌측 ui는 활성화/비활성화 로 설정해 주시기 바랍니다.
    public float[] invenX;
    public float invenY;
    public float speed;

    public bool isLeft;

    // Start is called before the first frame update
    void Start()
    {
        invenTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isLeft)
        {
            if (InventoryButton.GetIsActivate())
            {
                if (invenTransform.anchoredPosition.x < invenX[0])
                {
                    invenTransform.transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
            }
            else
            {
                if (invenTransform.anchoredPosition.x > invenX[1])
                {
                    invenTransform.transform.Translate(Vector2.left * speed * Time.deltaTime);
                }
            }
        }
        else
        {
            if (!InventoryButton.GetIsActivate())
            {
                if (invenTransform.anchoredPosition.x < invenX[0])
                {
                    invenTransform.transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
            }
            else
            {
                if (invenTransform.anchoredPosition.x > invenX[1])
                {
                    invenTransform.transform.Translate(Vector2.left * speed * Time.deltaTime);
                }
            }
        }

    }
}
