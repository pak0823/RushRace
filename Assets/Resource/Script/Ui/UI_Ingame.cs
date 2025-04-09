using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UI_Ingame : UIBase
{
    public Text speedText;
    int stageNum;
    public SoundData STAGE_1_BGM;
    public SoundData STAGE_2_BGM;
    public SoundData STAGE_3_BGM;

    private void Start()
    {
        stageNum = StageData.CurrentStageNum;

        switch(stageNum)
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOptionShow)
                TogglePauseWindow();
            else
                ToggleOptionWindow();
        }

        speedText.text = string.Format("{0:0}Km/s", Shared.Car.currentSpeed);
    }
}
