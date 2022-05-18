using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene; // 다음에 불러올 신의 이름

    [SerializeField]
    Image progressBar;      //로딩 바

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());    // 로딩 신 호출 함수를 코루틴으로 실행
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;      // 다음 신 이름 저장
        SceneManager.LoadScene("LoadingScene"); // 로딩 신 호출
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); // 로딩 신 이후 나오는 신
        op.allowSceneActivation = false;    // 장면이 준비된 즉시 활성화(여기서 X하는 이유는 로딩이 끝나기 전까지 신을 유지하기 위해)

        float timer = 0f;
        while(!op.isDone)   // 신 로딩이 끝나지 않았을 때
        {
            yield return null;  // 제어권을 넘겨져 진행바가 차오르는 모습을 보여줌

            timer += Time.deltaTime;
            if (op.progress < 0.9f)  // 로딩된 수준이 90% 이하일 때
            {
                //progressBar.fillAmount = op.progress; // 진행도 만큼 채움

                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);    // timer 값에 맞춰서 fillamount부터 op.progress까지 채움
                if (progressBar.fillAmount >= op.progress)   // 로딩 수준보다 바가 더 채워졌을 때
                {
                    timer = 0f; // 타이머 초기화
                }
            }
            else // 로딩된 수준이 90% 이상일 때
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); // timer 값에 맞춰서 fillAmount부터 100%까지 채움
                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true; // 신 이동 허용
                    yield break;    // 반복문 탈출
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
