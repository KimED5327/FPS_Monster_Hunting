using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField] float fallingDownSpeed = 3.81f;

    [Header("Limit")]
    [SerializeField] float limitFallSpeed = 9.81f;

    [Header("Ground Layer")]
    [SerializeField] LayerMask layerMask = 1;

    [Header("FallDamage")]
    [SerializeField] float[] fallPosYs = null;
    [SerializeField] int[] fallDamages = null;
    float priorFallPosY = 0;
    float fallPosY = 0;

    float curVelocityY = 0f;    // Y Velocity
    float rayDistance;          // 땅 탐지 광선 길이
    float halfHeight;           // 캐릭터 사이즈의 반
    bool isGround = false;


    Status theStatus;
    CharacterController theController;
    private void Awake()
    {
        theStatus = GetComponent<PlayerStatus>();
        theController = GetComponent<CharacterController>();

        halfHeight = theController.height * 0.5f;
        rayDistance = halfHeight + 0.1f;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    void CheckGround()
    {
        if (curVelocityY <= 0 && (Physics.Raycast(transform.position, Vector3.down, rayDistance, layerMask) || theController.isGrounded))
            StickToGround();
        else
            ApplyGravity();
    }


    void StickToGround()
    {

        if (priorFallPosY - fallPosY < priorFallPosY)
        {
            float t_fallDistance = priorFallPosY - fallPosY;

            for (int i = fallPosYs.Length - 1; i >= 0; i--)
            {
                if (t_fallDistance >= fallPosYs[i])
                {
                    theStatus.DecreaseHp(fallDamages[i]);

                    break;
                }
            }

        }
        priorFallPosY = transform.position.y;
        fallPosY = 0;
        curVelocityY = 0;
        isGround = true;

    }


    void ApplyGravity()
    {
        fallPosY = transform.position.y;

        isGround = false;
        curVelocityY -= fallingDownSpeed * Time.deltaTime;
        if (curVelocityY <= limitFallSpeed)
            curVelocityY = limitFallSpeed;

        theController.Move(transform.up * curVelocityY * Time.deltaTime);
    }

    public void SetVelocityY(float p_value)
    {
        curVelocityY = p_value;
    }

    public float GetVelocityY() { return curVelocityY; }

    public bool IsGrounded()
    {
        return isGround;
    }
}
