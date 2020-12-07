using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Basic Status")]
    [SerializeField] protected int maxHp = 100;
    protected int currentHp;

    bool isDead = false;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        currentHp = maxHp;
    }


    public int GetCurHp() { return currentHp; }
    public void SetCurHp(int p_value) { currentHp = p_value; }
    public int GetMaxHp() { return maxHp; }
    public void SetMaxHp(int p_value) { maxHp = p_value; }

    public void DecreaseHp(int p_value)
    {
        currentHp -= p_value;
        if (currentHp <= 0)
        {
            currentHp = 0;
            isDead = true;
        }
    }

    public bool IsDead() { return isDead; }
}
