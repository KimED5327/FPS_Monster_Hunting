using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    MELEE,
    RANGED,
}

public abstract class Weapon : MonoBehaviour
{
    protected Camera cam;
    [Header("Basic Info")]
    [SerializeField] protected string weaponName = "";
    [SerializeField] protected int damage = 0;
    [SerializeField] protected WeaponType weaponType = WeaponType.MELEE;

    protected bool isActivate = false;

    protected HUDCrosshair theCrosshair;
    protected WeaponManager theWeaponManager;
    protected void Awake()
    {
        theWeaponManager = FindObjectOfType<WeaponManager>();
        theCrosshair = FindObjectOfType<HUDCrosshair>();
        cam = Camera.main;
    }


    virtual protected void Update()
    {
        if (isActivate)
        {
            if (Input.GetMouseButtonDown(0))
                OnMouseButtonLeftDown();

            if (Input.GetMouseButton(0))
                OnMouseButtonLeft();

            if (Input.GetMouseButtonUp(0))
                OnMouseButtonLeftUp();
        }
    }

    abstract protected void OnMouseButtonLeftDown();
    abstract protected void OnMouseButtonLeft();
    abstract protected void OnMouseButtonLeftUp();


    public WeaponType GetWeaponType() { return weaponType; }
    public string GetWeaponName() { return weaponName; }
    public void SetActivate(bool p_flag) { isActivate = p_flag; }
}
