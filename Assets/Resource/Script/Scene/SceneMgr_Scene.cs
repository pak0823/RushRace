using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    이 구조를 기억하고 그대로 사용하면 된다. 게임회사에서 이미 검증된 코드이다.
    변경 및 추가 제거가 쉽고 그로 인해 유지보수가 좋다.
 
 */
public partial class SceneMgr
{
    public eSCENE Scene = eSCENE.eSCENE_TITLE;

    public void ChangeScene(eSCENE _e, bool _Loading = false)//매개변수명에 _를 넣어서 사용하면 구분하여 보기 좋다. 그냥 사용해서 익숙해지자
    {
        if (Scene == _e)
        {
            Debug.Log("이미 같은 씬에 속해 있습니다");
            return;
        }
            

        // 로딩 씬으로 전환
        SceneManager.LoadScene("LoadingScene");

        // LoadingScene으로 전환 후 다음 씬 이름을 설정하는 코루틴 실행
        Scene = _e;
        StartCoroutine(SetNextSceneName(_e));
    }

    IEnumerator SetNextSceneName(eSCENE _e)
    {
        // 로딩 씬이 완전히 로드될 때까지 기다림
        yield return null;

        // LoadingScene의 UI_Loading 스크립트를 찾아서 다음 씬 이름을 설정
        UI_Loading loadingScript = FindObjectOfType<UI_Loading>();

        if (loadingScript != null)
        {
            /*
            switch (_e)
            {
                case eSCENE.eSCENE_TITLE:   //SceneManager 함수 구성에는 int형도 사용이 가능하지만 보기 좋게 문자열로 작성한다. int형도 사용하고 싶으면 가능
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
