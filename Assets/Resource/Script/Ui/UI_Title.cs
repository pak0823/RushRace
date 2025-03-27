using UnityEngine;
using UnityEngine.UI;

public class UI_Title : UIBase
{
    public GameObject Credits;
    private bool isCredit = false;

    void Start()
    {
        CloseOptionWindow();
    }

    public void OnBtnGoToLoading()
    {
        ChangeScene(eSCENE.eSCENE_LOBBY);
    }

    public void OnBtnToggleCredits()
    {
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
}