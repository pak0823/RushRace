using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Practice : UIBase
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleOptionWindow();
    }

    public void OnBtnGoTitle()
    {
        ChangeScene(eSCENE.eSCENE_TITLE);
    }
}
