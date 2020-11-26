using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    [Header("Bullet")]
    [SerializeField] protected int totalAmmoMax = 60;
    [SerializeField] protected int magazineAmmoMax = 12;
    [SerializeField] protected int curMagazineAmmo = 0;
    [SerializeField] protected int curTotalAmmo = 0;


    [Header("Fire Info")]
    [SerializeField] float fireRate = 0.5f;
    float curFireTime = 0f;

    [Header("Accuracy"), Range(-0.1f, 0.3f)]
    [SerializeField] protected float accuracy = 0.05f;

    [Header("Reload")]
    [SerializeField] protected float reloadTime = 2f;

    // flag
    protected bool canFire = true;
    protected bool isReload = false;

    override protected void Update()
    {
        if (!isReload)
        {
            CalcFireRate();     // Calculate a next fire time.
            base.Update();      // Process a Left Mouse Button.

            if (Input.GetKeyDown(KeyCode.R))
                TryReload();
        }

    }


    protected void CalcFireRate()
    {
        if (!canFire)
        {
            curFireTime += Time.deltaTime;
            if (curFireTime >= fireRate)
            {
                canFire = true;
                curFireTime = 0f;
            }
        }
    }

    protected void Fire()
    {
        if (canFire)
        {
            if (curMagazineAmmo > 0)
                Shooting();
            else
                TryReload();
        }
    }

    protected void Shooting()
    {
        curMagazineAmmo--;
        canFire = false;

        BulletCreate();

        HUDWeapon.instance.SetAmmoUI(GetCurAmmo(), GetMagazineMax(), GetTotalAmmo());
    }

    abstract protected void BulletCreate();

    protected void TryReload()
    {
        if (curTotalAmmo <= 0)
            Debug.Log("모든 총알이 소진됨"); // 틱 소리 재생 필요
        else
            Reload();
    }

    protected void Reload()
    {
        if (!isReload)
        {
            isReload = true;

            // 탄알집 안에 있던 총알은 버리지 않음
            curTotalAmmo += curMagazineAmmo;
            HUDWeapon.instance.SetAmmoUI(GetCurAmmo(), GetMagazineMax(), GetTotalAmmo());

            theWeaponManager.ReloadPlay();

            Invoke(nameof(ReloadFinish), reloadTime);
        }
    }

    virtual protected void ReloadFinish()
    {
        // 가진 총알이 부족하면 가진만큼만 재장전하고 그렇지 않으면 전부 재장전
        int t_reloadBulletCount = (curTotalAmmo >= magazineAmmoMax)
                                ? magazineAmmoMax
                                : magazineAmmoMax - curTotalAmmo;

        curTotalAmmo -= t_reloadBulletCount;
        curMagazineAmmo = t_reloadBulletCount;

        HUDWeapon.instance.SetAmmoUI(GetCurAmmo(), GetMagazineMax(), GetTotalAmmo());

        isReload = false;
    }


    public float GetAcuuracy() { return accuracy; }
    public int GetCurAmmo() { return curMagazineAmmo; }
    public int GetMagazineMax() { return magazineAmmoMax; }
    public int GetTotalAmmo() { return curTotalAmmo; }

    public void SetTotalBullet(int p_value)
    {
        curTotalAmmo += p_value;
        if (curTotalAmmo >= totalAmmoMax)
            curTotalAmmo = totalAmmoMax;
    }

    public void SetCurrentBullet(int p_value)
    {
        curMagazineAmmo += p_value;
        if (curMagazineAmmo >= magazineAmmoMax)
            curMagazineAmmo = magazineAmmoMax;
    }

    abstract override protected void OnMouseButtonLeftDown();
    abstract override protected void OnMouseButtonLeft();
    abstract override protected void OnMouseButtonLeftUp();
}
