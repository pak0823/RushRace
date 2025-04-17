using UnityEngine;
using UnityEngine.UI;

public class MissionResultUI : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject panel;       // MissionResultPanel
    public Text resultText;        // ResultText 컴포넌트
    public Text getCoinText;       // getCoinText 컴포넌트

    private void OnEnable()
    {
        MissionManager.OnMissionResult += OnMissionResult;
    }

    private void OnDisable()
    {
        MissionManager.OnMissionResult -= OnMissionResult;
    }

    private void Start()
    {
        // 시작 시 무조건 숨김
        panel.SetActive(false);
    }

    private void OnMissionResult(bool success)
    {
        // 패널 켜고 텍스트 설정
        panel.SetActive(true);
        resultText.text = success ? "미션 클리어!" : "미션 실패…";
        getCoinText.text = $"획득한 {Shared.MissionManager.GetMissionProgress()}";

        // 게임(타이머, 차량 등) 일시정지
        Time.timeScale = 0f;
    }

    public void OnRetryClicked()
    {
        // 시간 흐름 복원
        Time.timeScale = 1f;

        // 미션 재시작
        Shared.MissionManager.StartMission();

        // 차량 리셋, 코인 리셋 등 필요한 로직 직접 호출
        Shared.Car.ResetVehicle();
        Shared.CoinManager?.ResetCoins();

        panel.SetActive(false);
    }

    public void OnExitStageClicked()
    {
        // 시간 흐름 복원
        Time.timeScale = 1f;

        // 스테이지 선택 화면으로 돌아가기
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_STAGE);
    }
}
