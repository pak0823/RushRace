// UI_Shop.cs - ���� UI ó�� �� ��ư ����

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