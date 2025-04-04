using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Car : MonoBehaviour
{
    // ���� ���� ����ü
    [System.Serializable]
    public struct Wheel
    {
        public WheelCollider wheelCollider; // ������ WheelCollider
        public Transform wheelTransform;    // �ð��� ���� ��
        public float rotationSpeedMultiplier; // ȸ�� ����
    }

    // �յ� ������
    public Wheel frontLeftWheel;
    public Wheel frontRightWheel;
    public Wheel rearLeftWheel;
    public Wheel rearRightWheel;

    // ���� �Ķ����
    public float motorTorque;             // �帮��Ʈ �� ����� �����ϱ� ���� ����
    public float brakeTorque;             // �帮��Ʈ �� ���� ��ȭ
    public float maxSteerAngle;
    public float decelerationRate;
    public float maxSpeed;

    // �ʱ� ��ġ ����
    private Quaternion initialRotation;
    private Vector3 initialPosition;

    public bool isBraking;
    public float currentSpeed { get; private set; }

    private Rigidbody rigidBody;

    // �ּ� �ӵ� ���� (ȸ�� ó����)
    private const float MinSpeedToRotateWheels = 0.1f;

    void Start()
    {
        Shared.Car = this;
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody ������Ʈ�� �����ϴ�!");
            enabled = false;
        }

        initialRotation = transform.rotation;
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // �帮��Ʈ ����: �극��ũ + ����
        bool isDrifting = isBraking && Mathf.Abs(horizontalInput) > 0.1f;

        // �帮��Ʈ �߿��� ����� ���� ��ũ ����
        float engineTorqueMultiplier = isDrifting ? 0.6f : 1f;
        float adjustedMotorTorque = verticalInput * motorTorque * engineTorqueMultiplier;

        // �ķ� ���� ���� �� �극��ũ �б� ó�� ����
        AdjustRearFrictionAndBraking(isDrifting);

        // ���� ���� ���
        float baseSteerSensitivity = isDrifting ? 1.0f : 0.4f; // �Ϲ� ���¿����� �� ���� ������ �����ϰ� ����
        float steerSensitivity = Mathf.Lerp(baseSteerSensitivity, 1.0f, Mathf.Clamp01(currentSpeed / maxSpeed));
        float driftSteerFactor = isDrifting ? 1.25f : 1.0f;

        // ���� ���� ���� ó��
        float targetSteer = horizontalInput * maxSteerAngle * steerSensitivity * driftSteerFactor;
        float steerLerpSpeed = isDrifting ? 5f : 2.5f; // �Ϲ� ���¿��� �� õõ�� ���� ��ȭ
        float steerAngle = Mathf.Lerp(frontLeftWheel.wheelCollider.steerAngle, targetSteer, Time.fixedDeltaTime * steerLerpSpeed);

        // �帮��Ʈ �� ������ ȸ�� �� ����
        if (isDrifting && Mathf.Abs(rigidBody.angularVelocity.y) > 1.0f)
        {
            Vector3 angular = rigidBody.angularVelocity;
            angular.y *= 0.9f;
            adjustedMotorTorque *= 0.9f;
            rigidBody.angularVelocity = angular;
        }

        // ���� ���� (���� + �ķ� �Ϻ�)
        ApplySteering(steerAngle, horizontalInput);

        // ���� ��ũ ����
        ApplyMotorTorque(adjustedMotorTorque);

        // �ӵ� ���
        currentSpeed = rigidBody.velocity.magnitude * 3.6f;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        // �ڿ� ���� ó��
        if (verticalInput == 0 && !isBraking)
        {
            float decelerationAmount = decelerationRate * Time.fixedDeltaTime * (currentSpeed / maxSpeed);
            currentSpeed -= decelerationAmount;
            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }
    }

    void Update()
    {
        // ���� ȸ�� ó��
        UpdateWheelPose(frontLeftWheel);
        UpdateWheelPose(frontRightWheel);
        UpdateWheelPose(rearLeftWheel);
        UpdateWheelPose(rearRightWheel);

        // �극��ũ �Է� ����
        isBraking = Input.GetKey(KeyCode.Space);
    }

    // ��� ������ ������ �극��ũ ��ũ ����
    void SetBrakeTorque(float torque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = torque;
        frontRightWheel.wheelCollider.brakeTorque = torque;
        rearLeftWheel.wheelCollider.brakeTorque = torque;
        rearRightWheel.wheelCollider.brakeTorque = torque;
    }

    // ȸ�� �ִϸ��̼� ����
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

    // ���� ���� ���� �Լ�
    void ApplySteering(float steerAngle, float horizontalInput)
    {
        frontLeftWheel.wheelCollider.steerAngle = steerAngle;
        frontRightWheel.wheelCollider.steerAngle = steerAngle;
        rearLeftWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * 0.2f;
        rearRightWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * 0.2f;
    }

    // ���� ��ũ �ϰ� ����
    void ApplyMotorTorque(float torque)
    {
        frontLeftWheel.wheelCollider.motorTorque = torque;
        frontRightWheel.wheelCollider.motorTorque = torque;
        rearLeftWheel.wheelCollider.motorTorque = torque;
        rearRightWheel.wheelCollider.motorTorque = torque;
    }

    // �帮��Ʈ ���ο� ���� �ķ� ���� �� �극��ũ �б� ó��
    void AdjustRearFrictionAndBraking(bool isDrifting)
    {
        WheelFrictionCurve frictionL = rearLeftWheel.wheelCollider.sidewaysFriction;
        WheelFrictionCurve frictionR;

        if (isDrifting)
        {
            // �� ���� �帮��Ʈ ȿ���� ���� �ķ� ���� ����
            frictionL.stiffness = 2.0f;
            frictionL.extremumSlip = 0.3f;
            frictionL.asymptoteSlip = 0.5f;
            frictionL.asymptoteValue = 0.65f;

            // �ķ��� ���ϰ� ���� ����
            ApplyBrakeForce(0f, brakeTorque * 0.2f);
        }
        else
        {
            // �Ϲ� ���� �� �� ���� ���������� �̲����� ���� ��ȭ
            frictionL.stiffness = 7.0f; // �������� �� ������ ������ ��ȭ
            frictionL.extremumSlip = 0.04f;
            frictionL.asymptoteSlip = 0.15f;
            frictionL.asymptoteValue = 1.0f;

            // �Ϲ� �극��ũ (���� ����)
            ApplyBrakeForce(isBraking ? brakeTorque : 0f, isBraking ? brakeTorque : 0f);
        }

        // �¿� ���� ����
        frictionR = frictionL;
        rearLeftWheel.wheelCollider.sidewaysFriction = frictionL;
        rearRightWheel.wheelCollider.sidewaysFriction = frictionR;
    }

    // ����, �ķ� �극��ũ �и� ����
    void ApplyBrakeForce(float frontTorque, float rearTorque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = frontTorque;
        frontRightWheel.wheelCollider.brakeTorque = frontTorque;
        rearLeftWheel.wheelCollider.brakeTorque = rearTorque;
        rearRightWheel.wheelCollider.brakeTorque = rearTorque;
    }

    // ���� �ʱ� ��ġ/�ӵ� ����
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

    // WheelCollider On/Off ����
    private void SetWheelColliderEnabled(bool enabled)
    {
        frontLeftWheel.wheelCollider.enabled = enabled;
        frontRightWheel.wheelCollider.enabled = enabled;
        rearLeftWheel.wheelCollider.enabled = enabled;
        rearRightWheel.wheelCollider.enabled = enabled;
    }
}