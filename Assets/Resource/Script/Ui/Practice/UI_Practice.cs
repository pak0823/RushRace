// UI_Practice.cs - 연습 모드 UI 및 일시정지/옵션 제어

using UnityEngine;
using UnityEngine.UI;

public class UI_Practice : UIBase
{
    public SoundData PRACTICE_BGM;
    public Text speedText;

    public override void Start()
    {
        OPTIONSHOW.SetActive(false);
        PlayBGM();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOptionShow)
                ToggleOptionWindow();
            else
                TogglePauseWindow();
        }

        if (speedText != null)
            speedText.text = string.Format("{0:0} Km/h", Shared.Car.currentSpeed);
    }
}
