using UnityEngine;
using UnityEngine.SceneManagement;

public partial class SceneMgr : MonoBehaviour
{
    private void Awake()
    {
        if(Shared.SceneMgr != null && Shared.SceneMgr != this)
        {
            Destroy(gameObject);
            return;
        }

        Shared.SceneMgr = this;
        DontDestroyOnLoad(this);
        //현재 씬을 제거하지 않음
    }
}
