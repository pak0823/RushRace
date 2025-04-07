using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Car : MonoBehaviour
{
    // ���� ������ ��� ����ü
    [System.Serializable]
    public struct Wheel
    {
        public WheelCollider wheelCollider;             // ���� ���� �� �ݶ��̴�
        public Transform wheelTransform;                // �ð��� �� ��
        public float rotationSpeedMultiplier;           // ȸ�� �ӵ� ����
    }

    // �� ���� ����
    public Wheel frontLeftWheel;
    public Wheel frontRightWheel;
    public Wheel rearLeftWheel;
    public Wheel rearRightWheel;

    // ���� ���� ���� ������
    public float motorTorque;                           // ���� ��ũ
    public float brakeTorque;                           // �극��ũ ��ũ
    public float maxSteerAngle;                         // �ִ� ���Ⱒ
    public float decelerationRate;                      // ���ӷ�
    public float maxSpeed;                              // �ְ� �ӵ�

    // �ʱ� ���� ����
    private Quaternion initialRotation;
    private Vector3 initialPosition;

    public bool isBraking;                              // �극��ũ �Է� ����
    public float currentSpeed { get; private set; }     // ���� �ӵ�

    private Rigidbody rigidBody;
    private const float MinSpeedToRotateWheels = 0.1f;  // �� ȸ�� �ּ� �ӵ� ����

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

        // �帮��Ʈ ����: �극��ũ + ���� �Է�
        bool isDrifting = isBraking && Mathf.Abs(horizontalInput) > 0.1f;

        // �帮��Ʈ �߿��� ��� �Ϻ� ����, �Ϲ� ���´� �ִ� ���
        float engineTorqueMultiplier = isDrifting ? 0.8f : 1f;
        float adjustedMotorTorque = verticalInput * motorTorque * engineTorqueMultiplier;

        // �ķ� ���� �� �극��ũ ó��
        AdjustRearFrictionAndBraking(isDrifting);

        // �ӵ� ��� ���� ���� ���
        float baseSteerSensitivity = isDrifting ? 1.0f : 0.2f; // �Ϲ� ������ �� �� �а��ϰ�
        float steerSensitivity = Mathf.Lerp(baseSteerSensitivity, 1.0f, Mathf.Clamp01(currentSpeed / maxSpeed));
        float driftSteerFactor = isDrifting ? 1.25f : 1.0f;

        // ��ǥ ���Ⱒ ��� �� ����
        float targetSteer = horizontalInput * maxSteerAngle * steerSensitivity * driftSteerFactor;
        float steerLerpSpeed = isDrifting ? 5f : 2.5f;
        float steerAngle = Mathf.Lerp(frontLeftWheel.wheelCollider.steerAngle, targetSteer, Time.fixedDeltaTime * steerLerpSpeed);

        // ���� ȸ�� ����
        if (isDrifting && Mathf.Abs(rigidBody.angularVelocity.y) > 1.0f)
        {
            Vector3 angular = rigidBody.angularVelocity;
            angular.y *= 0.9f;
            adjustedMotorTorque *= 0.9f;
            rigidBody.angularVelocity = angular;
        }

        // ����, ���� ��ũ ����
        ApplySteering(steerAngle, horizontalInput, isDrifting);
        ApplyMotorTorque(adjustedMotorTorque);

        // �ӵ� ���� (m/s �� km/h)
        currentSpeed = rigidBody.velocity.magnitude * 3.6f;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        // ���� ó��
        if (verticalInput == 0 && !isBraking)
        {
            float decelerationAmount = decelerationRate * Time.fixedDeltaTime * (currentSpeed / maxSpeed);
            currentSpeed -= decelerationAmount;
            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }

        //���� ���� ǥ�� ����
        WheelHit hit;
        if (frontLeftWheel.wheelCollider.GetGroundHit(out hit))
        {
            // hit.collider.sharedMaterial.name ���� �������� ���� ����
            if (hit.collider.sharedMaterial != null && hit.collider.sharedMaterial.name.Contains("Quad"))
            {
                adjustedMotorTorque *= 0.3f; // Road�� �ƴ� �� ��� 70% ����

                float slowdown = 20f * Time.fixedDeltaTime;//������ ����
                currentSpeed -= slowdown;
                currentSpeed = Mathf.Max(currentSpeed, 0f);

                // ������ ���ӵ� �ӵ��� Rigidbody�� �ݿ�
                Vector3 velocity = rigidBody.velocity;
                rigidBody.velocity = velocity.normalized * (currentSpeed / 3.6f);
            }
        }
    }

    void Update()
    {
        // ���� ȸ�� �ִϸ��̼� ������Ʈ
        UpdateWheelPose(frontLeftWheel);
        UpdateWheelPose(frontRightWheel);
        UpdateWheelPose(rearLeftWheel);
        UpdateWheelPose(rearRightWheel);

        // �극��ũ �Է� ó��
        isBraking = Input.GetKey(KeyCode.Space);
    }

    // �� ���� ��� �극��ũ ��ũ �ϰ� ����
    void SetBrakeTorque(float torque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = torque;
        frontRightWheel.wheelCollider.brakeTorque = torque;
        rearLeftWheel.wheelCollider.brakeTorque = torque;
        rearRightWheel.wheelCollider.brakeTorque = torque;
    }

    // �� ��ġ �� ȸ�� ����
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

    // ���� ���� ���� (������ steerAngle, �ķ��� ���� ����)
    void ApplySteering(float steerAngle, float horizontalInput, bool isDrifting)
    {
        frontLeftWheel.wheelCollider.steerAngle = steerAngle;
        frontRightWheel.wheelCollider.steerAngle = steerAngle;

        float rearSteerRatio = isDrifting ? 0.25f : 0.05f;
        rearLeftWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * rearSteerRatio;
        rearRightWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * rearSteerRatio;
    }

    // �� ���� ��� ���� ��ũ ����
    void ApplyMotorTorque(float torque)
    {
        frontLeftWheel.wheelCollider.motorTorque = torque;
        frontRightWheel.wheelCollider.motorTorque = torque;
        rearLeftWheel.wheelCollider.motorTorque = torque;
        rearRightWheel.wheelCollider.motorTorque = torque;
    }

    // �帮��Ʈ ���ο� ���� �ķ� ������ �� �극��ũ �и� ó��
    void AdjustRearFrictionAndBraking(bool isDrifting)
    {
        WheelFrictionCurve frictionL = rearLeftWheel.wheelCollider.sidewaysFriction;
        WheelFrictionCurve frictionR;

        if (isDrifting)
        {
            // �̲������� �����ϴ� ������ ����
            frictionL.stiffness = 2.0f;
            frictionL.extremumSlip = 0.3f;
            frictionL.asymptoteSlip = 0.5f;
            frictionL.asymptoteValue = 0.65f;

            // �ķ����� ���� �ҷ� ����
            ApplyBrakeForce(0f, brakeTorque * 0.2f);
        }
        else
        {
            // �Ϲ� ���¿����� ���� ������ ������ Ȯ��
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

    // ����/�ķ� �극��ũ ��ũ ���� ����
    void ApplyBrakeForce(float frontTorque, float rearTorque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = frontTorque;
        frontRightWheel.wheelCollider.brakeTorque = frontTorque;
        rearLeftWheel.wheelCollider.brakeTorque = rearTorque;
        rearRightWheel.wheelCollider.brakeTorque = rearTorque;
    }

    // ������ �ʱ� ��ġ�� ����
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

    // ���� ���� ���� ������
    private IEnumerator ResetFullPhysics()
    {
        yield return new WaitForSeconds(0.1f);

        rigidBody.isKinematic = false;
        SetWheelColliderEnabled(true);

        SetBrakeTorque(0f);
        currentSpeed = 0;
    }

    // �� �ݶ��̴� �ϰ� �ѱ�/���� - ���������� ����ϱ⿡ ������ ������ ���� �ʾ� �ʱ�ȭ �ϴ� ����� ����
    private void SetWheelColliderEnabled(bool enabled)
    {
        frontLeftWheel.wheelCollider.enabled = enabled;
        frontRightWheel.wheelCollider.enabled = enabled;
        rearLeftWheel.wheelCollider.enabled = enabled;
        rearRightWheel.wheelCollider.enabled = enabled;
    }
}
