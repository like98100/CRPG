using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnMap : MonoBehaviour
{
    public void ReturnToMap()
    {
        LoadingSceneManager.LoadScene("MapScene");
    }
}
