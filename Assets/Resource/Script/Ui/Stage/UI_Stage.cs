// UI_Stage.cs

using UnityEngine;
using UnityEngine.UI;

public class UI_Stage : UIBase
{
    public Text stageText;

    private int currentStageNum;
    private string currentStageName;

    // �� �̹����� ��Ÿ���� ������Ʈ ���� �߰�
    public GameObject stage2Rock;
    public GameObject stage3Rock;

    public override void Start()
    {
        PlayBGM();
        Shared.UI_Stage = this;
        OPTIONSHOW.SetActive(false);

        UpdateStageLockUI();
    }

    // ���������� ��� ���¿� ���� �� �̹��� ��Ȱ��ȭ ó��
    public void UpdateStageLockUI()
    {
        // ����: StageData�� �ε��� 1�� 2Stage, �ε��� 2�� 3Stage��� ����
        if (stage2Rock != null)
        {
            // ���� 2Stage�� �����Ǿ��ٸ� �� �̹��� ��Ȱ��ȭ
            stage2Rock.SetActive(Shared.StageData.IsStageUnlocked(1));
            // ����, ������ �����̸� SetActive(true)�� �ƴ϶� false�� �����ؾ� �� �̹����� �����
            // ���� ������ "�����Ǿ����� false, ��������� true"�� �ۼ�:
            stage2Rock.SetActive(!Shared.StageData.IsStageUnlocked(1));
        }

        if (stage3Rock != null)
        {
            stage3Rock.SetActive(!Shared.StageData.IsStageUnlocked(2));
        }
    }

    public void OnStageIconClicked(int stageNum)
    {
        // �������� ���� ���� ��� ���θ� üũ�մϴ�.
        if (!Shared.StageData.IsStageUnlocked(stageNum))
        {
            Debug.LogError($"�������� {stageNum}�� ���� ��� �ֽ��ϴ�.");
            return;
        }

        if (!isOptionShow)
        {
            currentStageNum = stageNum;
            currentStageName = GetStageName(stageNum);

            if (currentStageName == "error")
            {
                Debug.LogError("�߸��� �������� ��ȣ�Դϴ�.");
                return;
            }

            Shared.StageData.SetStage(currentStageNum);
            stageText.text = $"{currentStageName}�� �����Ͻðڽ��ϱ�?";
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
