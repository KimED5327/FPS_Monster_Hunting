using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject goBow = null;
    [SerializeField] GameObject goPistol1 = null;
    [SerializeField] GameObject goRifle1 = null;

    [SerializeField] GameObject goCurrentWeapon = null;
    [SerializeField] GameObject goCrosshair = null;

    Crosshair theCrosshair;

    private void Awake()
    {
        theCrosshair = FindObjectOfType<Crosshair>();
        ActiveWeapon(goCurrentWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (goCurrentWeapon == goBow) return;
            goCrosshair.SetActive(false);
            AllDeActive();
            ActiveWeapon(goBow);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (goCurrentWeapon == goPistol1) return;
            goCrosshair.SetActive(true);
            AllDeActive();
            ActiveWeapon(goPistol1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (goCurrentWeapon == goRifle1) return;
            goCrosshair.SetActive(true);
            AllDeActive();
            ActiveWeapon(goRifle1);
        }
    }

    void AllDeActive()
    {
        goBow.SetActive(false);
        goPistol1.SetActive(false);
        goRifle1.SetActive(false);
    }

    void ActiveWeapon(GameObject p_goWeapon)
    {
        p_goWeapon.SetActive(true);
        goCurrentWeapon = p_goWeapon;
        RangedWeapon t_weapon = p_goWeapon.GetComponent<RangedWeapon>();
        if(t_weapon.GetWeaponType() == WeaponType.RANGED)
        {
            theCrosshair.SetGunAccuracy(t_weapon.GetAcuuracy());
            HUDWeapon.instance.SetAmmoUI(t_weapon.GetCurAmmo(), t_weapon.GetMagazineMax(), t_weapon.GetTotalAmmo());
            HUDWeapon.instance.SetWeaponName(t_weapon.GetWeaponName());
        }
    }
}
