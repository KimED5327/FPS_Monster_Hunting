using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager instance;
    void Awake() => instance = this;


    [SerializeField] Text txtCurAmmo = null;
    [SerializeField] Text txtCurTotalAmmo = null;


    public void SetAmmoUI(int p_curMagazineAmmoCount, int p_magazineAmmoCount, int p_totalAmmoCount)
    {
        txtCurTotalAmmo.text = p_totalAmmoCount.ToString();
        txtCurAmmo.text = p_curMagazineAmmoCount + " / " + p_magazineAmmoCount;
    }

}
