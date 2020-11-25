using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{

    [SerializeField] int maxSp =100;
    int currentSp;

    [SerializeField] float recoverSpTime = 0.25f;
    float curSpTime = 0f;
    [SerializeField] int recoverSp = 1;


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
    }

    void RecoverSP()
    {
        curSpTime += Time.deltaTime;
        if(curSpTime >= recoverSpTime)
        {
            curSpTime = 0f;
            IncreaseCurSp(recoverSp);
            theStatusManager.SetSpText(currentSp);
        }
    }

    public int GetCurSp() { return currentHp; }
    public void IncreaseCurSp(int p_value)
    {
        currentSp += p_value;
        if (currentSp > maxSp)
            currentSp = maxSp;
        theStatusManager.SetSpText(currentSp);
    }
    public void DecreaseCurSp(int p_value)
    {
        currentSp -= p_value;
        if (currentSp < 0)
            currentSp = 0;
        theStatusManager.SetSpText(currentSp);
    }
    public int GetMaxSp() { return maxSp; }
    public void SetMaxSp(int p_value) { maxSp = p_value; 
    }
}
