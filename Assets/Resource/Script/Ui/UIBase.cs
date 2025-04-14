// UIBase.cs - UI 전반 공통 기능을 관리하는 베이스 클래스

using UnityEngine;

public class UIBase : MonoBehaviour
{
    protected bool isOptionShow = false;
    protected bool isPauseShow = false;

    public SoundData CLICKSOUND;
    public SoundData BGM;

    public GameObject OPTIONSHOW;

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOptionShow)
                ToggleOptionWindow();
            else if (isPauseShow)
                TogglePauseWindow();
        }
    }

    public void ToggleOptionWindow()
    {
        if (OPTIONSHOW == null) return;

        isOptionShow = !isOptionShow;
        OPTIONSHOW.SetActive(isOptionShow);
        ClickSound();
    }
    public virtual void TogglePauseWindow() { }

    public void OnBtnRepeat()
    {
        if (isPauseShow)
        {
            Time.timeScale = 1f;
            TogglePauseWindow();
        }
        Shared.Car.ResetVehicle();
    }

    public void ChangeScene(eSCENE scene)
    {
        Shared.SceneMgr.ChangeScene(scene);
        Shared.SoundManager.StopPlaySound();
        Shared.SoundManager.StopLoopSound();
    }

    public void OnBtnGoTitle() => ChangeScene(eSCENE.eSCENE_TITLE);
    public void OnBtnLobby() => ChangeScene(eSCENE.eSCENE_LOBBY);

    public void ClickSound() => Shared.SoundManager.PlaySound(CLICKSOUND);
    public void PlayBGM() => Shared.SoundManager.PlaySound(BGM);
}
