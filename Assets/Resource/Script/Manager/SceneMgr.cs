// SceneMgr.cs - �� ��ȯ�� �����ϴ� �Ŵ���

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
        SceneManager.LoadScene(scene.ToString());
    }
}
