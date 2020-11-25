using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    float speed;

    Rigidbody myRigid;


    public void SetArrow(float p_speed)
    {
        speed = p_speed;
        myRigid.velocity = transform.forward * speed;
    }

    // Start is called before the first frame update
    void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(myRigid.useGravity)
            transform.forward = myRigid.velocity;
    }


    private void OnCollisionEnter(Collision collision)
    {
        myRigid.useGravity = false;
        GetComponent<Collider>().enabled = false;
        myRigid.velocity = Vector3.zero;
        myRigid.angularVelocity = Vector3.zero;
        transform.position = collision.GetContact(0).point;
        Destroy(gameObject, 5f);
    }
}
