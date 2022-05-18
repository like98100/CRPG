using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
public class ClickUICard : MonoBehaviour
        , IPointerClickHandler      // Å¬¸¯
{
    int isClicked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isClicked = 0;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click" + eventData.pointerPress);
        if (eventData.pointerPress.tag == "PlayerCard") isClicked = 1;
        else if (eventData.pointerPress.tag == "InvenCard") isClicked = 2;
        else isClicked = 0;
    }

    public int GetEventObject()
    {
        return isClicked;
    }
}
