using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Utility;

public class Bomba : MonoBehaviour
{
    public float guc = 10f;
    public float menzil = 5f;
    public float yukariGuc = 1f;
    public ParticleSystem patlamaEfekti;
    AudioSource patlamaSesi;

    void Start()
    {
        patlamaSesi = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision != null)
            {
                Destroy(gameObject, 1f);
                Patlama();
            }
        }
    }

    void Patlama()
    {
        Vector3 patlamapozisyonu = transform.position;
        Instantiate(patlamaEfekti, transform.position, transform.rotation);
        patlamaSesi.Play();
        Collider[] colliders = Physics.OverlapSphere(patlamapozisyonu, menzil);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (hit != null && rb)
            {
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    hit.transform.gameObject.GetComponent<Enemy>().Die();
                }

                rb.AddExplosionForce(guc, patlamapozisyonu, menzil, .5f, ForceMode.Impulse);
            }
        }
    }

}
