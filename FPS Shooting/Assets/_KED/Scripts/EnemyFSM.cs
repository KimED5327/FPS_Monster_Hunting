using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Idle, Move, Attack, Return, Damaged, Die,
    }

    State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Move:
                Move();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Return:
                Return();
                break;
            case State.Damaged:
                Damaged();
                break;
            case State.Die:
                Die();
                break;
            default:
                break;
        }
    }


    void Idle()
    {

    }


    void Move()
    {

    }

    void Attack()
    {

    }


    void Return()
    {

    }

    void Damaged()
    {
        // 코루틴
        // 체력 1 > 0 일때만 피격
        // Any State
    }


    void Die()
    {
        // 코루틴
        // 체력 0 이하
        // 몬스터 오브젝트 삭제
        // Any State
    }
}
