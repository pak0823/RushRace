using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    이 구조를 기억하고 그대로 사용하면 된다. 게임회사에서 이미 검증된 코드이다.
    변경 및 추가 제거가 쉽고 그로 인해 유지보수가 좋다.
 
 */
public partial class SceneMgr : MonoBehaviour
{
    public eSCENE Scene = eSCENE.eSCENE_TITLE;

    public void ChangeScene(eSCENE _e, bool _Loading = false)//매개변수명에 _를 넣어서 사용하면 구분하여 보기 좋다. 그냥 사용해서 익숙해지자
    {
        if (Scene == _e)
            return;


        switch (_e)
        {
            case eSCENE.eSCENE_TITLE:   //SceneManager 함수 구성에는 int형도 사용이 가능하지만 보기 좋게 문자열로 작성한다. int형도 사용하고 싶으면 가능
                SceneManager.LoadScene("TitleScene");
                break;
            case eSCENE.eSCENE_LOADING:
                SceneManager.LoadScene("LoadingScene");
                break;
            case eSCENE.eSCENE_STAGE:
                SceneManager.LoadScene("LoadingScene");
                break;
            case eSCENE.eSCEME_INGAME:
                SceneManager.LoadScene("LoadingScene");
                break;
            case eSCENE.eSCEME_TEST:
                SceneManager.LoadScene("TestScene");
                break;
            case eSCENE.eSCENE_END:
                SceneManager.LoadScene("End");
                break;
        }
    }
}
