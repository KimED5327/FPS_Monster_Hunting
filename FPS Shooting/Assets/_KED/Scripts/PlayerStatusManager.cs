using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusManager : MonoBehaviour
{
    [SerializeField] Text txt_HP = null;
    [SerializeField] Text txt_SP = null;


    public void SetSpText(int p_value)
    {
        txt_SP.text = p_value.ToString();
    }
    public void SetHpText(int p_value)
    {
        txt_HP.text = p_value.ToString();
    }
}
