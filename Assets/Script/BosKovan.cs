using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosKovan : MonoBehaviour
{
    public AudioSource yereDusmeSesi;

    void Start()
    {
       Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            yereDusmeSesi.Play();

            if (!yereDusmeSesi.isPlaying)
            {
                Destroy(gameObject, 4f);
            }
        }
    }

}
