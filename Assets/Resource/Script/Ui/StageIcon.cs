using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIcon : MonoBehaviour
{
    float rotationSpeed = 30f; // 회전 속도 (도/초)
    public float smoothSpeed = 5f;  // 회전 부드러움 정도
    private float currentYRotation = 0f;

    private bool isMouseOver = false;

    private Quaternion defaultRotation;           // 초기 회전값
    private Quaternion targetRotation;            // 회전 목표값

    private void Start()
    {
        Shared.StageIcon = this;
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

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }
}
