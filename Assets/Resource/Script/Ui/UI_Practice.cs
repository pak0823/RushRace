// UI_Practice.cs - 연습 모드 UI 및 일시정지/옵션 제어

using UnityEngine;

public class UI_Practice : UIBase
{
    public SoundData PRACTICE_BGM;

    private void Start()
    {
        PlayBGM();
    }

    //public override void ToggleOptionWindow()
    //{
    //    base.ToggleOptionWindow();

    //    if (isOptionShow)
    //    {
    //        Time.timeScale = 0f;
    //        Shared.SoundManager.StopLoopSound();
    //    }
    //    else
    //    {
    //        Time.timeScale = 1f;
    //        Shared.SoundManager.ResumeLoopSound();
    //    }
    //}

    public override void TogglePauseWindow()
    {
        base.TogglePauseWindow();

        if (isPauseShow)
        {
            Time.timeScale = 0f;
            Shared.SoundManager.StopLoopSound();
        }
        else
        {
            Time.timeScale = 1f;
            Shared.SoundManager.ResumeLoopSound();
        }
    }
}
