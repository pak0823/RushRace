using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Practice : UIBase
{
    public GameObject PauseWindow;
    bool isPauseShow = false;

    void Start()
    {
        ClosePauseWindow();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (!isPauseShow)
                OpenPauseWindow();
            else
                ClosePauseWindow();
    }

    public void OpenPauseWindow() => SetPauseWindow(true);
    public void ClosePauseWindow() => SetPauseWindow(false);

    private void SetPauseWindow(bool show)
    {
        isPauseShow = show;
        if (PauseWindow != null)
            PauseWindow.SetActive(show);
    }

    public void OnBtnGoTitle()
    {
        ChangeScene(eSCENE.eSCENE_TITLE);
    }

    public void OnBtnHome()
    {
        ChangeScene(eSCENE.eSCENE_LOBBY);
    }

    public void OnBtnStart()
    {
        ClosePauseWindow();
    }
    public void OnBtnRepeat()
    {
        Shared.Car.ResetVehicle();
    }
}
