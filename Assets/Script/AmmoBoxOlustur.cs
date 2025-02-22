using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxOlustur : MonoBehaviour
{
    public List<GameObject> AmmoBoxPoint = new List<GameObject>();
    public GameObject AmmoBoxKendisi;

    public static bool AmmoBoxVarmi;
    public float AmmoBoxOlusmaSuresi;

    List<int> noktalar = new List<int>();
    
    void Start()
    {
        AmmoBoxVarmi = false;
        StartCoroutine(AmmoBoxYap());
    }

    IEnumerator AmmoBoxYap()
    {
        while (true)
        {
            yield return new WaitForSeconds(AmmoBoxOlusmaSuresi);

            int randomSayim = Random.Range(0, 5);

            if (!noktalar.Contains(randomSayim))
            {
                noktalar.Add(randomSayim);
            }
            else
            {
                randomSayim = Random.Range(0, 5);
                continue;
            }

            GameObject objem = Instantiate(AmmoBoxKendisi, AmmoBoxPoint[randomSayim].transform.position, AmmoBoxPoint[randomSayim].transform.rotation);
            objem.transform.gameObject.GetComponentInChildren<AmmoBox>().noktasi = randomSayim;
        }
    }

    public void NoktalariKaldirma(int deger)
    {
        noktalar.Remove(deger);
    }

}
