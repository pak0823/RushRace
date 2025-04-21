using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarStats", menuName = "Cars/CarStats")]
public class CarStats : ScriptableObject
{
    public string carId;              // ���� �ĺ��� �Ǵ� �̸�
    public float motorTorque = 0.0f; // ���ӵ�
    public float brakeTorque = 0.0f; // ������
    public float maxSteerAngle = 0.0f; // �ִ� �ڵ� ����
    public float maxSpeed = 0.0f;     // km/h ����
    public float decelerationRate = 0.0f;
    public float driftFactor = 0.0f;
}
