using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIcon : MonoBehaviour
{
    float rotationSpeed = 30f; // 초당 90도 회전
    private float currentYRotation = 0f;

    private void Start()
    {
        // 360도 넘으면 0부터 다시 시작
        if (currentYRotation >= 360f)
            currentYRotation = 0f;
    }

    void Update()
    {
        currentYRotation += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(130f, currentYRotation, 90f);
    }
}
