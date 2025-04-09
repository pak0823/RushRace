using UnityEngine;
using UnityEngine.UI;

public class UI_Stage : UIBase
{
    public GameObject checkWindow;
    public Text stageText;
    public bool isWindow = false;

    private void Start()
    {
        Shared.UI_Stage = this;
        PlayBGM();
    }
    public void OnBtnYes() => ChangeScene(eSCENE.eSCENE_INGAME);
    public void OnBtnNo() => IsWindow(false);
    public void OnBtnBack() => Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_LOBBY);

    public void IsWindow(bool _isshow)
    {
        checkWindow.gameObject.SetActive(_isshow);

        if(_isshow)
            isWindow = true;
        else
            isWindow = false;

        ClickSound();
    }
}