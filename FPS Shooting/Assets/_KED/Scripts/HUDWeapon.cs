using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDWeapon : MonoBehaviour
{
    public static HUDWeapon instance;
    void Awake() => instance = this;


    [SerializeField] Text txtCurAmmo = null;
    [SerializeField] Text txtCurTotalAmmo = null;
    [SerializeField] Text txtCurWeaponName = null;
    [SerializeField] Text txtCurGranadeCount = null;

    public void SetAmmoUI(int p_curMagazineAmmoCount, int p_magazineAmmoCount, int p_totalAmmoCount)
    {
        txtCurTotalAmmo.text = p_totalAmmoCount.ToString();
        txtCurAmmo.text = p_curMagazineAmmoCount + " / " + p_magazineAmmoCount;
    }

    public void SetGranadeUI(int p_value)
    {
        txtCurGranadeCount.text = p_value.ToString();
    }

    public void SetWeaponName(string p_weaponName)
    {
        txtCurWeaponName.text = p_weaponName;
    }

}
