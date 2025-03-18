using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarInitSetting", menuName = "ScriptableObjects/CarInitSetting", order = 1)]
public class CarInitSetting : ScriptableObject
{
    [System.Serializable]
    public struct WheelPrefabSettings
    {
        public GameObject wheelPrefab; // Wheel 프리팹
        public float rotationSpeedMultiplier; // 바퀴 회전 속도 배율
    }

    [System.Serializable]
    public struct WheelSettings
    {
        public WheelCollider wheelCollider; // WheelCollider 컴포넌트
        public Transform wheelTransform; // 바퀴 모델 Transform
        public float rotationSpeedMultiplier; // 바퀴 회전 속도 배율
    }

    public WheelSettings frontLeftWheelSettings;
    public WheelSettings frontRightWheelSettings;
    public WheelSettings rearLeftWheelSettings;
    public WheelSettings rearRightWheelSettings;

    public float motorTorque = 300f; // 엔진 토크
    public float brakeTorque = 500f; // 브레이크 힘
    public float maxSteerAngle = 20f; // 최대 스티어링 각도
    public float decelerationRate = 100f; // 감속 속도 (km/h/s)
    public float maxSpeed = 200f; // 최대 속도
}
