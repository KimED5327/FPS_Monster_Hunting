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
    float curSpeed;


    [Header("Limit")]
    [SerializeField] float limitAngleMinX = -45f;
    [SerializeField] float limitAngleMaxX = 45f;
    [SerializeField] float limitFallSpeed = 9.81f;


    [Header("Option")]
    [SerializeField] bool isFlip = true;

    [Header("Ground Layer")]
    [SerializeField] LayerMask layerMask = 1;


    float currentAngleY;
    float currentAngleX;
    float curVelocityY = 0f;    // Y Velocity
    float rayDistance;          // 땅 탐지 광선 길이
    float halfHeight;           // 캐릭터 사이즈의 반
    bool isGround = false;

    // Component
    Camera cam;
    CharacterController theController;
    Crosshair theCrosshair;
    PlayerStatus theStatus;

    void Start()
    {
        theStatus = GetComponent<PlayerStatus>();
        theCrosshair = GetComponent<Crosshair>();
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
        UpdateCrosshairState();
    }



    void FixedUpdate()
    {
        Jump();
        CheckGround();
    }

    void Move()
    {
        curSpeed = 0;

        float dirX = Input.GetAxisRaw("Horizontal");
        float dirZ = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(dirX, 0, dirZ).normalized;

        // 달리기
        if(dir != Vector3.zero)
            curSpeed = (Input.GetKey(KeyCode.LeftShift)) ? moveSpeed + runSpeed : moveSpeed;

        dir = transform.TransformDirection(dir);
        theController.Move(dir * curSpeed * Time.deltaTime);

    }

    void Spin()
    {
        float dirX = Input.GetAxis("Mouse X");
        float dirY = Input.GetAxis("Mouse Y");

        // 몸체 좌우 회전
        currentAngleY += dirX * spinSpeed * Time.fixedDeltaTime;
        transform.localEulerAngles = new Vector3(0f, currentAngleY, 0f);

        // 캠 상하 회전
        float t_reverse = (isFlip) ? -1f : 1f;
        currentAngleX += dirY * spinSpeed * t_reverse * Time.fixedDeltaTime;
        currentAngleX = Mathf.Clamp(currentAngleX, limitAngleMinX, limitAngleMaxX);
        cam.transform.localEulerAngles = new Vector3(currentAngleX, 0f, 0f);

    }

    void UpdateCrosshairState()
    {
        theCrosshair.StandState();

        if (Mathf.Abs(curVelocityY) >= 0.1f)
            theCrosshair.JumpState();
        else if (curSpeed > runSpeed) {
            theCrosshair.RunState();
            theStatus.DecreaseCurSp(1);
        }
        else if (curSpeed > 0.1f)
            theCrosshair.WalkState();
    }

    void CheckGround()
    {
        if (curVelocityY <= 0 && Physics.Raycast(transform.position, Vector3.down, rayDistance, layerMask))
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
        curVelocityY -= fallingDownSpeed * Time.deltaTime;
        if (curVelocityY <= limitFallSpeed)
            curVelocityY = limitFallSpeed;

        theController.Move(transform.up * curVelocityY * Time.deltaTime);
    }


    void Jump()
    {
        if (isGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                curVelocityY = jumpForce;
            }
        }
    }
}
