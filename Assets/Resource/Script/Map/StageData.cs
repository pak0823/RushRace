// StageData.cs - 선택된 스테이지 정보를 전역으로 저장/관리

using UnityEngine;

public class StageData : MonoBehaviour
{
    public static int CurrentStageNum { get; private set; } = 0;
    private static bool[] unlockedStages = new bool[3] { true, false, false };
    private void Awake()
    {
        if (Shared.StageData == null)
        {
            Shared.StageData = this;
            DontDestroyOnLoad(gameObject);
            LoadStageUnlockData(); // PlayerPrefs 등에서 불러오기
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 다음 스테이지 해제
    public void UnlockStage(int stageNum)
    {
        if (stageNum >= 0 && stageNum < unlockedStages.Length)
        {
            unlockedStages[stageNum] = true;
            SaveStageUnlockData();
            Debug.Log($"스테이지 {stageNum+1} 잠금 해제됨!");
        }
    }

    public bool IsStageUnlocked(int stageNum)
    {
        if (stageNum < 0 || stageNum >= unlockedStages.Length)
            return false;
        return unlockedStages[stageNum];
    }

    private void SaveStageUnlockData()
    {
        for (int i = 0; i < unlockedStages.Length; i++)
        {
            PlayerPrefs.SetInt("StageUnlocked_" + i, unlockedStages[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadStageUnlockData()
    {
        for (int i = 0; i < unlockedStages.Length; i++)
        {
            int unlockedVal = PlayerPrefs.GetInt("StageUnlocked_" + i, (i == 0) ? 1 : 0);
            // 첫 번째 스테이지는 기본 해제(1), 나머지는 0
            unlockedStages[i] = (unlockedVal == 1);
        }
    }

    public void SetStage(int stageNum)
    {
        CurrentStageNum = stageNum;
    }

    public int GetStage()
    {
        return CurrentStageNum;
    }
}
