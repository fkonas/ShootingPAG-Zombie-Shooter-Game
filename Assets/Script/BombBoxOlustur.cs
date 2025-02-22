using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBoxOlustur : MonoBehaviour
{
    public List<GameObject> BombBoxPoint = new List<GameObject>();
    public GameObject BombBoxKendisi;

    public static bool BombBoxVarmi;
    public float BombBoxOlusmaSuresi;

    int randomSayim;


    void Start()
    {
        BombBoxVarmi = false;
        StartCoroutine(BombBoxYap());
    }

    IEnumerator BombBoxYap()
    {
        while (true)
        {
            yield return new WaitForSeconds(BombBoxOlusmaSuresi);

            if (!BombBoxVarmi)
            {
                randomSayim = Random.Range(0, 6);

                Instantiate(BombBoxKendisi, BombBoxPoint[randomSayim].transform.position, BombBoxPoint[randomSayim].transform.rotation);
                BombBoxVarmi = true;
            }
        }
    }
}
