using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 로딩 신 안 쓸 경우
public class NewGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))    // 로딩 신 테스트 전용
        //    LoadingSceneManager.LoadScene("IngameScene");
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))    // 로딩 신 테스트 전용
        {
            LoadingSceneManager.LoadScene("MapScene"); //로딩 신 있는 버전
            //SceneManager.LoadScene("MapScene");             //로딩 신 없는 버전
        }

    }
}
