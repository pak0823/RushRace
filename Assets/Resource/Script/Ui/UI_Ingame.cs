// UI_Ingame.cs - �ΰ��� UI �� �Ͻ�����/�ɼ�â ����

using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : UIBase
{
    public Text speedText;
    public Text missionText;
    public Text timerText;

    public SoundData STAGE_1_BGM;
    public SoundData STAGE_2_BGM;
    public SoundData STAGE_3_BGM;

    private void Start()
    {
        int stageNum = StageData.CurrentStageNum;

        switch (stageNum)
        {
            case 1:
                Shared.SoundManager.PlaySound(STAGE_1_BGM);
                break;
            case 2:
                Shared.SoundManager.PlaySound(STAGE_2_BGM);
                break;
            case 3:
                Shared.SoundManager.PlaySound(STAGE_3_BGM);
                break;
        }
    }

    public override void Update()
    {
        base.Update();

        if (speedText != null)
            speedText.text = string.Format("{0:0} Km/h", Shared.Car.currentSpeed);

        if (missionText != null && Shared.MissionManager != null)
            missionText.text = Shared.MissionManager.GetMissionProgress();

        if (timerText != null && Shared.MissionManager != null)
            timerText.text = string.Format("{0:0.0}s", Shared.MissionManager.GetRemainingTime());
    }
}
