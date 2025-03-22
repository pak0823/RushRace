using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Loading : MonoBehaviour
{
    public Slider progressBar;
    public Text loadingText;
    public Text tipText;
    [HideInInspector] public string nextSceneName; // 다음 씬 이름

    private AsyncOperation asyncOperation;
    private float targetProgress;   //목표 진행률
    private float loadingDuration = 3.0f;   //2.5초동안 로딩 바 채우기

    void Start()
    {
        StartCoroutine(WaitForNextSceneNameAndLoad());
    }

    IEnumerator WaitForNextSceneNameAndLoad()
    {
        // nextSceneName이 설정될 때까지 기다림
        while (string.IsNullOrEmpty(nextSceneName))
        {
            yield return null;
        }

        // nextSceneName이 설정되면 씬을 로드
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);
        asyncOperation.allowSceneActivation = false;
        targetProgress = 0f; // 초기화

        float startTime = Time.time; // 시작 시간 기록

        tipText.text = string.Format("{0}", randomTip());

        while (!asyncOperation.isDone)
        {
            // 실제 로딩 진행 상황 (0 ~ 0.9)
            float realProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            // 진행될 목표 진행률 계산
            float elapsedTime = Time.time - startTime;
            targetProgress = Mathf.Clamp01(elapsedTime / loadingDuration);

            // progressBar.value를 목표 진행률로 부드럽게 이동
            progressBar.value = Mathf.Lerp(progressBar.value, targetProgress, Time.deltaTime * 5f); // Lerp 속도 조절 가능

            // 텍스트 업데이트
            loadingText.text = string.Format("{0:0}%", progressBar.value * 100);

            // 실제 로딩이 완료되고, 목표 진행률도 100%에 도달하면 씬 활성화
            if (realProgress >= 1f && !asyncOperation.allowSceneActivation)
            {
                if (targetProgress >= 1f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    string randomTip()  //랜덤 팁 텍스트 생성
    {
        string tip;
        int randNum = Random.Range(0, 5);

        switch(randNum)
        {
            case 0:
                tip = "randNum = 0";
                break;
            case 1:
                tip = "randNum = 1";
                break;
            case 2:
                tip = "randNum = 2";
                break;
            case 3:
                tip = "randNum = 3";
                break;
            case 4:
                tip = "randNum = 4";
                break;
            default:
                tip = "버그라 팁 없음";
                break;
        }

        return tip;
    }
}
