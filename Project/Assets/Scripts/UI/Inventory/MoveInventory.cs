using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInventory : MonoBehaviour
{
    public RectTransform invenTransform;

    // ���� ui�� ��Ȱ��ȭ/Ȱ��ȭ
    // ���� ui�� Ȱ��ȭ/��Ȱ��ȭ �� ������ �ֽñ� �ٶ��ϴ�.
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
