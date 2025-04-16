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
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (isOptionShow)
        //        ToggleOptionWindow();
        //    else if (isPauseShow)
        //        TogglePauseWindow();
        //}
    }

    public void ToggleOptionWindow()
    {
        if (OPTIONSHOW == null) return;

        isOptionShow = !isOptionShow;
        OPTIONSHOW.SetActive(isOptionShow);
        OPTIONSHOW.transform.parent.gameObject.SetActive(isOptionShow);
        ClickSound();
    }
    public virtual void TogglePauseWindow() 
    {
        if (PAUSESHOW == null) return;

        isPauseShow = !isPauseShow;

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

        
        PAUSESHOW.SetActive(isPauseShow);
        PAUSESHOW.transform.parent.gameObject.SetActive(isPauseShow);
        ClickSound();
    }

    public void OnBtnRepeat()
    {
        if (isPauseShow)
        {
            Time.timeScale = 1f;
            TogglePauseWindow();
        }

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

    public void ChangeScene(eSCENE scene)
    {
        Shared.SceneMgr.ChangeScene(scene);
        Shared.SoundManager.StopPlaySound();
        Shared.SoundManager.StopLoopSound();
        Time.timeScale = 1f;
    }

    public void OnBtnGoTitle() => ChangeScene(eSCENE.eSCENE_TITLE);
    public void OnBtnLobby() => ChangeScene(eSCENE.eSCENE_LOBBY);

    public void ClickSound() => Shared.SoundManager.PlaySound(CLICKSOUND);
    public void PlayBGM() => Shared.SoundManager.PlaySound(BGM);
}
