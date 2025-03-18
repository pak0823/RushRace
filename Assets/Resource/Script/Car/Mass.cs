using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fMass : MonoBehaviour
{
    public Color gizmoColor = Color.yellow; // Gizmo 색상
    public float gizmoSize = 0.5f; // Gizmo 크기

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody 컴포넌트가 없습니다!");
            enabled = false; // 스크립트 비활성화
        }
    }

    void OnDrawGizmos()
    {
        if (rb == null) return;

        // 무게 중심 위치 계산
        Vector3 worldCenterOfMass = transform.TransformPoint(rb.centerOfMass);

        // Gizmo 그리기
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(worldCenterOfMass, gizmoSize);
    }
}

