using UnityEngine;
using UnityEngine.UI;

public class UI_Stage : UIBase
{
    public Text stageText;
    public GameObject checkWindow;

    private string currentStageName;
    public void OnBtnYes() => ChangeScene(eSCENE.eSCENE_INGAME);
    public void OnBtnNo() => Window.SetActive(false);
    public void OnBtnBack() => Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_LOBBY);

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
        string clickedObjectName = gameObject.name;
        
        // 캡슐 이름을 스테이지 이름으로 매핑
        currentStageName = GetStageName(clickedObjectName);

        if (currentStageName == "error")
        {
            Debug.LogError("잘못된 스테이지 오브젝트 이름입니다.");
            return;
        }

        // 텍스트 출력
        stageText.text = $"{currentStageName}를 선택하시겠습니까?";
        checkWindow.SetActive(true);
    }

    private string GetStageName(string objectName)
    {
        switch (objectName)
        {
            case "ForestCapsule": return "1Stage";
            case "DesertCapsule": return "2Stage";
            case "DowntownCapsule": return "3Stage";
            default: return "error";
        }
    }

    public string GetCurrentStage()
    {
        return currentStageName;
    }

    
}