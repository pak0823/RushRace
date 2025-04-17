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
        // 스테이지 잠금 여부 확인
        if (!Shared.StageData.IsStageUnlocked(stageNum))
        {
            Debug.Log($"스테이지 {stageNum+1}는 아직 잠겨있습니다.");
            return;
        }
        if (Shared.UI_Stage != null)
        {
            Shared.UI_Stage.OnStageIconClicked(stageNum);  // 클릭 → UI에 전달
        }
    }
}
