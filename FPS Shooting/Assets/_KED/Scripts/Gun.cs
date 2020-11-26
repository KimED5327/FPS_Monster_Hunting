using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : RangedWeapon
{
    [Tooltip("Automatic rifle? or Single Shot Gun?"), SerializeField]
    bool isAutomatic = false;

    [Header("Effect")]
    [SerializeField] GameObject goImpactEffect = null;
    [SerializeField] ParticleSystem psMuzzleFlash = null;

    [Header("Recoil"), SerializeField] float recoilPosZ = -0.3f;
    [SerializeField] float recoilSpeed = 0.25f;
    [SerializeField] float recoilBackSpeed = 0.15f;
    float originPosZ;

    void Start()
    {
        originPosZ = transform.localPosition.z;
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }


    protected override void BulletCreate()
    {
        StopAllCoroutines();
        StartCoroutine(GunRecoil());
        psMuzzleFlash.Play();

        float t_randomX = Random.Range(-theCrosshair.GetAccuracy(), theCrosshair.GetAccuracy());
        float t_randomY = Random.Range(-theCrosshair.GetAccuracy(), theCrosshair.GetAccuracy());

        Vector3 dir = (cam.transform.forward + (cam.transform.up) * t_randomY + (cam.transform.right) * t_randomX).normalized;
        if (Physics.Raycast(cam.transform.position, dir, out RaycastHit hit, 1000f))
        {
            if (hit.transform.CompareTag("Enemy"))
                hit.transform.GetComponent<EnemyFSM>().Damage(damage);

            GameObject t_effect = Instantiate(goImpactEffect, hit.point, Quaternion.Euler(hit.normal));
            Destroy(t_effect, 5f);
        }

        theCrosshair.ShootState();
    }

    protected override void OnMouseButtonLeftDown()
    {
        if (!isAutomatic)
            base.Fire();
    }

    protected override void OnMouseButtonLeft()
    {
        if (isAutomatic)
            base.Fire();
    }

    protected override void OnMouseButtonLeftUp()
    {
        ;
    }

    IEnumerator GunRecoil()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, originPosZ);
        while(Mathf.Abs(transform.localPosition.z - recoilPosZ) > 0.01f)
        {
            Vector3 t_dest = transform.localPosition;
            t_dest.z = recoilPosZ;
            transform.localPosition = Vector3.Lerp(transform.localPosition, t_dest, recoilSpeed);
            yield return null;
        }

        while(Mathf.Abs(transform.localPosition.z - originPosZ) > 0.01f)
        {
            Vector3 t_dest = transform.localPosition;
            t_dest.z = originPosZ;
            transform.localPosition = Vector3.Lerp(transform.localPosition, t_dest, recoilBackSpeed);
            yield return null;
        }
    }
}
