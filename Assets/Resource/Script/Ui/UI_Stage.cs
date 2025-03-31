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
        
        // ĸ�� �̸��� �������� �̸����� ����
        currentStageName = GetStageName(clickedObjectName);

        if (currentStageName == "error")
        {
            Debug.LogError("�߸��� �������� ������Ʈ �̸��Դϴ�.");
            return;
        }

        // �ؽ�Ʈ ���
        stageText.text = $"{currentStageName}�� �����Ͻðڽ��ϱ�?";
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