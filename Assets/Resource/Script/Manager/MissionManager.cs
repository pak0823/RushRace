using System;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("���������� ���� ��ǥġ")]
    private int[] stageTargetCoinCounts = { 10, 75, 100 };
    [Header("���������� ���� �ð�(��)")]
    private float[] stageMissionTimes = { 120f, 150f, 180f };

    private int targetCoinCount;          // ��Ÿ�ӿ� �� ���� ���
    private float missionTime;            // ��Ÿ�ӿ� �� ���� ���

    private float remainingTime;
    private int currentCoinCount;
    private bool isMissionActive = false;
    private bool isMissionEnded = false;

    public GameObject Panel;

    public bool IsMissionActive => isMissionActive;

    private void Start()
    {
        Shared.MissionManager = this;

        // 1) ���� �������� ��ȣ ��������
        int stage = StageData.CurrentStageNum;

        // 2) �迭 ���� üũ �� ��ǥġ/�ð� �Ҵ�
        if (stage >= 0 && stage < stageTargetCoinCounts.Length)
            targetCoinCount = stageTargetCoinCounts[stage];
        else
            Debug.LogWarning($"Stage {stage} �� ���� targetCoinCount �̼���, �⺻ 0 ���.");

        if (stage >= 0 && stage < stageMissionTimes.Length)
            missionTime = stageMissionTimes[stage];
        else
            Debug.LogWarning($"Stage {stage} �� ���� missionTime �̼���, �⺻ 0 ���.");

        // 3) �̼� ����
        StartMission();
    }

    public void StartMission()
    {
        currentCoinCount = 0;
        remainingTime = missionTime;
        isMissionActive = true;
        isMissionEnded = false;
    }

    private void Update()
    {
        if (!isMissionActive || isMissionEnded) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            Panel.SetActive(true);
            EndMission(false);
        }
            
    }

    public void OnCoinCollected()
    {
        if (!isMissionActive || isMissionEnded) return;

        currentCoinCount++;
        if (currentCoinCount >= targetCoinCount)
            EndMission(true);
    }

    // �̼� ���� �� �˷��� �̺�Ʈ
    public static event Action<bool> OnMissionResult;

    private void EndMission(bool success)
    {
        isMissionActive = false;
        isMissionEnded = true;
        Shared.SoundManager.StopLoopSound();

        if (success)
        {
            Shared.GameManager.AddMoney(currentCoinCount);
            Debug.Log("�̼� ����!");
            UnlockNextStage(StageData.CurrentStageNum);
        }
        else
            Debug.Log("�̼� ����.");

        // UI���� ����/���� �˸�
        OnMissionResult?.Invoke(success);
    }
    private void UnlockNextStage(int clearedStage)
    {
        Shared.StageData.UnlockStage(clearedStage + 1);
    }

    // �ܺο��� ���� �ð�/���൵ ��ȸ��
    public float GetRemainingTime() => remainingTime;
    public string GetMissionProgress() => $"���� {currentCoinCount}/{targetCoinCount}";
}
