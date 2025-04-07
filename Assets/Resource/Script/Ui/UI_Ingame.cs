using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UI_Ingame : UIBase
{
    public Text speedText;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOptionShow)
                TogglePauseWindow();
            else
                ToggleOptionWindow();
        }

        speedText.text = string.Format("{0:0}", Shared.Car.currentSpeed);
    }
}
