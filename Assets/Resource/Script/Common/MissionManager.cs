// MissionManager.cs - �̼� ���� ó�� (���� �ð� �� ���� ����)

using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("�̼� ����")]
    public int targetCoinCount = 10;
    public float missionTime = 60f;

    private float remainingTime;
    private int currentCoinCount;
    private bool isMissionActive = false;
    private bool isMissionEnded = false;

    private void Start()
    {
        Shared.MissionManager = this;
        StartMission();
    }

    private void Update()
    {
        if (!isMissionActive || isMissionEnded) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            EndMission(false);
        }
    }

    public void StartMission()
    {
        currentCoinCount = 0;
        remainingTime = missionTime;
        isMissionActive = true;
        isMissionEnded = false;
    }

    public void OnCoinCollected()
    {
        if (!isMissionActive || isMissionEnded) return;

        currentCoinCount++;

        if (currentCoinCount >= targetCoinCount)
        {
            EndMission(true);
        }
    }

    private void EndMission(bool success)
    {
        isMissionActive = false;
        isMissionEnded = true;

        if (success)
        {
            Shared.GameManager.AddMoney(currentCoinCount);
            Debug.Log("�̼� ����!");

            int currentStage = StageData.CurrentStageNum;
            UnlockNextStage(currentStage);  // ���� �Լ��� ������
        }
        else
        {
            Debug.Log("�̼� ����.");
        }
    }

    private void UnlockNextStage(int clearedStage)
    {
        // ��) 1Stage Ŭ����� 2Stage ����
        //     2Stage Ŭ����� 3Stage ����
        //     ...
        int nextStage = clearedStage + 1;
        Shared.StageData.UnlockStage(nextStage);
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public string GetMissionProgress()
    {
        return $"���� {currentCoinCount}/{targetCoinCount}";
    }
}
