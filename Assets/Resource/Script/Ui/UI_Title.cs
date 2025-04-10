using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UI_Title : UIBase
{
    public GameObject Credits;
    private bool isCredit = false;
    public SoundData STARTBTNSOUND;
    public VideoPlayer backgroundVideo;

    private void Start()
    {
        Shared.SoundManager.PlaySound(BGM);
        if (backgroundVideo != null)
        {
            backgroundVideo.renderMode = VideoRenderMode.CameraFarPlane;
            backgroundVideo.isLooping = true;
            backgroundVideo.Play();
        }
        else
            Debug.Log("backgroundVideo is null");
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