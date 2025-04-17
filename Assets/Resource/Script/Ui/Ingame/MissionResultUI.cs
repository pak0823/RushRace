using UnityEngine;
using UnityEngine.UI;

public class MissionResultUI : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject panel;       // MissionResultPanel
    public Text resultText;        // ResultText ������Ʈ
    public Text getCoinText;       // getCoinText ������Ʈ

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
        // ���� �� ������ ����
        panel.SetActive(false);
    }

    private void OnMissionResult(bool success)
    {
        // �г� �Ѱ� �ؽ�Ʈ ����
        panel.SetActive(true);
        resultText.text = success ? "�̼� Ŭ����!" : "�̼� ���С�";
        getCoinText.text = $"ȹ���� {Shared.MissionManager.GetMissionProgress()}";

        // ����(Ÿ�̸�, ���� ��) �Ͻ�����
        Time.timeScale = 0f;
    }

    public void OnRetryClicked()
    {
        // �ð� �帧 ����
        Time.timeScale = 1f;

        // �̼� �����
        Shared.MissionManager.StartMission();

        // ���� ����, ���� ���� �� �ʿ��� ���� ���� ȣ��
        Shared.Car.ResetVehicle();
        Shared.CoinManager?.ResetCoins();

        panel.SetActive(false);
    }

    public void OnExitStageClicked()
    {
        // �ð� �帧 ����
        Time.timeScale = 1f;

        // �������� ���� ȭ������ ���ư���
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_STAGE);
    }
}
