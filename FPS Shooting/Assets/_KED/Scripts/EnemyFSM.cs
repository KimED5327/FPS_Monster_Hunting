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

    Transform tfTarget;

    // IDLE
    [SerializeField] float searchDistance = 7f;

    // Move
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float attackDistance = 1f;

    // Attack
    [SerializeField] int damage = 5;
    [SerializeField] float attackInterval = 2f;
    [SerializeField] float applyDamageDelay = 0.5f;
    float curAttackTime = 0f;

    // Return
    Vector3 tfOrigin;

    // Damaged
    [SerializeField] float chaseTime = 3f;
    float curChaseCount = 0f;

    // Die
    [SerializeField] float destroyWaitTime = 2f;


    Status theStatus;
    CharacterController theController;
    Animator myAnim;

    bool isDead = false;
    bool isChase = false;


    const string IDLE = "Idle";
    const string MOVE = "Move";
    const string ATTACK = "Attack";
    const string DAMAGE = "Damage";
    const string DIE = "Die";


    private void Awake()
    {
        myAnim = GetComponent<Animator>();
        theStatus = GetComponent<Status>();
        theController = GetComponent<CharacterController>();
    }


    void Start()
    {
        tfOrigin = transform.position;
        state = State.Idle;
        tfTarget = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        if (!isDead)
        {


            if (isChase)
            {
                Move();
                curChaseCount += Time.deltaTime;
                if(curChaseCount >= chaseTime)
                {
                    curChaseCount = 0f;
                    isChase = false;
                }
                return;
            }

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
                default:
                    break;
            }
        }
    }


    void Idle()
    {

        if((tfTarget.position - transform.position).magnitude <= searchDistance)
        {
            myAnim.SetBool(MOVE, true);
            state = State.Move;
        }

    }


    void Move()
    {
        Vector3 t_dir = (tfTarget.position - transform.position).normalized;
        theController.Move(t_dir * moveSpeed * Time.deltaTime);
        transform.forward = t_dir;

        if ((transform.position - tfTarget.position).magnitude <= attackDistance)
        {

            myAnim.SetBool(MOVE, false);
            state = State.Attack;
        }
        else if ((tfTarget.position - transform.position).magnitude  >= searchDistance)
        {
            myAnim.SetBool(MOVE, true);
            state = State.Return;
        }

    }

    void Attack()
    {
        curAttackTime += Time.deltaTime;
        if (curAttackTime >= attackInterval)
        {
            curAttackTime = 0f;
            Invoke(nameof(PlayerHit), applyDamageDelay);
            myAnim.SetTrigger(ATTACK);
        }

        if ((transform.position - tfTarget.position).magnitude >= attackDistance)
        {
            myAnim.SetBool(MOVE, true);
            state = State.Move;
        }
    }

    void PlayerHit()
    {
        tfTarget.GetComponent<Status>().DecreaseHp(damage);
    }

    void Return()
    {
        Vector3 t_dir = (tfOrigin - transform.position).normalized;
        theController.Move(t_dir * moveSpeed * Time.deltaTime);
        transform.forward = t_dir;

        if ((transform.position - tfTarget.position).magnitude <= searchDistance)
        {
            myAnim.SetBool(MOVE, true);
            state = State.Move;
        }

        if ((transform.position - tfTarget.position).sqrMagnitude <= 1f)
        {
            myAnim.SetBool(MOVE, false);
            state = State.Idle;
        }
    }

    public void Damage(int p_value)
    {
        if (!isDead)
        {
            isChase = true;
            curChaseCount = 0;
            theStatus.DecreaseHp(p_value);
            if (theStatus.GetCurHp() <= 0)
                Die();
            else
            {   
                if(IsPlaying(IDLE) || IsPlaying(MOVE))
                    myAnim.SetTrigger(DAMAGE);
            }
        }
    }


    void Die()
    {
        isDead = true;
        myAnim.SetTrigger(DIE);
        Destroy(gameObject, destroyWaitTime);
    }

    bool IsPlaying(string stateName)
    {
        if (myAnim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
}
