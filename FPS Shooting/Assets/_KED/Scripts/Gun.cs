using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    Camera cam;

    [Header("Status")]
    [SerializeField] float fireRate = 0.5f;
    float curFireTime = 0f;

    [Header("Bullet")]
    [SerializeField] int ammoTotalMax = 60;
    [SerializeField] int magazineAmmoMax = 12;
    int curMagazineAmmoCount = 0;
    int curTotalAmmoCount = 0;
    

    [Header("Effect")]
    [SerializeField] GameObject goEffect = null;
    [SerializeField] ParticleSystem psMuzzleFlash = null;

    // flag
    bool canFire = true;
    bool isReload = false;


    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
        curMagazineAmmoCount = magazineAmmoMax;
        curTotalAmmoCount = magazineAmmoMax;
    }

    void OnEnable()
    {
        AmmoManager.instance.SetAmmoUI(GetCurAmmo(), GetMagazineMax(), GetTotalAmmo());
    }

    // Update is called once per frame
    void Update()
    {
        CalcFireRate();
        
        if(Input.GetMouseButtonDown(0))
            Fire();

        if (Input.GetKeyDown(KeyCode.R))
            TryReload();
    }


    void CalcFireRate()
    {
        if (!canFire)
        {
            curFireTime += Time.deltaTime;
            if(curFireTime >= fireRate)
            {
                canFire = true;
                curFireTime = 0f;
            }
        }
    }

    void Fire()
    {
        if (canFire)
        {
            if(curMagazineAmmoCount > 0)
                ShootBullet();
            else
                TryReload();   
        }
    }
    
    void ShootBullet()
    {
        psMuzzleFlash.Play();
        curMagazineAmmoCount--;
        canFire = false;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 1000f))
        {
            GameObject t_effect = Instantiate(goEffect, hit.point, Quaternion.Euler(hit.normal));
            Destroy(t_effect, 5f); // temp
        }

        AmmoManager.instance.SetAmmoUI(GetCurAmmo(), GetMagazineMax(), GetTotalAmmo());
    }

    void TryReload()
    {
        if (curTotalAmmoCount <= 0)
            Debug.Log("모든 총알이 소진됨");
        else
            Reload();
    }

    void Reload()
    {
        if (!isReload)
        {
            isReload = true;

            // 탄알집 안에 있던 총알은 버리지 않음
            curTotalAmmoCount += curMagazineAmmoCount;

            // 가진 총알이 부족하면 가진만큼만 재장전하고 그렇지 않으면 전부 재장전
            int t_reloadBulletCount = (curTotalAmmoCount >= magazineAmmoMax)
                                    ? magazineAmmoMax
                                    : magazineAmmoMax - curTotalAmmoCount;

            curTotalAmmoCount -= t_reloadBulletCount;
            curMagazineAmmoCount = t_reloadBulletCount;

            AmmoManager.instance.SetAmmoUI(GetCurAmmo(), GetMagazineMax(), GetTotalAmmo());

            isReload = false;
        }
    }


    public int GetCurAmmo() { return curMagazineAmmoCount; }
    public int GetMagazineMax() { return magazineAmmoMax; }
    public int GetTotalAmmo() { return curTotalAmmoCount; }

    public void SetTotalBullet(int p_value) {
        curTotalAmmoCount += p_value;
        if (curTotalAmmoCount >= ammoTotalMax)
            curTotalAmmoCount = ammoTotalMax;
    }
}
