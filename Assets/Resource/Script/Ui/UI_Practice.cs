using UnityEngine;
using UnityEngine.UI;

public class UI_Practice : UIBase
{
    public Text speedText;

    private void Start()
    {
        PlayBGM();
    }
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isOptionShow)
                TogglePauseWindow();
            else
                ToggleOptionWindow();
        }

        speedText.text = string.Format("{0:0}Km/s", Shared.Car.currentSpeed);
    }
}
