using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIcon : MonoBehaviour
{
    float rotationSpeed = 30f; // �ʴ� 90�� ȸ��
    private float currentYRotation = 0f;

    private void Start()
    {
        // 360�� ������ 0���� �ٽ� ����
        if (currentYRotation >= 360f)
            currentYRotation = 0f;
    }

    void Update()
    {
        currentYRotation += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(130f, currentYRotation, 90f);
    }
}
