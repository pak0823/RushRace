using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIcon : MonoBehaviour
{
    float rotationSpeed = 30f; // ȸ�� �ӵ� (��/��)
    public float smoothSpeed = 5f;  // ȸ�� �ε巯�� ����
    private float currentYRotation = 0f;

    private bool isMouseOver = false;

    private Quaternion defaultRotation;           // �ʱ� ȸ����
    private Quaternion targetRotation;            // ȸ�� ��ǥ��

    private void Start()
    {
        Shared.StageIcon = this;
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

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }
}
