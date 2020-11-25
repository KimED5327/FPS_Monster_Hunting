using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] protected int maxHp = 100;
    protected int currentHp;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        currentHp = maxHp;
    }


    public int GetCurHp() { return currentHp; }
    public void SetCurHp(int p_value) { currentHp = p_value; }
    public int GetMaxHp() { return maxHp; }
    public void SetMaxHp(int p_value) { maxHp = p_value; }

}
