using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Objects : MonoBehaviour
{
    private float rotationSpeed = 60f; // ȸ�� �ӵ� (��/��)
    private float smoothSpeed = 5f;  // ȸ�� �ε巯�� ����
    private float currentYRotation = 0f;

    private Quaternion targetRotation;

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        currentYRotation += rotationSpeed * Time.deltaTime;
        targetRotation = Quaternion.Euler(0, currentYRotation, 0);

        // ���� ȸ������ ��ǥ�� �ε巴�� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
