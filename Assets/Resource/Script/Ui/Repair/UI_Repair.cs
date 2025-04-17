// UI_Repair.cs - ����� UI ó�� �� ��ư ����

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
        Shared.GameManager.SpendMoney(5); // ���� �ݾ� ����
    }

    public void OnBtnBack()
    {
        ClickSound();
        ChangeScene(eSCENE.eSCENE_LOBBY);
    }
}
