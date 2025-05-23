// UI_Title.cs - 타이틀 화면 UI 제어

using UnityEngine;

public class UI_Title : UIBase
{
    public GameObject Credits;
    private bool isCredit = false;
    public SoundData STARTBTNSOUND;

    public override void Start()
    {
        Shared.SoundManager.StopLoopSound();
        OPTIONSHOW.SetActive(false);
        PlayBGM();
    }
    public void OnBtnOptionShow() => ToggleOptionWindow();

    public void OnBtnGoToLoading()
    {
        Shared.SoundManager.PlaySound(STARTBTNSOUND);
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_LOADING);
    }

    public void OnBtnToggleCredits()
    {
        ClickSound();
        isCredit = !isCredit;
        if (Credits != null)
            Credits.SetActive(isCredit);
    }

    public void OnBtnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnBtnResetData()
    {
#if UNITY_EDITOR
        DataResetter.ResetAllData();
#endif
    }
}
