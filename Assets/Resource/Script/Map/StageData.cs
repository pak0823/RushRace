// StageData.cs - 선택된 스테이지 정보를 전역으로 저장/관리

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
        Debug.Log($"스테이지 설정됨: {stageNum}");
    }

    public int GetStage()
    {
        return CurrentStageNum;
    }
}
