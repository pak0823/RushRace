// UI_Shop.cs - 상점 UI 처리 및 버튼 연동

using UnityEngine;

public class UI_Shop : UIBase
{
    public SoundData SHOP_BGM;

    private void Start()
    {
        PlayBGM();
    }

    public void OnBtnBuyItem()
    {
        ClickSound();
        Shared.GameManager.SpendMoney(10); // 예시 금액 차감
    }

    public void OnBtnBack()
    {
        ClickSound();
        ChangeScene(eSCENE.eSCENE_LOBBY);
    }
}