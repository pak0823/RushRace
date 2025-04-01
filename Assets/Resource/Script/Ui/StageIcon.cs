using UnityEngine.UI;
using UnityEngine;

public class StageIcon : MonoBehaviour
{
    private float rotationSpeed = 30f; // 회전 속도 (도/초)
    private float smoothSpeed = 5f;  // 회전 부드러움 정도
    private float currentYRotation = 0f;
    private string currentStageName;
    private bool isMouseOver = false;
    
    private Quaternion defaultRotation;           // 초기 회전값
    private Quaternion targetRotation;            // 회전 목표값

    private void Start()
    {
        defaultRotation = transform.rotation;     // 초기 회전값 저장
        targetRotation = defaultRotation;         // 처음엔 같은 값
    }

    void Update()
    {
        if (!isMouseOver)
        {
            // 계속 목표 회전값 증가
            currentYRotation += rotationSpeed * Time.deltaTime;
            targetRotation = Quaternion.Euler(0, currentYRotation, 0);
        }
        else
        {
            // 원래 회전값으로 돌아감
            targetRotation = defaultRotation;
        }

        // 현재 회전값을 목표로 부드럽게 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        if(!Shared.UI_Stage.isWindow)
        {
            Debug.Log(gameObject.name);
            string clickedObjectName = gameObject.name;

            // 캡슐 이름을 스테이지 이름으로 매핑
            currentStageName = GetStageName(clickedObjectName);

            if (currentStageName == "error")
            {
                Debug.LogError("잘못된 스테이지 오브젝트 이름입니다.");
                return;
            }

            // 텍스트 출력
            Shared.UI_Stage.stageText.text = $"{currentStageName}를 선택하시겠습니까?";
            Shared.UI_Stage.IsWindow(true);
        }
    }

    private string GetStageName(string objectName)
    {
        switch (objectName)
        {
            case "ForestCapsule": return "1Stage";
            case "DesertCapsule": return "2Stage";
            case "DowntownCapsule": return "3Stage";
            default: return "error";
        }
    }

    public string GetCurrentStage()
    {
        return currentStageName;
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }
}
