using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float bombWaitTime = 2.5f;
    [SerializeField] GameObject goEffect = null;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Explosion), bombWaitTime);
    }

    void Explosion()
    {
        Destroy(gameObject);
        Instantiate(goEffect, transform.position, Quaternion.identity);
    }
}
