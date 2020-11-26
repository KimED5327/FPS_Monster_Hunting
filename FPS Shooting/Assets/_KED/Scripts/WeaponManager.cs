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

    const string PUT_IN = "PutIn";
    const string TAKE_OUT = "TakeOut";
    const string RELOAD = "Reload";

    const float weaponSwapWaitTime = 0.15f;

    Crosshair theCrosshair;
    Animator myAnim;

    bool isFinished = true;

    private void Awake()
    {
        theCrosshair = FindObjectOfType<Crosshair>();
        myAnim = GetComponentInChildren<Animator>();
        StartCoroutine(ActiveWeapon(goCurrentWeapon));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (goCurrentWeapon == goBow) return;
            StartCoroutine(ChangeWeapon(goBow));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (goCurrentWeapon == goPistol1) return;
            StartCoroutine(ChangeWeapon(goPistol1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (goCurrentWeapon == goRifle1) return;
            StartCoroutine(ChangeWeapon(goRifle1));
        }
    }

    IEnumerator ChangeWeapon(GameObject p_goWeapon)
    {
        isFinished = false;
        StartCoroutine(AllDeActive());
        yield return new WaitUntil(() => isFinished);
        StartCoroutine(ActiveWeapon(p_goWeapon));
    }

    IEnumerator AllDeActive()
    {
        goCurrentWeapon.GetComponent<Weapon>().SetActivate(false);
        myAnim.SetTrigger(PUT_IN);
        yield return new WaitForSeconds(weaponSwapWaitTime);

        goCrosshair.SetActive(false);
        goBow.SetActive(false);
        goPistol1.SetActive(false);
        goRifle1.SetActive(false); 
        isFinished = true;
    }

    IEnumerator ActiveWeapon(GameObject p_goWeapon)
    {
        p_goWeapon.SetActive(true);
        myAnim.SetTrigger(TAKE_OUT);
        yield return new WaitForSeconds(weaponSwapWaitTime);

        goCurrentWeapon = p_goWeapon;
        RangedWeapon t_weapon = p_goWeapon.GetComponent<RangedWeapon>();
        if(t_weapon != null && t_weapon.GetWeaponType() == WeaponType.RANGED)
        {
            if(t_weapon.GetWeaponName() != "Short Bow") // temp
                goCrosshair.SetActive(true);

            theCrosshair.SetGunAccuracy(t_weapon.GetAcuuracy());
            HUDWeapon.instance.SetAmmoUI(t_weapon.GetCurAmmo(), t_weapon.GetMagazineMax(), t_weapon.GetTotalAmmo());
            HUDWeapon.instance.SetWeaponName(t_weapon.GetWeaponName());
        }
        goCurrentWeapon.GetComponent<Weapon>().SetActivate(true);
    }

    public void ReloadPlay()
    {
        myAnim.SetTrigger(RELOAD);
    }
}
