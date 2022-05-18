using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public RectTransform infoTransform;

    public float[] infoX;
    public float infoY;

    // Start is called before the first frame update
    void Start()
    {
        infoTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!InventoryButton.GetIsActivate())
        {
            if (infoTransform.anchoredPosition.x > infoX[0])
            {
                infoTransform.transform.Translate(Vector2.left * 3000.0f * Time.deltaTime);
            }
        }
        else
        {
            if (infoTransform.anchoredPosition.x < infoX[1])
            {
                infoTransform.transform.Translate(Vector2.right * 3000.0f * Time.deltaTime);
            }
        }
    }
}
