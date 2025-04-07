using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Car : MonoBehaviour
{
    // 바퀴 정보를 담는 구조체
    [System.Serializable]
    public struct Wheel
    {
        public WheelCollider wheelCollider;             // 물리 계산용 휠 콜라이더
        public Transform wheelTransform;                // 시각적 휠 모델
        public float rotationSpeedMultiplier;           // 회전 속도 배율
    }

    // 각 바퀴 설정
    public Wheel frontLeftWheel;
    public Wheel frontRightWheel;
    public Wheel rearLeftWheel;
    public Wheel rearRightWheel;

    // 차량 주행 관련 설정값
    public float motorTorque;                           // 엔진 토크
    public float brakeTorque;                           // 브레이크 토크
    public float maxSteerAngle;                         // 최대 조향각
    public float decelerationRate;                      // 감속률
    public float maxSpeed;                              // 최고 속도

    // 초기 상태 저장
    private Quaternion initialRotation;
    private Vector3 initialPosition;

    public bool isBraking;                              // 브레이크 입력 상태
    public float currentSpeed { get; private set; }     // 현재 속도

    private Rigidbody rigidBody;
    private const float MinSpeedToRotateWheels = 0.1f;  // 휠 회전 최소 속도 기준

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

        // 드리프트 조건: 브레이크 + 조향 입력
        bool isDrifting = isBraking && Mathf.Abs(horizontalInput) > 0.1f;

        // 드리프트 중에는 출력 일부 유지, 일반 상태는 최대 출력
        float engineTorqueMultiplier = isDrifting ? 0.8f : 1f;
        float adjustedMotorTorque = verticalInput * motorTorque * engineTorqueMultiplier;

        // 후륜 마찰 및 브레이크 처리
        AdjustRearFrictionAndBraking(isDrifting);

        // 속도 기반 조향 감도 계산
        float baseSteerSensitivity = isDrifting ? 1.0f : 0.2f; // 일반 상태일 때 더 둔감하게
        float steerSensitivity = Mathf.Lerp(baseSteerSensitivity, 1.0f, Mathf.Clamp01(currentSpeed / maxSpeed));
        float driftSteerFactor = isDrifting ? 1.25f : 1.0f;

        // 목표 조향각 계산 및 보간
        float targetSteer = horizontalInput * maxSteerAngle * steerSensitivity * driftSteerFactor;
        float steerLerpSpeed = isDrifting ? 5f : 2.5f;
        float steerAngle = Mathf.Lerp(frontLeftWheel.wheelCollider.steerAngle, targetSteer, Time.fixedDeltaTime * steerLerpSpeed);

        // 빠른 회전 억제
        if (isDrifting && Mathf.Abs(rigidBody.angularVelocity.y) > 1.0f)
        {
            Vector3 angular = rigidBody.angularVelocity;
            angular.y *= 0.9f;
            adjustedMotorTorque *= 0.9f;
            rigidBody.angularVelocity = angular;
        }

        // 조향, 구동 토크 적용
        ApplySteering(steerAngle, horizontalInput, isDrifting);
        ApplyMotorTorque(adjustedMotorTorque);

        // 속도 갱신 (m/s → km/h)
        currentSpeed = rigidBody.velocity.magnitude * 3.6f;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        // 감속 처리
        if (verticalInput == 0 && !isBraking)
        {
            float decelerationAmount = decelerationRate * Time.fixedDeltaTime * (currentSpeed / maxSpeed);
            currentSpeed -= decelerationAmount;
            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }

        //바퀴 접촉 표면 감지
        WheelHit hit;
        if (frontLeftWheel.wheelCollider.GetGroundHit(out hit))
        {
            // hit.collider.sharedMaterial.name 등을 기준으로 감속 적용
            if (hit.collider.sharedMaterial != null && hit.collider.sharedMaterial.name.Contains("Quad"))
            {
                adjustedMotorTorque *= 0.3f; // Road가 아닐 시 출력 70% 감소

                float slowdown = 20f * Time.fixedDeltaTime;//강제로 감속
                currentSpeed -= slowdown;
                currentSpeed = Mathf.Max(currentSpeed, 0f);

                // 강제로 감속된 속도를 Rigidbody에 반영
                Vector3 velocity = rigidBody.velocity;
                rigidBody.velocity = velocity.normalized * (currentSpeed / 3.6f);
            }
        }
    }

    void Update()
    {
        // 바퀴 회전 애니메이션 업데이트
        UpdateWheelPose(frontLeftWheel);
        UpdateWheelPose(frontRightWheel);
        UpdateWheelPose(rearLeftWheel);
        UpdateWheelPose(rearRightWheel);

        // 브레이크 입력 처리
        isBraking = Input.GetKey(KeyCode.Space);
    }

    // 네 바퀴 모두 브레이크 토크 일괄 설정
    void SetBrakeTorque(float torque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = torque;
        frontRightWheel.wheelCollider.brakeTorque = torque;
        rearLeftWheel.wheelCollider.brakeTorque = torque;
        rearRightWheel.wheelCollider.brakeTorque = torque;
    }

    // 휠 위치 및 회전 보간
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

    // 조향 각도 적용 (전륜은 steerAngle, 후륜은 비율 조정)
    void ApplySteering(float steerAngle, float horizontalInput, bool isDrifting)
    {
        frontLeftWheel.wheelCollider.steerAngle = steerAngle;
        frontRightWheel.wheelCollider.steerAngle = steerAngle;

        float rearSteerRatio = isDrifting ? 0.25f : 0.05f;
        rearLeftWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * rearSteerRatio;
        rearRightWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * rearSteerRatio;
    }

    // 네 바퀴 모두 구동 토크 적용
    void ApplyMotorTorque(float torque)
    {
        frontLeftWheel.wheelCollider.motorTorque = torque;
        frontRightWheel.wheelCollider.motorTorque = torque;
        rearLeftWheel.wheelCollider.motorTorque = torque;
        rearRightWheel.wheelCollider.motorTorque = torque;
    }

    // 드리프트 여부에 따른 후륜 마찰력 및 브레이크 분리 처리
    void AdjustRearFrictionAndBraking(bool isDrifting)
    {
        WheelFrictionCurve frictionL = rearLeftWheel.wheelCollider.sidewaysFriction;
        WheelFrictionCurve frictionR;

        if (isDrifting)
        {
            // 미끄러짐을 유도하는 마찰값 설정
            frictionL.stiffness = 2.0f;
            frictionL.extremumSlip = 0.3f;
            frictionL.asymptoteSlip = 0.5f;
            frictionL.asymptoteValue = 0.65f;

            // 후륜에만 제동 소량 적용
            ApplyBrakeForce(0f, brakeTorque * 0.2f);
        }
        else
        {
            // 일반 상태에서는 높은 마찰로 안정성 확보
            frictionL.stiffness = 8.0f;
            frictionL.extremumSlip = 0.03f;
            frictionL.asymptoteSlip = 0.1f;
            frictionL.asymptoteValue = 1.0f;

            ApplyBrakeForce(isBraking ? brakeTorque : 0f, isBraking ? brakeTorque : 0f);
        }

        frictionR = frictionL;
        rearLeftWheel.wheelCollider.sidewaysFriction = frictionL;
        rearRightWheel.wheelCollider.sidewaysFriction = frictionR;
    }

    // 전륜/후륜 브레이크 토크 개별 적용
    void ApplyBrakeForce(float frontTorque, float rearTorque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = frontTorque;
        frontRightWheel.wheelCollider.brakeTorque = frontTorque;
        rearLeftWheel.wheelCollider.brakeTorque = rearTorque;
        rearRightWheel.wheelCollider.brakeTorque = rearTorque;
    }

    // 차량을 초기 위치로 리셋
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

    // 리셋 직후 물리 재적용
    private IEnumerator ResetFullPhysics()
    {
        yield return new WaitForSeconds(0.1f);

        rigidBody.isKinematic = false;
        SetWheelColliderEnabled(true);

        SetBrakeTorque(0f);
        currentSpeed = 0;
    }

    // 휠 콜라이더 일괄 켜기/끄기 - 물리엔진을 사용하기에 완전히 정지가 되지 않아 초기화 하는 방식을 택함
    private void SetWheelColliderEnabled(bool enabled)
    {
        frontLeftWheel.wheelCollider.enabled = enabled;
        frontRightWheel.wheelCollider.enabled = enabled;
        rearLeftWheel.wheelCollider.enabled = enabled;
        rearRightWheel.wheelCollider.enabled = enabled;
    }
}
