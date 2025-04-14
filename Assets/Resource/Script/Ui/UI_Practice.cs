// UI_Practice.cs - ���� ��� UI �� �Ͻ�����/�ɼ� ����

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
