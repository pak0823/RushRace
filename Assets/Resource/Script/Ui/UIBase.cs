using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    protected bool isShow = false;
    public GameObject Window;

    public virtual void ToggleOptionWindow()
    {
        if (Window == null) return;

        isShow = !isShow;
        Window.SetActive(isShow);
    }

    public void OpenOptionWindow() => SetOptionWindow(true);
    public void CloseOptionWindow() => SetOptionWindow(false);

    private void SetOptionWindow(bool show)
    {
        isShow = show;
        if (Window != null)
            Window.SetActive(show);
    }

    public void ChangeScene(eSCENE scene)
    {
        Shared.SceneMgr.ChangeScene(scene);
    }
}
