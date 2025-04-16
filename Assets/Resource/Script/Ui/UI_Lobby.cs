// UI_Lobby.cs - �κ� UI �� ��ư �̺�Ʈ ó��

using UnityEngine;

public class UI_Lobby : UIBase
{
    public override void Start()
    {
        OPTIONSHOW.transform.parent.gameObject.SetActive(false);
        PlayBGM();
    }

    public void OnBtnGoToIngame()
    {
        ClickSound();
        ChangeScene(eSCENE.eSCENE_STAGE);
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
