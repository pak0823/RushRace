using UnityEngine.UI;
using UnityEngine;

public class StageIcon : MonoBehaviour
{
    private float rotationSpeed = 30f; // ȸ�� �ӵ� (��/��)
    private float smoothSpeed = 5f;  // ȸ�� �ε巯�� ����
    private float currentYRotation = 0f;
    private string currentStageName;
    private bool isMouseOver = false;
    
    private Quaternion defaultRotation;           // �ʱ� ȸ����
    private Quaternion targetRotation;            // ȸ�� ��ǥ��

    private void Start()
    {
        defaultRotation = transform.rotation;     // �ʱ� ȸ���� ����
        targetRotation = defaultRotation;         // ó���� ���� ��
    }

    void Update()
    {
        if (!isMouseOver)
        {
            // ��� ��ǥ ȸ���� ����
            currentYRotation += rotationSpeed * Time.deltaTime;
            targetRotation = Quaternion.Euler(0, currentYRotation, 0);
        }
        else
        {
            // ���� ȸ�������� ���ư�
            targetRotation = defaultRotation;
        }

        // ���� ȸ������ ��ǥ�� �ε巴�� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        if(!Shared.UI_Stage.isWindow)
        {
            Debug.Log(gameObject.name);
            string clickedObjectName = gameObject.name;

            // ĸ�� �̸��� �������� �̸����� ����
            currentStageName = GetStageName(clickedObjectName);

            if (currentStageName == "error")
            {
                Debug.LogError("�߸��� �������� ������Ʈ �̸��Դϴ�.");
                return;
            }

            // �ؽ�Ʈ ���
            Shared.UI_Stage.stageText.text = $"{currentStageName}�� �����Ͻðڽ��ϱ�?";
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
