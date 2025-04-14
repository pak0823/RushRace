// UI_Stage.cs

using UnityEngine;
using TMPro;

public class UI_Stage : UIBase
{
    public GameObject checkWindow;
    public TextMeshProUGUI stageText;

    private int currentStageNum;
    private string currentStageName;

    public void OnStageIconClicked(int stageNum)
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
        checkWindow.SetActive(true);
    }

    public void OnBtnYes() => ChangeScene(eSCENE.eSCENE_INGAME);
    public void OnBtnNo() => checkWindow.SetActive(false);
    public void OnBtnBack() { ClickSound(); ChangeScene(eSCENE.eSCENE_LOBBY); }

    private string GetStageName(int stageNum)
    {
        switch (stageNum)
        {
            case 1: return "1Stage";
            case 2: return "2Stage";
            case 3: return "3Stage";
            default: return "error";
        }
    }
}
