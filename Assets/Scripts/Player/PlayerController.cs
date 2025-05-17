using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed;
    public float jumpPower;
    public LayerMask groundLayer;
    private Vector2 curMovementInput;

    [Header("Look")] 
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    public float lookSensitivity; //민감도
    private Vector2 mouseDelta;
    private float camCurXRot;
    
    
    
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    //--------------------------------------------------------------
    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    private void Move()
    {
        Vector3 dir= transform.forward * curMovementInput.y+transform.right * curMovementInput.x;
        dir*= moveSpeed;
        dir.y=rigid.velocity.y;
        
        rigid.velocity = dir;
    }
    
    //--------------------------------------------------------------
    
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;// 카메라의 Y축을 돌려 카메라의 X방향이 감도 만큼 돌려지게끔
        camCurXRot = Math.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // 카메라를 Rotation했을 때 실제로 보여지게 돌려지는것은 반대이기때문에 -으로 변환
        
        transform.localEulerAngles+=new Vector3(0, mouseDelta.x * lookSensitivity, 0);//X값에 민감도를 곱해 Y값에 넣어서 구현
    }
    
    //--------------------------------------------------------------

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        if (context.phase == InputActionPhase.Started&&IsGrounded())
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i],0.1f,groundLayer))
            {
                return true;
            }
        }
        return false;
    }
}