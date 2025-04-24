// UI_Stage.cs

using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class UI_Stage : UIBase
{
    public Text stageText;
    private int currentStageNum;
    private string currentStageName;

    public GameObject NotificationWindow;
    private float notificationFadeDuration = 2.5f;
    public bool isnoti = false;
    public Text notiText;

    CanvasGroup notifCG;
    // 락 이미지를 나타내는 오브젝트 참조 추가
    public GameObject stage2Rock;
    public GameObject stage3Rock;

    public override void Start()
    {
        PlayBGM();
        Shared.UI_Stage = this;
        OPTIONSHOW.SetActive(false);

        UpdateStageLockUI();

        notifCG = NotificationWindow.GetComponent<CanvasGroup>();
        if (notifCG == null)
            notifCG = NotificationWindow.AddComponent<CanvasGroup>();
        notifCG.alpha = 0f;
        NotificationWindow.SetActive(false);
    }

    // 스테이지의 잠금 상태에 따라 락 이미지 비활성화 처리
    public void UpdateStageLockUI()
    {
        // 예시: StageData의 인덱스 1이 2Stage, 인덱스 2가 3Stage라고 가정
        if (stage2Rock != null)
        {
            // 만약 2Stage가 해제되었다면 락 이미지 비활성화
            stage2Rock.SetActive(Shared.StageData.IsStageUnlocked(1));
            // 만약, 해제된 상태이면 SetActive(true)가 아니라 false로 설정해야 락 이미지가 사라짐
            // 따라서 보통은 "해제되었으면 false, 잠겨있으면 true"로 작성:
            stage2Rock.SetActive(!Shared.StageData.IsStageUnlocked(1));
        }

        if (stage3Rock != null)
        {
            stage3Rock.SetActive(!Shared.StageData.IsStageUnlocked(2));
        }
    }

    public void OnStageIconClicked(int stageNum)
    {
        if (!isOptionShow)
        {
            currentStageNum = stageNum;
            currentStageName = GetStageName(stageNum);

            if (currentStageName == "error")
            {
                Debug.LogError("잘못된 스테이지 번호입니다.");
                return;
            }

            Shared.StageData.SetStage(currentStageNum);
            stageText.text = $"{currentStageName}를 선택하시겠습니까?";
            ToggleOptionWindow();
        }
    }

    private string GetStageName(int stageNum)
    {
        switch (stageNum)
        {
            case 0: return "1Stage";
            case 1: return "2Stage";
            case 2: return "3Stage";
            default: return "error";
        }
    }

    public IEnumerator FadeNotification()
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

    public void OnBtnYes() => Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_INGAME);
    public void OnBtnNo() => ToggleOptionWindow();
    public void OnBtnBack() { ClickSound(); Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_LOBBY); }
}
