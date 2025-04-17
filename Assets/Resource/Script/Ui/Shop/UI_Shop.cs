// UI_Shop.cs - 상점 UI 처리 및 버튼 연동

using UnityEngine;

public class UI_Shop : UIBase
{
    public override void Start()
    {
        OPTIONSHOW.SetActive(false);
        PlayBGM();
    }

    public void OnBtnBack()
    {
        ClickSound();
        ChangeScene(eSCENE.eSCENE_LOBBY);
    }
}