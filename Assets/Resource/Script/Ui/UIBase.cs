using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    protected bool isOptionShow = false;
    protected bool isPauseShow = false;
    public GameObject optionWindow;
    public GameObject pauseWindow;
    public SoundData CLICKSOUND;
    public SoundData BGM;

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
        if (optionWindow == null) return;

        isOptionShow = !isOptionShow;
        optionWindow.SetActive(isOptionShow);
        ClickSound();
    }

    public void TogglePauseWindow()
    {
        if (pauseWindow == null) return;

        isPauseShow = !isPauseShow;
        pauseWindow.SetActive(isPauseShow);
        ClickSound();

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
            

    }

    public void OnBtnRepeat()
    {
        if(isPauseShow)
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
    public void OnBtnHome() => ChangeScene(eSCENE.eSCENE_LOBBY);

    public void ClickSound() => Shared.SoundManager.PlaySound(CLICKSOUND);
    public void PlayBGM() => Shared.SoundManager.PlaySound(BGM);


}
