using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title : UIBase
{
    public GameObject Credits;
    private bool isCredit = false;
    public SoundData STARTBTNSOUND;

    private void Start()
    {
        Shared.SoundManager.PlaySound(BGM);
    }
    public void OnBtnGoToLoading()
    {
        Shared.SoundManager.PlaySound(STARTBTNSOUND);
        ChangeScene(eSCENE.eSCENE_LOBBY);
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
}