using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : RangedWeapon
{


    [Header("Arrow Speed")]
    [SerializeField] float fireSpeedMax = 40f;
    [SerializeField] float fireSpeedMin = 3f;
    [SerializeField] float pullSpeed = 10f;
    float arrowSpeed;


    [Header("Arrow")]
    [SerializeField] GameObject goArrowPrefab = null;
    [SerializeField] GameObject goArrowShow = null;

    [Header("ArrowPos")]
    [SerializeField] Transform tfArrowOriginPos = null;
    [SerializeField] Transform tfArrowFullPowerPos = null;

    [Header("FOV")]
    [SerializeField] float targetFov = 50f;
    float originFov = 0f;


    bool isGrabArrow = false;
    bool isSettingArrow = false;



    void OnEnable()
    {
        goArrowShow.SetActive(false);

        if(originFov == 0f)
            originFov = cam.fieldOfView;

        TryReload();
    }

    override protected void Update()
    {
        base.Update();
    }



    void ReadyToFire()
    {
        StopAllCoroutines();
        goArrowShow.transform.position = Vector3.Lerp(goArrowShow.transform.position, tfArrowFullPowerPos.position, 0.03f);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, 0.03f);

        arrowSpeed += pullSpeed * Time.deltaTime;
        if (arrowSpeed >= fireSpeedMax)
            arrowSpeed = fireSpeedMax;
    }


    protected override void BulletCreate()
    {
        isGrabArrow = false;
        isSettingArrow = false;
        cam.fieldOfView = originFov;

        goArrowShow.SetActive(false);

        GameObject t_arrow = Instantiate(goArrowPrefab, goArrowShow.transform.position, goArrowShow.transform.rotation);
        t_arrow.GetComponent<Arrow>().SetArrow(arrowSpeed);

        HUDWeapon.instance.SetAmmoUI(curMagazineAmmo, 1, curTotalAmmo);
    }


    override protected void ReloadFinish()
    {
        // 가진 총알이 부족하면 가진만큼만 재장전하고 그렇지 않으면 전부 재장전
        int t_reloadBulletCount = (curTotalAmmo >= magazineAmmoMax)
                                ? magazineAmmoMax
                                : magazineAmmoMax - curTotalAmmo;

        curTotalAmmo -= t_reloadBulletCount;
        curMagazineAmmo = t_reloadBulletCount;

        HUDWeapon.instance.SetAmmoUI(GetCurAmmo(), GetMagazineMax(), GetTotalAmmo());

        goArrowShow.SetActive(true);
        goArrowShow.transform.position = tfArrowOriginPos.position;
        arrowSpeed = fireSpeedMin;

        isSettingArrow = true;

        isReload = false;
    }


    protected override void OnMouseButtonLeftDown()
    {
        if (isSettingArrow && curMagazineAmmo > 0)
            isGrabArrow = true;
        else
            TryReload();
    }

    protected override void OnMouseButtonLeft()
    {
        if(isGrabArrow)
            ReadyToFire();
    }

    protected override void OnMouseButtonLeftUp()
    {
        if(isGrabArrow)
            Fire();
    }
}
