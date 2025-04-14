// UI_Lobby.cs - 로비 UI 및 버튼 이벤트 처리

using UnityEngine;

public class UI_Lobby : UIBase
{
    private void Start()
    {
        PlayBGM();
    }

    public void OnBtnGoToPractice()
    {
        ClickSound();
        ChangeScene(eSCENE.eSCENE_PRACTICE);
    }

    public void OnBtnGoToShop()
    {
        ClickSound();
        ChangeScene(eSCENE.eSCENE_SHOP);
    }

    public void OnBtnGoToRepair()
    {
        ClickSound();
        ChangeScene(eSCENE.eSCENE_REPAIR);
    }
}
