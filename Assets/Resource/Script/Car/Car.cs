using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Car : MonoBehaviour
{
    // 바퀴 정보 구조체
    [System.Serializable]
    public struct Wheel
    {
        public WheelCollider wheelCollider; // 물리용 WheelCollider
        public Transform wheelTransform;    // 시각적 바퀴 모델
        public float rotationSpeedMultiplier; // 회전 배율
    }

    // 앞뒤 바퀴들
    public Wheel frontLeftWheel;
    public Wheel frontRightWheel;
    public Wheel rearLeftWheel;
    public Wheel rearRightWheel;

    // 차량 파라미터
    public float motorTorque;             // 드리프트 중 출력을 유지하기 위해 상향
    public float brakeTorque;             // 드리프트 중 감속 완화
    public float maxSteerAngle;
    public float decelerationRate;
    public float maxSpeed;

    // 초기 위치 저장
    private Quaternion initialRotation;
    private Vector3 initialPosition;

    public bool isBraking;
    public float currentSpeed { get; private set; }

    private Rigidbody rigidBody;

    // 최소 속도 정의 (회전 처리용)
    private const float MinSpeedToRotateWheels = 0.1f;

    void Start()
    {
        Shared.Car = this;
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody 컴포넌트가 없습니다!");
            enabled = false;
        }

        initialRotation = transform.rotation;
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // 드리프트 조건: 브레이크 + 조향
        bool isDrifting = isBraking && Mathf.Abs(horizontalInput) > 0.1f;

        // 드리프트 중에도 충분한 엔진 토크 유지
        float engineTorqueMultiplier = isDrifting ? 0.6f : 1f;
        float adjustedMotorTorque = verticalInput * motorTorque * engineTorqueMultiplier;

        // 후륜 마찰 조정 및 브레이크 분기 처리 포함
        AdjustRearFrictionAndBraking(isDrifting);

        // 조향 감도 계산
        float baseSteerSensitivity = isDrifting ? 1.0f : 0.4f; // 일반 상태에서는 더 낮은 감도로 묵직하게 조향
        float steerSensitivity = Mathf.Lerp(baseSteerSensitivity, 1.0f, Mathf.Clamp01(currentSpeed / maxSpeed));
        float driftSteerFactor = isDrifting ? 1.25f : 1.0f;

        // 조향 각도 보간 처리
        float targetSteer = horizontalInput * maxSteerAngle * steerSensitivity * driftSteerFactor;
        float steerLerpSpeed = isDrifting ? 5f : 2.5f; // 일반 상태에서 더 천천히 조향 변화
        float steerAngle = Mathf.Lerp(frontLeftWheel.wheelCollider.steerAngle, targetSteer, Time.fixedDeltaTime * steerLerpSpeed);

        // 드리프트 중 빠르게 회전 시 감속
        if (isDrifting && Mathf.Abs(rigidBody.angularVelocity.y) > 1.0f)
        {
            Vector3 angular = rigidBody.angularVelocity;
            angular.y *= 0.9f;
            adjustedMotorTorque *= 0.9f;
            rigidBody.angularVelocity = angular;
        }

        // 조향 적용 (전륜 + 후륜 일부)
        ApplySteering(steerAngle, horizontalInput);

        // 엔진 토크 적용
        ApplyMotorTorque(adjustedMotorTorque);

        // 속도 계산
        currentSpeed = rigidBody.velocity.magnitude * 3.6f;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        // 자연 감속 처리
        if (verticalInput == 0 && !isBraking)
        {
            float decelerationAmount = decelerationRate * Time.fixedDeltaTime * (currentSpeed / maxSpeed);
            currentSpeed -= decelerationAmount;
            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }
    }

    void Update()
    {
        // 바퀴 회전 처리
        UpdateWheelPose(frontLeftWheel);
        UpdateWheelPose(frontRightWheel);
        UpdateWheelPose(rearLeftWheel);
        UpdateWheelPose(rearRightWheel);

        // 브레이크 입력 감지
        isBraking = Input.GetKey(KeyCode.Space);
    }

    // 모든 바퀴에 동일한 브레이크 토크 적용
    void SetBrakeTorque(float torque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = torque;
        frontRightWheel.wheelCollider.brakeTorque = torque;
        rearLeftWheel.wheelCollider.brakeTorque = torque;
        rearRightWheel.wheelCollider.brakeTorque = torque;
    }

    // 회전 애니메이션 적용
    public void UpdateWheelPose(Wheel wheel)
    {
        if (wheel.wheelCollider == null || wheel.wheelTransform == null)
            return;

        Vector3 pos;
        Quaternion rot;
        wheel.wheelCollider.GetWorldPose(out pos, out rot);

        wheel.wheelTransform.position = pos;
        wheel.wheelTransform.rotation = rot;
    }

    // 조향 각도 적용 함수
    void ApplySteering(float steerAngle, float horizontalInput)
    {
        frontLeftWheel.wheelCollider.steerAngle = steerAngle;
        frontRightWheel.wheelCollider.steerAngle = steerAngle;
        rearLeftWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * 0.2f;
        rearRightWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * 0.2f;
    }

    // 엔진 토크 일괄 적용
    void ApplyMotorTorque(float torque)
    {
        frontLeftWheel.wheelCollider.motorTorque = torque;
        frontRightWheel.wheelCollider.motorTorque = torque;
        rearLeftWheel.wheelCollider.motorTorque = torque;
        rearRightWheel.wheelCollider.motorTorque = torque;
    }

    // 드리프트 여부에 따라 후륜 마찰 및 브레이크 분기 처리
    void AdjustRearFrictionAndBraking(bool isDrifting)
    {
        WheelFrictionCurve frictionL = rearLeftWheel.wheelCollider.sidewaysFriction;
        WheelFrictionCurve frictionR;

        if (isDrifting)
        {
            // 더 강한 드리프트 효과를 위한 후륜 마찰 감소
            frictionL.stiffness = 2.0f;
            frictionL.extremumSlip = 0.3f;
            frictionL.asymptoteSlip = 0.5f;
            frictionL.asymptoteValue = 0.65f;

            // 후륜만 약하게 제동 적용
            ApplyBrakeForce(0f, brakeTorque * 0.2f);
        }
        else
        {
            // 일반 주행 시 더 높은 마찰값으로 미끄러짐 방지 강화
            frictionL.stiffness = 7.0f; // 기존보다 더 높여서 안정감 강화
            frictionL.extremumSlip = 0.04f;
            frictionL.asymptoteSlip = 0.15f;
            frictionL.asymptoteValue = 1.0f;

            // 일반 브레이크 (전후 동일)
            ApplyBrakeForce(isBraking ? brakeTorque : 0f, isBraking ? brakeTorque : 0f);
        }

        // 좌우 동일 적용
        frictionR = frictionL;
        rearLeftWheel.wheelCollider.sidewaysFriction = frictionL;
        rearRightWheel.wheelCollider.sidewaysFriction = frictionR;
    }

    // 전륜, 후륜 브레이크 분리 적용
    void ApplyBrakeForce(float frontTorque, float rearTorque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = frontTorque;
        frontRightWheel.wheelCollider.brakeTorque = frontTorque;
        rearLeftWheel.wheelCollider.brakeTorque = rearTorque;
        rearRightWheel.wheelCollider.brakeTorque = rearTorque;
    }

    // 차량 초기 위치/속도 리셋
    public void ResetVehicle()
    {
        SetWheelColliderEnabled(false);

        if (rigidBody != null)
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.isKinematic = true;
        }

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        StartCoroutine(ResetFullPhysics());
    }

    private IEnumerator ResetFullPhysics()
    {
        yield return new WaitForSeconds(0.1f);

        rigidBody.isKinematic = false;
        SetWheelColliderEnabled(true);

        SetBrakeTorque(0f);
        currentSpeed = 0;
    }

    // WheelCollider On/Off 제어
    private void SetWheelColliderEnabled(bool enabled)
    {
        frontLeftWheel.wheelCollider.enabled = enabled;
        frontRightWheel.wheelCollider.enabled = enabled;
        rearLeftWheel.wheelCollider.enabled = enabled;
        rearRightWheel.wheelCollider.enabled = enabled;
    }
}