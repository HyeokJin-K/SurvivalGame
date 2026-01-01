
using System;
using UnityEngine;

[Serializable]
public class PlayerPhysicsConfig
{
    [Header("중력 제어")]
    public bool useGravity;
    public float gravity;
    public float terminalVelocityY;
    
    [Header("회전 제어")]
    public bool doLerpRotate;
    public float lerpRotateSpeed;
    [Range(0f, 1f)] public float attackRotateSpeedDeclinedRate;
    
    [Header("감속 제어")]
    public float normalAcceleration;
    public float sprintAcceleration;
    public float decelerationSpeed;
    public float rollSpeed;
    public float rollDecelerationSpeed;
}

