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
    [SerializeField] float jumpForce = 5f;
    float curSpeed;


    [Header("Limit")]
    [SerializeField] float limitAngleMinX = -45f;
    [SerializeField] float limitAngleMaxX = 45f;


    [Header("Option")]
    [SerializeField] bool isFlip = true;


    float currentAngleY;
    float currentAngleX;

    // Component
    Camera cam;
    CharacterController theController;
    Crosshair theCrosshair;
    PlayerStatus theStatus;
    Gravity theGravity;

    void Start()
    {
        theStatus = GetComponent<PlayerStatus>();
        theGravity = GetComponent<Gravity>();
        theCrosshair = GetComponent<Crosshair>();
        theController = GetComponent<CharacterController>();
        cam = Camera.main;

        currentAngleY = transform.localEulerAngles.x;
        currentAngleX = cam.transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Spin();
        UpdateCrosshairState();
        Jump();
    }

    void Move()
    {
        curSpeed = 0;

        float dirX = Input.GetAxisRaw("Horizontal");
        float dirZ = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(dirX, 0, dirZ).normalized;

        // 걷기
        if (dir != Vector3.zero)
        {
            curSpeed = moveSpeed;

            // 달리기
            if (Input.GetKey(KeyCode.LeftShift) && theStatus.GetCurSp() > 0)
            {
                curSpeed += runSpeed;
                theStatus.DecreaseCurSp(1); // sp 소모
            }
        }

        dir = transform.TransformDirection(dir);
        //transform.forward = dir;

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

        if (Mathf.Abs(theGravity.GetVelocityY()) >= 0.1f)
            theCrosshair.JumpState();
        else if (curSpeed > runSpeed)
            theCrosshair.RunState();
        else if (curSpeed > 0.1f)
            theCrosshair.WalkState();
    }

    void Jump()
    {
        if (theGravity.IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                theGravity.SetVelocityY(jumpForce);
            }
        }
    }
}
