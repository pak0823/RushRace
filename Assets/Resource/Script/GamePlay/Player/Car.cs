// Car.cs - 자동차 주행 및 드리프트, 사운드 이펙트 제어
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Car : MonoBehaviour
{
    [System.Serializable]
    public struct Wheel
    {
        public WheelCollider wheelCollider;
        public Transform wheelTransform;
        public float rotationSpeedMultiplier;
    }

    [Header("사운드")]
    public SoundData ENGINE_LOW;    //낮은 엔진 사운드
    public SoundData ENGINE_HIGH;   //높은 엔진 사운드
    public SoundData DRIFT;     //드리프트 사운드

    private bool isDriftSoundPlaying = false; // 드리프트 사운드 중복 방지 플래그

    [Header("이펙트 효과")]
    public ParticleSystem driftSmoke_RL;    //연기 효과 파티클 할당
    public ParticleSystem driftSmoke_RR;
    public TrailRenderer skidEffect_RL;  // 스키드 효과 파티클 할당
    public TrailRenderer skidEffect_RR;

    [Header("능력치 데이터")]
    public CarStats stats;
    public float motorTorque;
    public float brakeTorque;
    public float maxSteerAngle;
    public float decelerationRate;
    public float maxSpeed;
    public float driftFactor;

    public Wheel frontLeftWheel;
    public Wheel frontRightWheel;
    public Wheel rearLeftWheel;
    public Wheel rearRightWheel;

    private Quaternion initialRotation;
    private Vector3 initialPosition;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _isBraking;
    private bool _isDrifting;
    public float currentSpeed { get; private set; }

    private Rigidbody rigidBody;

    

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

        // stats 에 정의된 값들로 초기화
        motorTorque = stats.motorTorque;
        brakeTorque = stats.brakeTorque;
        maxSteerAngle = stats.maxSteerAngle;
        maxSpeed = stats.maxSpeed;
        decelerationRate = stats.decelerationRate;
        driftFactor = stats.driftFactor;

        frontLeftWheel.wheelCollider.ConfigureVehicleSubsteps(5f, 3, 10);
        frontRightWheel.wheelCollider.ConfigureVehicleSubsteps(5f, 3, 10);
        rearLeftWheel.wheelCollider.ConfigureVehicleSubsteps(5f, 3, 10);
        rearRightWheel.wheelCollider.ConfigureVehicleSubsteps(5f, 3, 10);

        rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        UpdateWheelPose(frontLeftWheel);
        UpdateWheelPose(frontRightWheel);
        UpdateWheelPose(rearLeftWheel);
        UpdateWheelPose(rearRightWheel);

        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
        _isBraking = Input.GetKey(KeyCode.Space);

        if (skidEffect_RL != null && skidEffect_RR != null)
        {
            skidEffect_RL.emitting = _isBraking;  // 브레이크 입력 시에만 스키드 효과 적용
            skidEffect_RR.emitting = _isBraking;
        }
        if (driftSmoke_RL != null && driftSmoke_RR != null)
        {
            // _isDrifting를 사용하여 드리프트 조건(브레이크와 좌우 입력의 조합)일 때만 연기 효과 발생
            if (_isDrifting)
            {
                if (!driftSmoke_RL.isPlaying) driftSmoke_RL.Play();
                if (!driftSmoke_RR.isPlaying) driftSmoke_RR.Play();
            }
            else
            {
                if (driftSmoke_RL.isPlaying) driftSmoke_RL.Stop();
                if (driftSmoke_RR.isPlaying) driftSmoke_RR.Stop();
            }
        }

        // 미션이 끝났거나 일시정지 중엔 엔진 이펙트/사운드 모두 중지
        if ((Shared.MissionManager != null && !Shared.MissionManager.IsMissionActive)
            || Time.timeScale == 0f)
        {
            Shared.SoundManager.StopLoopSound();
            return;
        }

        if (Time.timeScale != 0f)
        {
            if (currentSpeed > 1f)
                Shared.SoundManager.PlayLoopSound(ENGINE_LOW);
            else
                Shared.SoundManager.StopLoopSound();
        }
        else
            Shared.SoundManager.StopLoopSound();

    }

    private void FixedUpdate()
    {
        bool localDrift = _isBraking && Mathf.Abs(_horizontalInput) > 0.1f;
        _isDrifting = localDrift;
        float adjustedMotorTorque = _verticalInput * motorTorque * (_isDrifting ? 0.8f : 1f);

        AdjustRearFrictionAndBraking(localDrift);

        float baseSteerSensitivity = localDrift ? 1.0f : 0.2f;
        float steerSensitivity = Mathf.Lerp(baseSteerSensitivity, 1.0f, Mathf.Clamp01(currentSpeed / maxSpeed));
        float driftSteerFactor = localDrift ? driftFactor : 1.0f;

        float targetSteer = _horizontalInput * maxSteerAngle * steerSensitivity * driftSteerFactor;
        float steerLerpSpeed = localDrift ? 5f : 2.5f;
        float steerAngle = Mathf.Lerp(frontLeftWheel.wheelCollider.steerAngle, targetSteer, Time.fixedDeltaTime * steerLerpSpeed);

        if (localDrift && Mathf.Abs(rigidBody.angularVelocity.y) > 1.0f)
        {
            Vector3 angular = rigidBody.angularVelocity;
            angular.y *= 0.9f;
            adjustedMotorTorque *= 0.9f;
            rigidBody.angularVelocity = angular;
        }

        ApplySteering(steerAngle, _horizontalInput, localDrift);
        ApplyMotorTorque(adjustedMotorTorque);

        currentSpeed = rigidBody.velocity.magnitude * 3.6f;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        if (_verticalInput == 0 && !_isBraking)
        {
            float decelerationAmount = decelerationRate * Time.fixedDeltaTime * (currentSpeed / maxSpeed);
            currentSpeed -= decelerationAmount;
            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }

        WheelHit hit;
        if (frontLeftWheel.wheelCollider.GetGroundHit(out hit)) //지정된 길 이외의 길을 갈 경우
        {
            if (hit.collider.sharedMaterial != null && hit.collider.sharedMaterial.name.Contains("Quad"))
            {
                adjustedMotorTorque *= 0.5f;

                float slowdown = 20f * Time.fixedDeltaTime;
                currentSpeed -= slowdown;
                currentSpeed = Mathf.Max(currentSpeed, 0f);

                Vector3 velocity = rigidBody.velocity;
                rigidBody.velocity = velocity.normalized * (currentSpeed / 3.6f);
            }
        }
    }

    

    private void UpdateWheelPose(Wheel wheel)
    {
        if (wheel.wheelCollider == null || wheel.wheelTransform == null) return;

        Vector3 targetPos;
        Quaternion targetRot;
        wheel.wheelCollider.GetWorldPose(out targetPos, out targetRot);

        // 부드러운 보간 계수 (높일수록 빠르게)
        float smooth = Time.deltaTime * 10f;

        // 위치는 Lerp, 회전은 Slerp
        wheel.wheelTransform.position = Vector3.Lerp(
            wheel.wheelTransform.position,
            targetPos,
            smooth
        );
        wheel.wheelTransform.rotation = Quaternion.Slerp(
            wheel.wheelTransform.rotation,
            targetRot,
            smooth
        );
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
            if (!isDriftSoundPlaying)
            {
                Shared.SoundManager.PlaySound(DRIFT);
                isDriftSoundPlaying = true;
            }

            frictionL.stiffness = 2.0f;
            frictionL.extremumSlip = 0.3f;
            frictionL.asymptoteSlip = 0.5f;
            frictionL.asymptoteValue = 0.65f;

            ApplyBrakeForce(0f, brakeTorque * 0.2f);
        }
        else
        {
            isDriftSoundPlaying = false;

            frictionL.stiffness = 8.0f;
            frictionL.extremumSlip = 0.03f;
            frictionL.asymptoteSlip = 0.1f;
            frictionL.asymptoteValue = 1.0f;

            ApplyBrakeForce(_isBraking ? brakeTorque : 0f, _isBraking ? brakeTorque : 0f);
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
