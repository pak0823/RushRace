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
    }

    public void TogglePauseWindow()
    {
        if (pauseWindow == null) return;

        isPauseShow = !isPauseShow;
        pauseWindow.SetActive(isPauseShow);
    }
    public void ChangeScene(eSCENE scene)
    {
        Shared.SceneMgr.ChangeScene(scene);
    }

    public void OnBtnGoTitle() => ChangeScene(eSCENE.eSCENE_TITLE);
    public void OnBtnHome() => ChangeScene(eSCENE.eSCENE_LOBBY);
    public void OnBtnRepeat() => Shared.Car.ResetVehicle();

    
}
