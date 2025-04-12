using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCarRotator : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}
