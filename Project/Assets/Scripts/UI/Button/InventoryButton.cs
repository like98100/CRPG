using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    static bool isActivate;

    public static bool GetIsActivate()
    {
        return isActivate;
    }

    public static void SetIsActivate(bool idx)
    {
        isActivate = idx;
    }

    // Start is called before the first frame update
    void Start()
    {
        isActivate = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void isActivateInventory()
    {
        if (!GetIsActivate() && !MenualButton.GetIsActivate()) SetIsActivate(true);
    }
}
