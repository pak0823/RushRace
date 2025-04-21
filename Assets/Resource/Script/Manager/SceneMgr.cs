// SceneMgr.cs - 씬 전환을 관리하는 매니저

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    private void Awake()
    {
        if (Shared.SceneMgr == null)
        {
            Shared.SceneMgr = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene(eSCENE scene)
    {
        Shared.SoundManager.StopPlaySound();
        Shared.SoundManager.StopLoopSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene.ToString());
    }
}
