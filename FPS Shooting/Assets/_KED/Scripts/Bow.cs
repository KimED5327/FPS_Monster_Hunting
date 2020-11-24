using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] float reloadTime = 1.0f;
    [SerializeField] int magazineMax = 1;
    [SerializeField] int totalAmmoMax = 10;
    int curAmmo;
    int curTotalAmmo;


    [Header("Arrow Speed")]
    [SerializeField] float fireSpeedMax = 40f;
    [SerializeField] float fireSpeedMin = 3f;
    [SerializeField] float pullSpeed = 10f;
    float arrowSpeed;


    [Header("Arrow")]
    [SerializeField] GameObject goArrowPrefab = null;
    [SerializeField] GameObject goArrowShow = null;

    [Header("ArrowPos")]
    [SerializeField] Transform tfArrowReloadPos = null;
    [SerializeField] Transform tfArrowOriginPos = null;
    [SerializeField] Transform tfArrowFullPowerPos = null;

    [Header("FOV")]
    [SerializeField] float targetFov = 50;
    float originFov;
    Camera cam;

    bool isGrabArrow = false;
    bool isSettingArrow = false;

    private void Awake()
    {
        cam = Camera.main;
        originFov = cam.fieldOfView;
        curAmmo = magazineMax;
        curTotalAmmo = magazineMax;
    }

    void OnEnable()
    {
        goArrowShow.SetActive(false);

        AmmoManager.instance.SetAmmoUI(curAmmo, 1, curTotalAmmo);
        TryReload(false);

    }

    void Update()
    {
        if (Input.GetMouseButton(0) && isSettingArrow && curAmmo > 0)
            ReadyToFire();

        if (Input.GetMouseButtonUp(0) && isGrabArrow && isSettingArrow)
            FireArrow();
    }

    void ReadyToFire()
    {
        isGrabArrow = true;
        StopAllCoroutines();
        goArrowShow.transform.position = Vector3.Lerp(goArrowShow.transform.position, tfArrowFullPowerPos.position, 0.03f);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, 0.03f);

        arrowSpeed += pullSpeed * Time.deltaTime;
        if (arrowSpeed >= fireSpeedMax)
            arrowSpeed = fireSpeedMax;

    }

    void FireArrow()
    {
        cam.fieldOfView = originFov;
        isGrabArrow = false;
        isSettingArrow = false;
        goArrowShow.SetActive(false);
        GameObject t_arrow = Instantiate(goArrowPrefab, goArrowShow.transform.position, goArrowShow.transform.rotation);
        t_arrow.GetComponent<Arrow>().SetArrow(arrowSpeed);

        curAmmo--;
        AmmoManager.instance.SetAmmoUI(curAmmo, 1, curTotalAmmo);

        TryReload(true);
    }

    void TryReload(bool isMinus)
    {
        StopAllCoroutines();
        if (curTotalAmmo > 0)
            StartCoroutine(ReloadArrowCo(isMinus));
        else
            Debug.Log("화살 부족");
    }

    IEnumerator ReloadArrowCo(bool isMinus = false)
    {
        if (curTotalAmmo > 0)
        {
            goArrowShow.transform.position = tfArrowReloadPos.position;
            goArrowShow.SetActive(true);

            yield return new WaitForSeconds(reloadTime);
            if (isMinus)
            {
                curTotalAmmo--;
                curAmmo = 1;
            }

            AmmoManager.instance.SetAmmoUI(curAmmo, 1, curTotalAmmo);
            arrowSpeed = fireSpeedMin;

            isSettingArrow = true;

            while (true)
            {
                if (goArrowShow == null) break;

                goArrowShow.transform.position = Vector3.Lerp(goArrowShow.transform.position, tfArrowOriginPos.position, 0.05f);
                yield return null;
            }

        }

    }



    public void SetTotalBullet(int p_value)
    {
        curTotalAmmo += p_value;
        if (curTotalAmmo >= totalAmmoMax)
            curTotalAmmo = totalAmmoMax;
    }
}
