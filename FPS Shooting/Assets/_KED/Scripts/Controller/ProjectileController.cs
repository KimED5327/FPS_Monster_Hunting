using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] Transform tfFirePos = null;

    [SerializeField] GameObject goGranade = null;
    [SerializeField] float throwForce = 10f;

    [SerializeField] int granadeCount = 2;
    

    void Start()
    {
        HUDWeapon.instance.SetGranadeUI(granadeCount);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && granadeCount > 0)
        {
            granadeCount--;
            HUDWeapon.instance.SetGranadeUI(granadeCount);
            GameObject t_clone = Instantiate(goGranade, tfFirePos.position, tfFirePos.rotation);
            t_clone.GetComponent<Rigidbody>().velocity = (tfFirePos.forward + (tfFirePos.up * 0.5f)) * throwForce;
            int t_randomRange = 10;
            t_clone.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-t_randomRange, t_randomRange), Random.Range(-t_randomRange, t_randomRange), Random.Range(-t_randomRange, t_randomRange));
        }        
    }
}
