// StageData.cs - ���õ� �������� ������ �������� ����/����

using UnityEngine;

public class StageData : MonoBehaviour
{
    public static int CurrentStageNum { get; private set; } = 1;

    private void Awake()
    {
        if (Shared.StageData == null)
        {
            Shared.StageData = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetStage(int stageNum)
    {
        CurrentStageNum = stageNum;
        Debug.Log($"�������� ������: {stageNum}");
    }

    public int GetStage()
    {
        return CurrentStageNum;
    }
}
