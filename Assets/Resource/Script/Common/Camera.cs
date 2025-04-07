using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target; // 따라다닐 대상
    public float distance = 10.0f; // 대상과의 거리
    public float height = 5.0f; // 대상보다 높은 위치
    public float smoothSpeed = 10.0f; // 부드러운 움직임을 위한 속도
    public float rotationDamping = 5.0f; // 회전 댐핑 추가
    public Vector3 offset = new Vector3(0, 7, -10); // 오프셋 값

    private Vector3 velocity = Vector3.zero; // 현재 속도

    private void LateUpdate()
    {
        if (target == null) return; // 대상이 없으면 아무것도 하지 않음

        // 원하는 위치 계산
        Vector3 desiredPosition = target.TransformPoint(offset);

        // 부드럽게 위치 변경 (SmoothDamp 유지)
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;

        // 부드럽게 회전 (LookAtRotation 사용)
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationDamping * Time.fixedDeltaTime);
    }
}
