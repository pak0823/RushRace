using UnityEngine;

public class UIBase : MonoBehaviour
{
    protected bool isOptionShow = false;
    public GameObject OptionWindow;

    public virtual void ToggleOptionWindow()
    {
        if (OptionWindow == null) return;

        isOptionShow = !isOptionShow;
        OptionWindow.SetActive(isOptionShow);
    }

    public void OpenOptionWindow() => SetOptionWindow(true);
    public void CloseOptionWindow() => SetOptionWindow(false);

    private void SetOptionWindow(bool show)
    {
        isOptionShow = show;
        if (OptionWindow != null)
            OptionWindow.SetActive(show);
    }

    public void ChangeScene(eSCENE scene)
    {
        Shared.SceneMgr.ChangeScene(scene);
    }
}
