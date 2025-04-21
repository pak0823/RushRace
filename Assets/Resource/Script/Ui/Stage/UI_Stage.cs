// UI_Stage.cs

using UnityEngine;
using UnityEngine.UI;

public class UI_Stage : UIBase
{
    public Text stageText;

    private int currentStageNum;
    private string currentStageName;

    // 락 이미지를 나타내는 오브젝트 참조 추가
    public GameObject stage2Rock;
    public GameObject stage3Rock;

    public override void Start()
    {
        PlayBGM();
        Shared.UI_Stage = this;
        OPTIONSHOW.SetActive(false);

        UpdateStageLockUI();
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
        // 스테이지 선택 전에 잠금 여부를 체크합니다.
        if (!Shared.StageData.IsStageUnlocked(stageNum))
        {
            Debug.LogError($"스테이지 {stageNum}는 아직 잠겨 있습니다.");
            return;
        }

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

    public void OnBtnYes() => Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_INGAME);
    public void OnBtnNo() => ToggleOptionWindow();
    public void OnBtnBack() { ClickSound(); Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_LOBBY); }
}
