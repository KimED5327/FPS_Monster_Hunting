using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] float reloadTime = 1.0f;

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


    bool isGrabArrow = false;
    bool isSettingArrow = false;

    void Start()
    {
        goArrowShow.SetActive(false);
        StartCoroutine(ReloadArrowCo());
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && isSettingArrow)
            ReadyToFire();

        if (Input.GetMouseButtonUp(0) && isGrabArrow && isSettingArrow)
            FireArrow();
    }

    void ReadyToFire()
    {
        isGrabArrow = true;
        StopAllCoroutines();
        goArrowShow.transform.position = Vector3.Lerp(goArrowShow.transform.position, tfArrowFullPowerPos.position, 0.03f);

        arrowSpeed += pullSpeed * Time.deltaTime;
        if (arrowSpeed >= fireSpeedMax)
            arrowSpeed = fireSpeedMax;

    }

    void FireArrow()
    {
        isGrabArrow = false;
        isSettingArrow = false;
        goArrowShow.SetActive(false);
        GameObject t_arrow = Instantiate(goArrowPrefab, goArrowShow.transform.position, goArrowShow.transform.rotation);
        t_arrow.GetComponent<Arrow>().SetArrow(arrowSpeed);
        StopAllCoroutines();
        StartCoroutine(ReloadArrowCo());
    }

    IEnumerator ReloadArrowCo()
    {
        yield return new WaitForSeconds(reloadTime);
        arrowSpeed = fireSpeedMin;
        goArrowShow.transform.position = tfArrowReloadPos.position;
        goArrowShow.SetActive(true);


        isSettingArrow = true;

        while (true)
        {
            if (goArrowShow == null) break;

            goArrowShow.transform.position = Vector3.Lerp(goArrowShow.transform.position, tfArrowOriginPos.position, 0.05f);
            yield return null;
        }

    }
}
