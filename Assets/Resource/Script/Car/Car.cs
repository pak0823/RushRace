// Car.cs - 자동차 주행 및 드리프트, 사운드 이펙트 제어

using UnityEngine;

public partial class Car : MonoBehaviour
{
    [System.Serializable]
    public struct Wheel
    {
        public WheelCollider wheelCollider;
        public Transform wheelTransform;
        public float rotationSpeedMultiplier;
    }

    public Wheel frontLeftWheel;
    public Wheel frontRightWheel;
    public Wheel rearLeftWheel;
    public Wheel rearRightWheel;

    public float motorTorque;
    public float brakeTorque;
    public float maxSteerAngle;
    public float decelerationRate;
    public float maxSpeed;

    private Quaternion initialRotation;
    private Vector3 initialPosition;

    public bool isBraking;
    public float currentSpeed { get; private set; }

    private Rigidbody rigidBody;
    private const float MinSpeedToRotateWheels = 0.1f;

    public SoundData ENGINE_LOW;
    public SoundData ENGINE_HIGH;
    public SoundData DRIFT;

    private void Start()
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

    private void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        bool isDrifting = isBraking && Mathf.Abs(horizontalInput) > 0.1f;

        float engineTorqueMultiplier = isDrifting ? 0.8f : 1f;
        float adjustedMotorTorque = verticalInput * motorTorque * engineTorqueMultiplier;

        AdjustRearFrictionAndBraking(isDrifting);

        float baseSteerSensitivity = isDrifting ? 1.0f : 0.2f;
        float steerSensitivity = Mathf.Lerp(baseSteerSensitivity, 1.0f, Mathf.Clamp01(currentSpeed / maxSpeed));
        float driftSteerFactor = isDrifting ? 1.25f : 1.0f;

        float targetSteer = horizontalInput * maxSteerAngle * steerSensitivity * driftSteerFactor;
        float steerLerpSpeed = isDrifting ? 5f : 2.5f;
        float steerAngle = Mathf.Lerp(frontLeftWheel.wheelCollider.steerAngle, targetSteer, Time.fixedDeltaTime * steerLerpSpeed);

        if (isDrifting && Mathf.Abs(rigidBody.angularVelocity.y) > 1.0f)
        {
            Vector3 angular = rigidBody.angularVelocity;
            angular.y *= 0.9f;
            adjustedMotorTorque *= 0.9f;
            rigidBody.angularVelocity = angular;
        }

        ApplySteering(steerAngle, horizontalInput, isDrifting);
        ApplyMotorTorque(adjustedMotorTorque);

        currentSpeed = rigidBody.velocity.magnitude * 3.6f;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        if (verticalInput == 0 && !isBraking)
        {
            float decelerationAmount = decelerationRate * Time.fixedDeltaTime * (currentSpeed / maxSpeed);
            currentSpeed -= decelerationAmount;
            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }

        WheelHit hit;
        if (frontLeftWheel.wheelCollider.GetGroundHit(out hit))
        {
            if (hit.collider.sharedMaterial != null && hit.collider.sharedMaterial.name.Contains("Quad"))
            {
                adjustedMotorTorque *= 0.3f;

                float slowdown = 20f * Time.fixedDeltaTime;
                currentSpeed -= slowdown;
                currentSpeed = Mathf.Max(currentSpeed, 0f);

                Vector3 velocity = rigidBody.velocity;
                rigidBody.velocity = velocity.normalized * (currentSpeed / 3.6f);
            }
        }
    }

    private void Update()
    {
        UpdateWheelPose(frontLeftWheel);
        UpdateWheelPose(frontRightWheel);
        UpdateWheelPose(rearLeftWheel);
        UpdateWheelPose(rearRightWheel);

        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void UpdateWheelPose(Wheel wheel)
    {
        if (wheel.wheelCollider == null || wheel.wheelTransform == null) return;

        Vector3 pos;
        Quaternion rot;
        wheel.wheelCollider.GetWorldPose(out pos, out rot);

        wheel.wheelTransform.position = pos;
        wheel.wheelTransform.rotation = rot;
    }

    private void ApplySteering(float steerAngle, float horizontalInput, bool isDrifting)
    {
        frontLeftWheel.wheelCollider.steerAngle = steerAngle;
        frontRightWheel.wheelCollider.steerAngle = steerAngle;

        float rearSteerRatio = isDrifting ? 0.25f : 0.05f;
        rearLeftWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * rearSteerRatio;
        rearRightWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * rearSteerRatio;
    }

    private void ApplyMotorTorque(float torque)
    {
        frontLeftWheel.wheelCollider.motorTorque = torque;
        frontRightWheel.wheelCollider.motorTorque = torque;
        rearLeftWheel.wheelCollider.motorTorque = torque;
        rearRightWheel.wheelCollider.motorTorque = torque;
    }

    private void AdjustRearFrictionAndBraking(bool isDrifting)
    {
        WheelFrictionCurve frictionL = rearLeftWheel.wheelCollider.sidewaysFriction;
        WheelFrictionCurve frictionR;

        if (isDrifting)
        {
            Shared.SoundManager.PlaySound(DRIFT);

            frictionL.stiffness = 2.0f;
            frictionL.extremumSlip = 0.3f;
            frictionL.asymptoteSlip = 0.5f;
            frictionL.asymptoteValue = 0.65f;

            ApplyBrakeForce(0f, brakeTorque * 0.2f);
        }
        else
        {
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

    private void ApplyBrakeForce(float frontTorque, float rearTorque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = frontTorque;
        frontRightWheel.wheelCollider.brakeTorque = frontTorque;
        rearLeftWheel.wheelCollider.brakeTorque = rearTorque;
        rearRightWheel.wheelCollider.brakeTorque = rearTorque;
    }

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

    private System.Collections.IEnumerator ResetFullPhysics()
    {
        yield return new WaitForSeconds(0.1f);

        rigidBody.isKinematic = false;
        SetWheelColliderEnabled(true);

        SetBrakeTorque(0f);
        currentSpeed = 0;
    }

    private void SetBrakeTorque(float torque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = torque;
        frontRightWheel.wheelCollider.brakeTorque = torque;
        rearLeftWheel.wheelCollider.brakeTorque = torque;
        rearRightWheel.wheelCollider.brakeTorque = torque;
    }

    private void SetWheelColliderEnabled(bool enabled)
    {
        frontLeftWheel.wheelCollider.enabled = enabled;
        frontRightWheel.wheelCollider.enabled = enabled;
        rearLeftWheel.wheelCollider.enabled = enabled;
        rearRightWheel.wheelCollider.enabled = enabled;
    }
}
