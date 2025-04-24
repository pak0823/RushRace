// UI_Lobby.cs - 로비 UI 및 버튼 이벤트 처리
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class UI_Lobby : UIBase
{
    public GameObject NotificationWindow;
    private float notificationFadeDuration = 2f;
    private bool isnoti = false;

    CanvasGroup notifCG;
    public override void Start()
    {
        Shared.SoundManager.StopLoopSound();
        OPTIONSHOW.SetActive(false);
        PlayBGM();

        notifCG = NotificationWindow.GetComponent<CanvasGroup>();
        if (notifCG == null)
            notifCG = NotificationWindow.AddComponent<CanvasGroup>();
        notifCG.alpha = 0f;
        NotificationWindow.SetActive(false);
    }

    public void OnBtnGoToIngame()
    {
        ClickSound();
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_STAGE);
    }

    public void OnBtnGoToPractice()
    {
        ClickSound();
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_PRACTICE);
    }

    public void OnBtnGoToShop()
    {
        ClickSound();
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_SHOP);
    }

    public void OnBtnGoToRepair()
    {
        if (isnoti)
            return;

        ClickSound();
        StartCoroutine(FadeNotification());
    }

    IEnumerator FadeNotification()
    {
        NotificationWindow.SetActive(true);
        isnoti = true;

        float half = notificationFadeDuration * 0.5f;
        float t = 0f;

        // Fade In (0 → 1)
        while (t < half)
        {
            notifCG.alpha = t / half;
            t += Time.deltaTime;
            yield return null;
        }
        notifCG.alpha = 1f;

        // Fade Out (1 → 0)
        t = 0f;
        while (t < half)
        {
            notifCG.alpha = 1f - (t / half);
            t += Time.deltaTime;
            yield return null;
        }
        notifCG.alpha = 0f;
        NotificationWindow.SetActive(false);
        isnoti = false;
    }


}
