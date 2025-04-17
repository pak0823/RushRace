using System;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("스테이지별 코인 목표치")]
    public int[] stageTargetCoinCounts = { 50, 75, 100 };
    [Header("스테이지별 제한 시간(초)")]
    public float[] stageMissionTimes = { 90f, 120f, 150f };

    private int targetCoinCount;          // 런타임에 이 값을 사용
    private float missionTime;            // 런타임에 이 값을 사용

    private float remainingTime;
    private int currentCoinCount;
    private bool isMissionActive = false;
    private bool isMissionEnded = false;

    public GameObject Panel;

    private void Start()
    {
        Shared.MissionManager = this;

        // 1) 현재 스테이지 번호 가져오기
        int stage = StageData.CurrentStageNum;

        // 2) 배열 범위 체크 후 목표치/시간 할당
        if (stage >= 0 && stage < stageTargetCoinCounts.Length)
            targetCoinCount = stageTargetCoinCounts[stage];
        else
            Debug.LogWarning($"Stage {stage} 에 대한 targetCoinCount 미설정, 기본 0 사용.");

        if (stage >= 0 && stage < stageMissionTimes.Length)
            missionTime = stageMissionTimes[stage];
        else
            Debug.LogWarning($"Stage {stage} 에 대한 missionTime 미설정, 기본 0 사용.");

        // 3) 미션 시작
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

    // 미션 종료 시 알려줄 이벤트
    public static event Action<bool> OnMissionResult;

    private void EndMission(bool success)
    {
        isMissionActive = false;
        isMissionEnded = true;

        if (success)
        {
            Shared.GameManager.AddMoney(currentCoinCount);
            Debug.Log("미션 성공!");
            UnlockNextStage(StageData.CurrentStageNum);
        }
        else
            Debug.Log("미션 실패.");

        // UI에게 성공/실패 알림
        OnMissionResult?.Invoke(success);
    }
    private void UnlockNextStage(int clearedStage)
    {
        Shared.StageData.UnlockStage(clearedStage + 1);
    }

    // 외부에서 남은 시간/진행도 조회용
    public float GetRemainingTime() => remainingTime;
    public string GetMissionProgress() => $"코인 {currentCoinCount}/{targetCoinCount}";
}
