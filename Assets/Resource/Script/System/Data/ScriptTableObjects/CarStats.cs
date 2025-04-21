using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarStats", menuName = "Cars/CarStats")]
public class CarStats : ScriptableObject
{
    public string carId;              // 내부 식별자 또는 이름
    public float motorTorque = 0.0f; // 가속도
    public float brakeTorque = 0.0f; // 제동력
    public float maxSteerAngle = 0.0f; // 최대 핸들 각도
    public float maxSpeed = 0.0f;     // km/h 단위
    public float decelerationRate = 0.0f;
    public float driftFactor = 0.0f;
}
