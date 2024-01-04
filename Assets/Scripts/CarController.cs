using System;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody playerRB;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;

    public float gasInput;
    public float brakeInput;
    public float steeringInput;

    public float motorPower;
    public float brakePower;

    private float speed;
    public AnimationCurve steeringCurve;

    public MyButton gasPedal;
    public MyButton brakePedal;

    private float gyroSensitivity = 5.0f; // Adjust this value based on gyro sensitivity
    private bool gyroEnabled;
    private Gyroscope gyro;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();

        // Check if gyro is available
        gyroEnabled = SystemInfo.supportsGyroscope;

        if (gyroEnabled)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
        else
        {
            Debug.LogError("Gyroscope not supported on this device");
        }
    }

    void Update()
    {
        speed = playerRB.velocity.magnitude;
        CheckInput();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();
        ApplYWheelPositions();
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        if (gasPedal.isPressed)
        {
            gasInput += gasPedal.dampenPress;
        }
        if (brakePedal.isPressed)
        {
            gasInput -= brakePedal.dampenPress;
        }
        
        steeringInput = Input.acceleration.x;
        

        float movingDirection = Vector3.Dot(transform.forward, playerRB.velocity);
        if (movingDirection < -0.5f && gasInput > 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        else if (movingDirection > 0.5f && gasInput < 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        else
        {
            brakeInput = 0;
        }
    }

    
    void ApplyBrake()
    {
        colliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f;

        colliders.BR2Wheel.brakeTorque = brakeInput * brakePower ;
        colliders.BL2Wheel.brakeTorque = brakeInput * brakePower ;


    }
    void ApplyMotor()
    {

        colliders.BR2Wheel.motorTorque = motorPower * gasInput;
        colliders.BL2Wheel.motorTorque = motorPower * gasInput;

    }
    void ApplySteering()
    {

        float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);

        steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
        colliders.FRWheel.steerAngle = steeringAngle;
        colliders.FLWheel.steerAngle = steeringAngle;
    }
    void ApplYWheelPositions()
    {
        UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(colliders.BL1Wheel, wheelMeshes.BL1Wheel);
        UpdateWheel(colliders.BR1Wheel, wheelMeshes.BR1Wheel);
        UpdateWheel(colliders.BL2Wheel, wheelMeshes.BL2Wheel); 
        UpdateWheel(colliders.BR2Wheel, wheelMeshes.BR2Wheel);
    }
    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat * Quaternion.Euler(0, 90 , 0);
    }
}
[System.Serializable]
public class WheelColliders
{
    public WheelCollider FLWheel;
    public WheelCollider FRWheel;
    public WheelCollider BL1Wheel;
    public WheelCollider BR1Wheel;
    public WheelCollider BL2Wheel;
    public WheelCollider BR2Wheel;
}
[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FLWheel;
    public MeshRenderer FRWheel;
    public MeshRenderer BL1Wheel;
    public MeshRenderer BR1Wheel;
    public MeshRenderer BL2Wheel;
    public MeshRenderer BR2Wheel;
}