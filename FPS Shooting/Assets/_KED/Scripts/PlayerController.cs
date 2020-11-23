using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] float spinSpeed = 10f;
    [SerializeField] float fallingDownSpeed = 3.81f;
    [SerializeField] float jumpForce = 5f;

    [Header("Limit")]
    [SerializeField] float limitAngleMinX = -45f;
    [SerializeField] float limitAngleMaxX = 45f;
    [SerializeField] float limitFallSpeed = 9.81f;


    [Header("Option")]
    [SerializeField] bool isFlip = true;

    [Header("Ground Layer")]
    [SerializeField] LayerMask layerMask = 1;

    [Header("Handle Angle")]
    [SerializeField] Transform goHandle = null;
    [SerializeField] float ratioY = 0.5f;

    float currentAngleY;
    float currentAngleX;
    float curVelocityY = 0f;    // Y Velocity
    float rayDistance;          // 땅 탐지 광선 길이
    float halfHeight;           // 캐릭터 사이즈의 반
    bool isGround = false;

    // Component
    Camera cam;
    CharacterController theController;
    
    void Start()
    {
        theController = GetComponent<CharacterController>();
        cam = Camera.main;

        halfHeight = theController.height;
        rayDistance = halfHeight + 0.05f;

        currentAngleY = transform.localEulerAngles.x;
        currentAngleX = cam.transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Spin();
    }

    void FixedUpdate()
    {
        Jump();
        CheckGround();
    }

    void Move()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirZ = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(dirX, 0, dirZ).normalized;

        // 달리기
        float applySpeed = (Input.GetKey(KeyCode.LeftShift)) ? moveSpeed + runSpeed : moveSpeed;

        if(dir.z != 0)
            theController.Move(transform.forward * dir.z * applySpeed * Time.deltaTime);
        if(dir.x != 0)
            theController.Move(transform.right * dir.x * applySpeed * Time.deltaTime);
    }

    void Spin()
    {
        float dirX = Input.GetAxis("Mouse X");
        float dirY = Input.GetAxis("Mouse Y");

        // 몸체 좌우 회전
        Vector3 dir = new Vector3(dirX, dirY, 0f);
        currentAngleY += dir.x * spinSpeed * Time.fixedDeltaTime;
        transform.localEulerAngles = new Vector3(0f, currentAngleY, 0f);

        // 캠 상하 회전
        float t_reverse = (isFlip) ? -1f : 1f;
        currentAngleX += dir.y * spinSpeed * t_reverse * Time.fixedDeltaTime;
        currentAngleX = Mathf.Clamp(currentAngleX, limitAngleMinX, limitAngleMaxX);
        cam.transform.localEulerAngles = new Vector3(currentAngleX, 0f, 0f);

        // 손
        goHandle.localEulerAngles = new Vector3(currentAngleX * ratioY, 0f, 0f);
    }


    void CheckGround()
    {
        // 상승 중이 아닌 경우에 바닥 판정 체크
        if (curVelocityY >= 0 && Physics.Raycast(transform.position, Vector3.down, rayDistance, layerMask))
            StickToGround();
        else
            ApplyGravity();
    }


    void StickToGround()
    {
        isGround = true;
        curVelocityY = 0;
    }


    void ApplyGravity()
    {
        isGround = false;
        curVelocityY += fallingDownSpeed * Time.deltaTime;
        if (curVelocityY > limitFallSpeed)
            curVelocityY = limitFallSpeed;

        theController.Move(transform.up * -curVelocityY * Time.deltaTime);
    }


    void Jump()
    {
        if (isGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                curVelocityY = -jumpForce;
            }
        }
    }
}
