using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    bool isBraking = false;
    bool wasDrifting = false;
    public float currentSpeed { get; private set; }

    private Rigidbody rigidBody;
    private const float MinSpeedToRotateWheels = 0.1f;

    // 이펙트 시스템
    public bool useEffects = false;
    public ParticleSystem RLWParticleSystem;
    public ParticleSystem RRWParticleSystem;
    public TrailRenderer RLWTireSkid;
    public TrailRenderer RRWTireSkid;

    // 사운드 관련
    private Dictionary<string, SoundData> soundDict;
    public SoundData ENGINE_LOW;
    public SoundData ENGINE_HIGH;
    public SoundData DRIFT;

    enum EngineSoundState { None, Low, High }
    EngineSoundState engineState = EngineSoundState.None;

    private void Awake()
    {
        soundDict = new Dictionary<string, SoundData>
        {
            { "DRIFT", DRIFT },
            { "ENGINE_LOW", ENGINE_LOW },
            { "ENGINE_HIGH", ENGINE_HIGH }
        };
    }

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

        if (currentSpeed > 5 && currentSpeed < 110)
        {
            if (engineState != EngineSoundState.Low)
            {
                Shared.SoundManager.PlayLoopSound(ENGINE_LOW);
                engineState = EngineSoundState.Low;
            }
        }
        else if (currentSpeed >= 110)
        {
            if (engineState != EngineSoundState.High)
            {
                Shared.SoundManager.PlayLoopSound(ENGINE_HIGH);
                engineState = EngineSoundState.High;
            }
        }
        else
        {
            if (engineState != EngineSoundState.None)
            {
                Shared.SoundManager.StopLoopSound();
                engineState = EngineSoundState.None;
            }
        }

        if (isDrifting && !wasDrifting)
        {
            PlaySFXSound("DRIFT");
        }

        // 스키드 이펙트 활성화 여부 체크
        bool shouldEmitSkid = isDrifting && ShouldEmitSkid();
        DriftEffect(shouldEmitSkid);

        wasDrifting = isDrifting;
    }

    void Update()
    {
        UpdateWheelPose(frontLeftWheel);
        UpdateWheelPose(frontRightWheel);
        UpdateWheelPose(rearLeftWheel);
        UpdateWheelPose(rearRightWheel);

        isBraking = Input.GetKey(KeyCode.Space);
    }

    void SetBrakeTorque(float torque)
    {
        frontLeftWheel.wheelCollider.brakeTorque = torque;
        frontRightWheel.wheelCollider.brakeTorque = torque;
        rearLeftWheel.wheelCollider.brakeTorque = torque;
        rearRightWheel.wheelCollider.brakeTorque = torque;
    }

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

    void ApplySteering(float steerAngle, float horizontalInput, bool isDrifting)
    {
        frontLeftWheel.wheelCollider.steerAngle = steerAngle;
        frontRightWheel.wheelCollider.steerAngle = steerAngle;

        float rearSteerRatio = isDrifting ? 0.25f : 0.05f;
        rearLeftWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * rearSteerRatio;
        rearRightWheel.wheelCollider.steerAngle = horizontalInput * maxSteerAngle * rearSteerRatio;
    }

    void ApplyMotorTorque(float torque)
    {
        frontLeftWheel.wheelCollider.motorTorque = torque;
        frontRightWheel.wheelCollider.motorTorque = torque;
        rearLeftWheel.wheelCollider.motorTorque = torque;
        rearRightWheel.wheelCollider.motorTorque = torque;
    }

    void AdjustRearFrictionAndBraking(bool isDrifting)
    {
        WheelFrictionCurve frictionL = rearLeftWheel.wheelCollider.sidewaysFriction;
        WheelFrictionCurve frictionR;

        if (isDrifting)
        {
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

    void ApplyBrakeForce(float frontTorque, float rearTorque)
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

    private IEnumerator ResetFullPhysics()
    {
        yield return new WaitForSeconds(0.1f);

        rigidBody.isKinematic = false;
        SetWheelColliderEnabled(true);

        SetBrakeTorque(0f);
        currentSpeed = 0;
    }

    private void SetWheelColliderEnabled(bool enabled)
    {
        frontLeftWheel.wheelCollider.enabled = enabled;
        frontRightWheel.wheelCollider.enabled = enabled;
        rearLeftWheel.wheelCollider.enabled = enabled;
        rearRightWheel.wheelCollider.enabled = enabled;
    }

    void PlaySFXSound(string _key)
    {
        if (soundDict.TryGetValue(_key, out SoundData data))
        {
            Shared.SoundManager.PlaySound(data);
        }
    }

    // 드리프트 시 스키드 이펙트와 파티클 실행 여부
    void DriftEffect(bool _emit)
    {
        if (useEffects)
        {
            if (_emit)
            {
                RLWParticleSystem?.Play();
                RRWParticleSystem?.Play();
            }
            else
            {
                RLWParticleSystem?.Stop();
                RRWParticleSystem?.Stop();
            }
            RLWTireSkid.emitting = _emit;
            RRWTireSkid.emitting = _emit;
        }
    }

    // 실제 슬립 발생 여부 확인
    bool ShouldEmitSkid()
    {
        WheelHit hit;
        if (rearLeftWheel.wheelCollider.GetGroundHit(out hit))
        {
            return Mathf.Abs(hit.sidewaysSlip) > 0.25f || Mathf.Abs(hit.forwardSlip) > 0.25f;
        }
        return false;
    }
}
