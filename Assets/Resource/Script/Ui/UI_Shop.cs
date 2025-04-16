// UI_Shop.cs - ���� UI ó�� �� ��ư ����

using UnityEngine;

public class UI_Shop : UIBase
{
    public SoundData SHOP_BGM;

    public override void Start()
    {
        OPTIONSHOW.SetActive(false);
        PlayBGM();
    }

    public void OnBtnBuyItem()
    {
        ClickSound();
        Shared.GameManager.SpendMoney(10); // ���� �ݾ� ����
    }

    public void OnBtnBack()
    {
        ClickSound();
        ChangeScene(eSCENE.eSCENE_LOBBY);
    }
}