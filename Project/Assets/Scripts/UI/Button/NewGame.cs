using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // �ε� �� �� �� ���
public class NewGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))    // �ε� �� �׽�Ʈ ����
        //    LoadingSceneManager.LoadScene("IngameScene");
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))    // �ε� �� �׽�Ʈ ����
        {
            LoadingSceneManager.LoadScene("MapScene"); //�ε� �� �ִ� ����
            //SceneManager.LoadScene("MapScene");             //�ε� �� ���� ����
        }

    }
}
