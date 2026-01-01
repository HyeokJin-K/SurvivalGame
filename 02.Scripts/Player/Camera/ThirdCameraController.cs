using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


// 3인칭 카메라 컨트롤러
public class ThirdCameraController : MonoBehaviour
{
    private Vector2 _direction;
    private Vector3 _cameraRotateValue;

    private readonly (float min, float max) CameraRotateYClamp = (-40f, 50f);

    [Header("카메라 타겟")] [SerializeField] private Transform target;

    [Header("카메라 수치 조정")] [SerializeField] private float cameraMouseSensitivity;
    [SerializeField, Range(0f, 1f)] private float lerpCameraRotateSpeed;
    [SerializeField, Range(0f, 1f)] private float lerpCameraFollowSpeed;
    [SerializeField] private Vector2 offset;
    [SerializeField] private bool isMouseMoveLerp;

    private float _offsetMagnitude;
    private Vector3 _velocity = Vector3.zero;

    #region Unity LifeCycle

    private void Awake()
    {
        if(!target) Debug.LogError("3인칭 카메라 컴포넌트 플레이어 하위에 존재하는 CameraTarget 오브젝트 캐싱 필요");
    }

    private void Start()
    {
        if(!target) return;
        
        transform.position = target.position;
        _offsetMagnitude = offset.magnitude;
    }

    private void Update()
    {
        if(!target) return;
        
        ApplyRotateValue();
    }

    private void FixedUpdate()
    {
        if(!target) return;
        
        CameraRotate();  
        CameraPosition();
    }
    
    #endregion

    private void CameraRotate()
    {
        if (isMouseMoveLerp)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(_cameraRotateValue),
                lerpCameraRotateSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Euler(_cameraRotateValue);
        }
    }

    private void CameraPosition()
    {
        transform.position = Vector3.Lerp(transform.position, AdjustObstacleCameraPosition(), lerpCameraFollowSpeed);
    }

    private void ApplyRotateValue()
    {
        if (_direction.sqrMagnitude < 0.01f) return;
        
        _cameraRotateValue += new Vector3(-_direction.y, _direction.x, 0f) * (cameraMouseSensitivity * Time.deltaTime);
        _cameraRotateValue.x = Mathf.Clamp(_cameraRotateValue.x, CameraRotateYClamp.min, CameraRotateYClamp.max);
    }

    private Vector3 GetCameraPosition()
    {
        return target.position + transform.rotation * new Vector3(0f, offset.y, -offset.x);
    }

    private Vector3 AdjustObstacleCameraPosition()
    {
        var dir = transform.rotation * new Vector3(0f, offset.y, -offset.x);
        Debug.DrawRay(target.position, dir, Color.red);

        if (Physics.SphereCast(target.position, 0.1f, dir, out var hitInfo,
                _offsetMagnitude, LayerMask.GetMask("Ground")))
        {
            var adjustedPosition = hitInfo.point + hitInfo.normal * 0.1f;
            return adjustedPosition;
        }
        else
        {
            return GetCameraPosition();
        }
    }

    #region InputCallBack
    public void OnLook(InputAction.CallbackContext context)
    {
//        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        
        var dir = context.ReadValue<Vector2>();
        _direction = dir;
    }
    #endregion

}