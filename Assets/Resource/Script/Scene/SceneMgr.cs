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
        //���� ���� �������� ����
    }
}
