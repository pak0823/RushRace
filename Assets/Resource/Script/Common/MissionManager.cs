// MissionManager.cs - 미션 로직 처리 (제한 시간 내 코인 수집)

using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("미션 설정")]
    public int targetCoinCount = 10;
    public float missionTime = 60f;

    private float remainingTime;
    private int currentCoinCount;
    private bool isMissionActive = false;
    private bool isMissionEnded = false;

    private void Start()
    {
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
            Debug.Log("미션 성공!");
        }
        else
        {
            Debug.Log("미션 실패.");
        }
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public string GetMissionProgress()
    {
        return $"코인 {currentCoinCount}/{targetCoinCount}";
    }
}
