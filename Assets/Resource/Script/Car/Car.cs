using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Car : MonoBehaviour
{
    [System.Serializable] // Inspector 창에서 보기 위해 추가
    public struct Wheel
    {
        public WheelCollider wheelCollider;
        public Transform wheelTransform; // 바퀴 모델 Transform
        public float rotationSpeedMultiplier; // 바퀴 회전 속도 배율
    }

    public Wheel frontLeftWheel;
    public Wheel frontRightWheel;
    public Wheel rearLeftWheel;
    public Wheel rearRightWheel;

    public float motorTorque = 300f; // 엔진 토크
    public float brakeTorque = 500f; // 브레이크 힘
    public float maxSteerAngle = 20f; // 최대 스티어링 각도
    public float decelerationRate = 100f; // 감속 속도 (km/h/s)
    public float maxSpeed;


    private Quaternion initialRotation; //기본 회전값
    private Vector3 initialPosition;    //기본 위치값

    //public float downforceAtSpeed = 100f; // 최대 속도에서 다운포스 (N)

    public float currentSpeed { get; private set; } // 현재 속도 (읽기 전용)

    private Rigidbody rigidBody; // Rigidbody 컴포넌트

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody 컴포넌트가 없습니다!");
            enabled = false; // 스크립트 비활성화
        }

        initialRotation = transform.rotation;
        initialPosition = transform.position;

    }

    void FixedUpdate()
    {
        // 입력 받기
        float verticalInput = Input.GetAxis("Vertical"); // 전진/후진 (W/S 또는 위/아래 화살표)
        float horizontalInput = Input.GetAxis("Horizontal"); // 좌/우 (A/D 또는 좌/우 화살표)
        bool isBraking = Input.GetKey(KeyCode.Space); // 스페이스바 누르면 브레이크

        //float speedFactor = Mathf.Clamp01(rigidBody.velocity.magnitude / maxSpeed);
        //Vector3 downforce = Vector3.down * downforceAtSpeed * speedFactor;
        //rigidBody.AddForce(downforce);


        // 엔진 토크 적용
        frontLeftWheel.wheelCollider.motorTorque = verticalInput * motorTorque;
        frontRightWheel.wheelCollider.motorTorque = verticalInput * motorTorque;
        rearLeftWheel.wheelCollider.motorTorque = verticalInput * motorTorque;
        rearRightWheel.wheelCollider.motorTorque = verticalInput * motorTorque;

        // 스티어링 적용
        frontLeftWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle;
        frontRightWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle;

        // 브레이크 적용
        SetBrakeTorque(isBraking ? brakeTorque : 0f); // 브레이크 시 brakeTorque, 아니면 0

        // 현재 속도 계산 (km/h)
        currentSpeed = rigidBody.velocity.magnitude * 3.6f; // m/s -> km/h
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed); // 최대 속도 제한

        // 감속 로직 (브레이크 시 비활성화)
        if (verticalInput == 0 && !isBraking)
        {
            // 현재 속도에 따라 감속량 조절
            float decelerationAmount = decelerationRate * Time.fixedDeltaTime * (currentSpeed / maxSpeed);
            currentSpeed -= decelerationAmount;
            currentSpeed = Mathf.Max(currentSpeed, 0f); // 최소 속도 0
        }

        //Debug.Log((int)currentSpeed);
    }

    void Update()
    {
        // 바퀴 회전
        RotateWheel(frontLeftWheel);
        RotateWheel(frontRightWheel);
        RotateWheel(rearLeftWheel);
        RotateWheel(rearRightWheel);

        // R 키를 누르면 차량 초기화
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetVehicle();
        }
    }

    // 브레이크 토크 설정 함수
    void SetBrakeTorque(float torque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = torque;
        frontRightWheel.wheelCollider.brakeTorque = torque;
        rearLeftWheel.wheelCollider.brakeTorque = torque;
        rearRightWheel.wheelCollider.brakeTorque = torque;
    }

    // 바퀴 회전 함수
    void RotateWheel(Wheel wheel)
    {
        if (wheel.wheelCollider != null && wheel.wheelTransform != null)
        {
            float maxRotationSpeed = 1000f;
            float rotationAngle = Mathf.Clamp(wheel.wheelCollider.rpm * 6 * Time.deltaTime * wheel.rotationSpeedMultiplier, -maxRotationSpeed * Time.deltaTime, maxRotationSpeed * Time.deltaTime);
            wheel.wheelTransform.Rotate(rotationAngle, 0, 0);
        }
    }

    void ResetVehicle()
    {
        // 위치와 회전 초기화
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        //Rigidbody 속도 초기화
        if (rigidBody != null)
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }
    }
}
