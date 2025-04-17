// UI_Practice.cs - ���� ��� UI �� �Ͻ�����/�ɼ� ����

using UnityEngine;
using UnityEngine.UI;

public class UI_Practice : UIBase
{
    public SoundData PRACTICE_BGM;
    public Text speedText;

    public override void Start()
    {
        OPTIONSHOW.SetActive(false);
        PlayBGM();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOptionShow)
                ToggleOptionWindow();
            else
                TogglePauseWindow();
        }

        if (speedText != null)
            speedText.text = string.Format("{0:0} Km/h", Shared.Car.currentSpeed);
    }
}
