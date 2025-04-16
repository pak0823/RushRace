// UI_Repair.cs - 정비소 UI 처리 및 버튼 연동

using UnityEngine;

public class UI_Repair : UIBase
{
    public SoundData REPAIR_BGM;

    public override void Start()
    {
        OPTIONSHOW.transform.parent.gameObject.SetActive(false);
        PlayBGM();
    }

    public void OnBtnRepair()
    {
        ClickSound();
        Shared.GameManager.SpendMoney(5); // 예시 금액 차감
    }

    public void OnBtnBack()
    {
        ClickSound();
        ChangeScene(eSCENE.eSCENE_LOBBY);
    }
}
