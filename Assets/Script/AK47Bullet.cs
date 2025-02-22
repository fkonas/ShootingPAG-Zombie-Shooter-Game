using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class AK47Bullet : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * 250;
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
