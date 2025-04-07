using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target; // ����ٴ� ���
    public float distance = 10.0f; // ������ �Ÿ�
    public float height = 5.0f; // ��󺸴� ���� ��ġ
    public float smoothSpeed = 10.0f; // �ε巯�� �������� ���� �ӵ�
    public float rotationDamping = 5.0f; // ȸ�� ���� �߰�
    public Vector3 offset = new Vector3(0, 7, -10); // ������ ��

    private Vector3 velocity = Vector3.zero; // ���� �ӵ�

    private void LateUpdate()
    {
        if (target == null) return; // ����� ������ �ƹ��͵� ���� ����

        // ���ϴ� ��ġ ���
        Vector3 desiredPosition = target.TransformPoint(offset);

        // �ε巴�� ��ġ ���� (SmoothDamp ����)
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;

        // �ε巴�� ȸ�� (LookAtRotation ���)
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationDamping * Time.fixedDeltaTime);
    }
}
