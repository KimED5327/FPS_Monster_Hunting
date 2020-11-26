using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{

    [SerializeField] int maxSp = 100;
    int currentSp;

    [Header("Recover")]
    [SerializeField] 
    float recoverSpTime = 0.25f;
    float curSpTime = 0f;
    [SerializeField] 
    int recoverSp = 1;

    [SerializeField] float recoverWaitTime = 3f;
    float curRecoverWaitTime = 0f;

    [Header("Decs Delay")]
    [SerializeField] 
    int decreaseCnt = 2;
    int curDecreaseCnt = 0;
    PlayerStatusManager theStatusManager;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        currentSp = maxSp;
        theStatusManager = FindObjectOfType<PlayerStatusManager>();
    }

    // Update is called once per frame
    void Update()
    {
        RecoverSP();
        theStatusManager.SetSpText(currentSp);
        theStatusManager.SetHpText(currentHp);
    }

    void RecoverSP()
    {
        curRecoverWaitTime += Time.deltaTime;
        if(curRecoverWaitTime >= recoverWaitTime)
        {
            curSpTime += Time.deltaTime;
            if (curSpTime >= recoverSpTime)
            {
                curSpTime = 0f;
                IncreaseCurSp(recoverSp);
            }
        }

    }

    public int GetCurSp() { return currentSp; }
    public void IncreaseCurSp(int p_value)
    {
        currentSp += p_value;
        if (currentSp > maxSp)
            currentSp = maxSp;
        theStatusManager.SetSpText(currentSp);
    }
    public void DecreaseCurSp(int p_value)
    {

        curRecoverWaitTime = 0f; // 스태미나 자동 회복 카운트 초기화

        if(curDecreaseCnt++ >= decreaseCnt) // 스태미나 감소 카운트
        {
            curDecreaseCnt = 0;
            currentSp -= p_value;
            if (currentSp < 0)
                currentSp = 0;
            theStatusManager.SetSpText(currentSp);
        }
    }
    public int GetMaxSp() { return maxSp; }
    public void SetMaxSp(int p_value) { maxSp = p_value; 
    }
}
