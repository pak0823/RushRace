using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Objects : MonoBehaviour
{
    private float rotationSpeed = 60f; // 회전 속도 (도/초)
    private float smoothSpeed = 5f;  // 회전 부드러움 정도
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

        // 현재 회전값을 목표로 부드럽게 회전
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
