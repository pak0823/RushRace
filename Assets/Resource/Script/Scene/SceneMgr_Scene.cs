using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    �� ������ ����ϰ� �״�� ����ϸ� �ȴ�. ����ȸ�翡�� �̹� ������ �ڵ��̴�.
    ���� �� �߰� ���Ű� ���� �׷� ���� ���������� ����.
 
 */
public partial class SceneMgr
{
    public eSCENE Scene = eSCENE.eSCENE_TITLE;

    public void ChangeScene(eSCENE _e, bool _Loading = false)//�Ű������� _�� �־ ����ϸ� �����Ͽ� ���� ����. �׳� ����ؼ� �ͼ�������
    {
        if (Scene == _e)
        {
            Debug.Log("�̹� ���� ���� ���� �ֽ��ϴ�");
            return;
        }
            

        // �ε� ������ ��ȯ
        SceneManager.LoadScene("LoadingScene");

        // LoadingScene���� ��ȯ �� ���� �� �̸��� �����ϴ� �ڷ�ƾ ����
        Scene = _e;
        StartCoroutine(SetNextSceneName(_e));
    }

    IEnumerator SetNextSceneName(eSCENE _e)
    {
        // �ε� ���� ������ �ε�� ������ ��ٸ�
        yield return null;

        // LoadingScene�� UI_Loading ��ũ��Ʈ�� ã�Ƽ� ���� �� �̸��� ����
        UI_Loading loadingScript = FindObjectOfType<UI_Loading>();

        if (loadingScript != null)
        {
            /*
            switch (_e)
            {
                case eSCENE.eSCENE_TITLE:   //SceneManager �Լ� �������� int���� ����� ���������� ���� ���� ���ڿ��� �ۼ��Ѵ�. int���� ����ϰ� ������ ����
                    SceneManager.LoadScene("TitleScene");
                    break;
                case eSCENE.eSCENE_LOADING:
                    SceneManager.LoadScene("LoadingScene");
                    break;
                case eSCENE.eSCENE_LOBBY:
                    SceneManager.LoadScene("LobbyScene");
                    break;
                case eSCENE.eSCENE_STAGE:
                    SceneManager.LoadScene("StageScene");
                    break;
                case eSCENE.eSCENE_INGAME:
                    SceneManager.LoadScene("IngameScene");
                    break;
                case eSCENE.eSCENE_TEST:
                    SceneManager.LoadScene("TestScene");
                    break;
                case eSCENE.eSCENE_END:
                    SceneManager.LoadScene("EndScene");
                    break;
            
            }
            */

            switch (_e)
            {
                case eSCENE.eSCENE_TITLE:
                    loadingScript.nextSceneName = "TitleScene";
                    break;
                case eSCENE.eSCENE_LOADING:
                    loadingScript.nextSceneName = "LoadingScene";
                    break;
                case eSCENE.eSCENE_LOBBY:
                    loadingScript.nextSceneName = "LobbyScene";
                    break;
                case eSCENE.eSCENE_STAGE:
                    loadingScript.nextSceneName = "StageScene";
                    break;
                case eSCENE.eSCENE_INGAME:
                    loadingScript.nextSceneName = "IngameScene";
                    break;
                case eSCENE.eSCENE_SHOP:
                    loadingScript.nextSceneName = "ShopScene";
                    break;
                case eSCENE.eSCENE_REPAIR:
                    loadingScript.nextSceneName = "RepairScene";
                    break;
                case eSCENE.eSCENE_PRACTICE:
                    loadingScript.nextSceneName = "PracticeScene";
                    break;
                case eSCENE.eSCENE_TEST:
                    loadingScript.nextSceneName = "TestScene";
                    break;
                case eSCENE.eSCENE_END:
                    loadingScript.nextSceneName = "EndScene";
                    break;
            }
        }
        else
            Debug.Log("loadingScript is null");
    }
}
