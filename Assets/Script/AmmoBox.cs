using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBox : MonoBehaviour
{
    string[] silahlar =
    {
        "AK47",
        "Pompali",
        "Sniper",
        "Magnum"        
    };

    int[] mermiSayisi =
    {
        20,
        5,
        6,
        6
    };

    public List<Sprite> silahResimleri = new List<Sprite>();
    public Image silahResmi;

    public string olusanSilahinTuru;
    public int olusanMermiSayisi;
    public int noktasi;

    void Start()
    {
        int gelenAnahtar = Random.Range(0, silahlar.Length);

        olusanSilahinTuru = silahlar[gelenAnahtar];
        olusanMermiSayisi = mermiSayisi[Random.Range(0, mermiSayisi.Length)];

        silahResmi.sprite = silahResimleri[gelenAnahtar];


        //olusanSilahinTuru = "AKM";
        //olusanMermiSayisi = 30;
    }






}
