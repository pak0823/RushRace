// StageIcon.cs

using UnityEngine;

public class StageIcon : MonoBehaviour
{
    public int stageNum;
    public float rotationSpeed = 30f;

    private float currentYRotation = 0f;
    private bool isHovered = false;

    private void Update()
    {
        if (!isHovered)
        {
            currentYRotation += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
        }
    }

    private void OnMouseEnter() => isHovered = true;
    private void OnMouseExit() => isHovered = false;

    private void OnMouseDown()
    {
        UI_Stage uiStage = FindObjectOfType<UI_Stage>();
        if (uiStage != null)
        {
            uiStage.OnStageIconClicked(stageNum);  // 클릭 → UI에 전달
        }
    }
}
