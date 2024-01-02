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
    public float slipAngle;
    private float speed;
    public AnimationCurve steeringCurve;



    void Start()
    {
        playerRB = GetComponent<Rigidbody>();  
    }
    void Update()
    {
        speed = playerRB.velocity.magnitude;
        CheckInput();
        ApplyMotor();
        ApplySteering(); 
        ApplYWheelPositions();
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        slipAngle = Vector3.Angle(transform.forward, playerRB.velocity - transform.forward);

        //fixed code to brake even after going on reverse by Andrew Alex 
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

        colliders.BR2Wheel.brakeTorque = brakeInput * brakePower * 0.3f;
        colliders.BL2Wheel.brakeTorque = brakeInput * brakePower * 0.3f;


    }
    void ApplyMotor()
    {

        colliders.BR2Wheel.motorTorque = motorPower * gasInput;
        colliders.BL2Wheel.motorTorque = motorPower * gasInput;

    }
    void ApplySteering()
    {

        float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
        if (slipAngle < 120f)
        {
            steeringAngle += Vector3.SignedAngle(transform.forward, playerRB.velocity + transform.forward, Vector3.up);
        }
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