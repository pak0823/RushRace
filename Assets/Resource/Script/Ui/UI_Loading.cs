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
    [HideInInspector] public string nextSceneName; // ���� �� �̸�

    private AsyncOperation asyncOperation;
    private float targetProgress;   //��ǥ �����
    private float loadingDuration = 1.0f;   //���� �ð����� �ε� �� ä���

    void Start()
    {
        StartCoroutine(WaitForNextSceneNameAndLoad());
    }

    IEnumerator WaitForNextSceneNameAndLoad()
    {
        // nextSceneName�� ������ ������ ��ٸ�
        while (string.IsNullOrEmpty(nextSceneName))
        {
            yield return null;
        }

        // nextSceneName�� �����Ǹ� ���� �ε�
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);
        asyncOperation.allowSceneActivation = false;
        targetProgress = 0f; // �ʱ�ȭ

        float startTime = Time.time; // ���� �ð� ���

        tipText.text = string.Format("{0}", randomTip());

        while (!asyncOperation.isDone)
        {
            // ���� �ε� ���� ��Ȳ (0 ~ 0.9)
            float realProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            // ����� ��ǥ ����� ���
            float elapsedTime = Time.time - startTime;
            targetProgress = Mathf.Clamp01(elapsedTime / loadingDuration);

            // progressBar.value�� ��ǥ ������� �ε巴�� �̵�
            progressBar.value = Mathf.Lerp(progressBar.value, targetProgress, Time.deltaTime * 5f); // Lerp �ӵ� ���� ����

            // �ؽ�Ʈ ������Ʈ
            loadingText.text = string.Format("{0:0}%", progressBar.value * 100);

            // ���� �ε��� �Ϸ�ǰ�, ��ǥ ������� 100%�� �����ϸ� �� Ȱ��ȭ
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

    string randomTip()  //���� �� �ؽ�Ʈ ����
    {
        string tip;
        int randNum = Random.Range(0, 5);

        switch(randNum)
        {
            case 0:
                tip = "-���ֿ����� ���� �ؼ��� �ȵ˴ϴ�.-";
                break;
            case 1:
                tip = "-���Ӱ� ������ �ٸ��ϴ�.-";
                break;
            case 2:
                tip = "-���� ���� �����Ͽ� �� ���� �ӵ����� ��⼼��.-";
                break;
            case 3:
                tip = "-������ �Ͽ� �Ƿ��� ������ ������.-";
                break;
            case 4:
                tip = "-�̽��Ϳ��װ� ������ �ִ� ���� �ֽ��ϴ�.-";
                break;
            default:
                tip = "-���׶� �� ����.-";
                break;
        }

        return tip;
    }
}
