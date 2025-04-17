// StageData.cs - ���õ� �������� ������ �������� ����/����

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
            LoadStageUnlockData(); // PlayerPrefs ��� �ҷ�����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���� �������� ����
    public void UnlockStage(int stageNum)
    {
        if (stageNum >= 0 && stageNum < unlockedStages.Length)
        {
            unlockedStages[stageNum] = true;
            SaveStageUnlockData();
            Debug.Log($"�������� {stageNum+1} ��� ������!");
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
            // ù ��° ���������� �⺻ ����(1), �������� 0
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
