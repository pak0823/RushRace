using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI_Lobby : MonoBehaviour
{
    public GameObject backBtn;
    public GameObject OptionWindow;

    bool isOptionShow = false;

    void Start()
    {
        
    }

    public void OnBtnOptionWindows()//옵션창 닫기
    {
        if(isOptionShow)
        {
            OptionWindow.gameObject.SetActive(false);
            isOptionShow = false;
        } 
        else
        {
            OptionWindow.gameObject.SetActive(true);
            isOptionShow = true;
        }
            
    }

    public void OnBtnGoInGame()
    {
        Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_TEST);
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
