using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject goBow = null;
    [SerializeField] GameObject goGun = null;

    [SerializeField] GameObject goCurrentWeapon = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (goCurrentWeapon == goBow) return;
            AllDeActive();
            ActiveWeapon(goBow);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (goCurrentWeapon == goGun) return;
            AllDeActive();
            ActiveWeapon(goGun);
        }
    }

    void AllDeActive()
    {
        goBow.SetActive(false);
        goGun.SetActive(false);
    }

    void ActiveWeapon(GameObject p_goWeapon)
    {
        p_goWeapon.SetActive(true);
        goCurrentWeapon = p_goWeapon;
    }
}
