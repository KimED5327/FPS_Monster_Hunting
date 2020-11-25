using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    const int LEFT = 0, RIGHT = 1, UP = 2, DOWN = 3;

    [Header("Crosshair")]
    [SerializeField] RectTransform[] tfCrosshairParts = null;

    [SerializeField] float enlargeMaxValue = 200f;


    [Header("Basic Accuracy")]
    [SerializeField] float standAccuracy = 0.01f;
    [SerializeField] float walkAccuracy = 0.02f;
    [SerializeField] float runAccuracy = 0.05f;
    [SerializeField] float jumpAccuracy = 0.075f;

    [SerializeField] float shootAccuracy = 0.02f;

    [Header("Crosshair Sensitivity")]
    [SerializeField] float speed = 10f;
    [SerializeField] float enlargeMultiply = 10f;
    [SerializeField] float stableMultiply = 2f;

    float gunAccuracy = 0f;
    float curStateAccuracy = 0f;
    float curShootAccuracy = 0f;

    void Start()
    {
        StandState();
    }

    void Update()
    {
        float t_pos = enlargeMaxValue * (curStateAccuracy + curShootAccuracy + gunAccuracy) * enlargeMultiply;

        tfCrosshairParts[LEFT].localPosition = Vector3.MoveTowards(tfCrosshairParts[LEFT].localPosition,
                                        new Vector3(-t_pos, 0, 0), speed * Time.deltaTime);
        tfCrosshairParts[RIGHT].localPosition = Vector3.MoveTowards(tfCrosshairParts[RIGHT].localPosition,
                                        new Vector3(t_pos, 0, 0), speed * Time.deltaTime);
        tfCrosshairParts[UP].localPosition = Vector3.MoveTowards(tfCrosshairParts[UP].localPosition,
                                        new Vector3(0, t_pos, 0), speed * Time.deltaTime);
        tfCrosshairParts[DOWN].localPosition = Vector3.MoveTowards(tfCrosshairParts[DOWN].localPosition,
                                        new Vector3(0, -t_pos, 0), speed * Time.deltaTime);

        StableCrosshair();
    }



    void StableCrosshair()
    {
        if(curShootAccuracy > 0)
        {
            curShootAccuracy -= Time.deltaTime * stableMultiply;
            if (curShootAccuracy < 0)
                curShootAccuracy = 0;
        }
    }

    public void ShootState()
    {
        curShootAccuracy = shootAccuracy;
    }

    public void StandState()
    {
        curStateAccuracy = standAccuracy;
    }
    public void WalkState()
    {
        curStateAccuracy = walkAccuracy;
    }
    public void RunState()
    {
        curStateAccuracy = runAccuracy;
    }
    public void JumpState()
    {
        curStateAccuracy = jumpAccuracy;
    }

    public float GetAccuracy()
    {
        float t_accuracy = (curStateAccuracy + curShootAccuracy + gunAccuracy);
        if (t_accuracy < 0)
            t_accuracy = 0;
        return t_accuracy;
    }

    public void SetGunAccuracy(float p_value)
    {
        gunAccuracy = p_value;
    }

}
