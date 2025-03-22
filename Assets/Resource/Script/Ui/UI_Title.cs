using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title : MonoBehaviour
{
    public GameObject OptionWindow;//옵션창 오브젝트
    public GameObject Credits;//크레딧창 오브젝트

    bool isCredit = false;

    public void Start()
    {
        OptionWindow.gameObject.SetActive(false);
    }

    public void OnBtnGoToLoading()
    {
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_TEST);
    }


    public void OnBtnOpenOptionWindows()//옵션창 열기
    {
        OptionWindow.gameObject.SetActive(true);
    }
    public void OnBtnCloseOptionWindows()//옵션창 닫기
    {
        OptionWindow.gameObject.SetActive(false);
    }

    public void OnBtnOpenCreaditWindows()//크레딧 온오프
    {
        if(isCredit)
        {
            Credits.gameObject.SetActive(false);
            isCredit = false;
        }
        else
        {
            Credits.gameObject.SetActive(true);
            isCredit = true;
        }
            
    }

    public void OnBtnExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // 에디터에서 종료
#else
        Application.Quit(); // 빌드된 게임에서 종료
#endif
    }

}