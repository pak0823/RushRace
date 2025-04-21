// UIBase.cs - UI 전반 공통 기능을 관리하는 베이스 클래스

using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    protected bool isOptionShow = false;
    protected bool isPauseShow = false;

    public SoundData CLICKSOUND;
    public SoundData BGM;

    public GameObject OPTIONSHOW;
    public GameObject PAUSESHOW;

    public abstract void Start();

    public virtual void Update()
    {

    }

    public void ToggleOptionWindow()
    {
        if (OPTIONSHOW == null) return;
        if (isPauseShow) TogglePauseWindow();

        isOptionShow = !isOptionShow;
        OPTIONSHOW.SetActive(isOptionShow);
        ClickSound();
    }
    public virtual void TogglePauseWindow() 
    {
        if (PAUSESHOW == null) return;
        if (isOptionShow) ToggleOptionWindow();

        isPauseShow = !isPauseShow;

        if (isPauseShow)
        {
            Shared.SoundManager.StopLoopSound();
            Time.timeScale = 0f;
        }
        else
        {
            Shared.SoundManager.ResumeLoopSound();
            Time.timeScale = 1f;
        }
        PAUSESHOW.SetActive(isPauseShow);
        ClickSound();
    }

    public void OnBtnRepeat()
    {
        Time.timeScale = 1f;

        if (isPauseShow)
            TogglePauseWindow();

        Shared.Car.ResetVehicle();

        // 미션 상태 초기화: 남은 시간과 수집 코인 초기화
        if (Shared.MissionManager != null)
        {
            Shared.MissionManager.StartMission();
        }
        if (Shared.CoinManager != null)
        {
            Shared.CoinManager.ResetCoins();
        }
    }

    public void OnBtnGoTitle() => Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_TITLE);
    public void OnBtnLobby() => Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_LOBBY);

    public void ClickSound() => Shared.SoundManager.PlaySound(CLICKSOUND);
    public void PlayBGM() => Shared.SoundManager.PlaySound(BGM);
}
