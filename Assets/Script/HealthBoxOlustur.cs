using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoxOlustur : MonoBehaviour
{
    public List<GameObject> HealthBoxPoint = new List<GameObject>();
    public GameObject HealthBoxKendisi;

    public static bool HealthBoxVarmi;
    public float HealthBoxOlusmaSuresi;

    int randomSayim;


    void Start()
    {
        HealthBoxVarmi = false;
        StartCoroutine(HealthBoxYap());
    }

    IEnumerator HealthBoxYap()
    {
        while (true)
        {
            yield return new WaitForSeconds(HealthBoxOlusmaSuresi);

            if (!HealthBoxVarmi)
            {
                randomSayim = Random.Range(0, 6);

                Instantiate(HealthBoxKendisi, HealthBoxPoint[randomSayim].transform.position, HealthBoxPoint[randomSayim].transform.rotation);
                HealthBoxVarmi = true;
            }
        }
    }
}
