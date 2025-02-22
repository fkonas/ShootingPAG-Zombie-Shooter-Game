using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Vehicles.Aeroplane;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SNIPER: MonoBehaviour
{
    Animator animator;

    [Header("Settings")]
    public bool atesEdebilirmi;
    float iceridenAtesEtmeSikligi;
    public float disaridanAtesEtmeSikligi;
    public float menzil;
    public GameObject Cross;
    public GameObject Scope;

    [Header("Audios")]
    public AudioSource atesSesi;
    public AudioSource ReloadingSound;
    public AudioSource emptyMagazine;
    public AudioSource mermiAlmaSesi;

    [Header("Effects")]
    public ParticleSystem atesEfekt;
    public ParticleSystem metalEfekti;
    public ParticleSystem kanEfekti;
    public ParticleSystem betonEfekti;
    public ParticleSystem plastikEfekti;

    [Header("Others")]
    public Camera benimCam;
    float camFieldPov;
    public float yaklasmaPov;

    [Header("Weapon Settings")]
    int toplamMermiSayisi;
    public int sarjorKapasitesi;
    int kalanMermi;
    public string silahinAdi;
    public Text toplamMermi_Text;
    public Text kalanMermi_Text;
    public float hitPower;

    public bool kovanCiksinmi;
    public GameObject kovanCikisNoktasi;
    public GameObject kovanObject;

    public AmmoBoxOlustur ammoBoxOlustur;
    public GameObject bulletPoint;
    public GameObject bullet;

    void Start()
    {
        toplamMermiSayisi = PlayerPrefs.GetInt(silahinAdi + "_Mermi");
        kovanCiksinmi = true;
        BaslangicMermiDoldur();
        SarjorDoldurmaFonk("NormalYaz");
        animator = GetComponent<Animator>();
        camFieldPov = benimCam.fieldOfView;
    }  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (atesEdebilirmi && Time.time > iceridenAtesEtmeSikligi && kalanMermi != 0)
            {
                if (!GameController.OyunDurdumu)
                {
                    AtesEt();
                    iceridenAtesEtmeSikligi = Time.time + disaridanAtesEtmeSikligi;
                }
            }
            
            if (kalanMermi==0)
            {
                emptyMagazine.Play();
            }

        }

        if (Input.GetKey(KeyCode.R) && kalanMermi < sarjorKapasitesi && toplamMermiSayisi != 0)
        {
            animator.Play("Reloading");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            MermiAl();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            KameraYaklastirScopeCikar(true);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            KameraYaklastirScopeCikar(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AmmoBox"))
        {
            MermiKaydet(other.transform.gameObject.GetComponent<AmmoBox>().olusanSilahinTuru, other.transform.gameObject.GetComponent<AmmoBox>().olusanMermiSayisi);
            ammoBoxOlustur.NoktalariKaldirma(other.transform.gameObject.GetComponent<AmmoBox>().noktasi);
            Destroy(other.transform.parent.gameObject);
        }
        if (other.gameObject.CompareTag("HealthBox"))
        {
            ammoBoxOlustur.GetComponent<GameController>().GetHealth();
            HealthBoxOlustur.HealthBoxVarmi = false;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.CompareTag("BomBox"))
        {
            ammoBoxOlustur.GetComponent<GameController>().GetBomb();
            BombBoxOlustur.BombBoxVarmi = false;
            Destroy(other.transform.gameObject);
        }
    }

    IEnumerator CameraShake(float shakeTime, float magnitude)
    {
        Vector3 orjPosition = benimCam.transform.localPosition;

        float gecenSure = 0.0f;
        while (gecenSure < shakeTime)
        {
            float x = Random.Range(-1f, .5f) * magnitude;

            benimCam.transform.localPosition = new Vector3(x, orjPosition.y, orjPosition.x);
            gecenSure += Time.deltaTime;
            yield return null;
        }

        benimCam.transform.localPosition = orjPosition;
    }

    void AtesEt()
    {
        AtesEtmeTeknikIslemler();

        RaycastHit hit;

        if (Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, menzil))
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                Instantiate(kanEfekti, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<Enemy>().EnemyHit(hitPower);
            }
            else if (hit.transform.gameObject.CompareTag("EmptyBarrel"))
            {
                Rigidbody rg = hit.transform.gameObject.GetComponent<Rigidbody>();
                rg.AddForce(-hit.normal * 100f);

                Instantiate(plastikEfekti, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Instantiate(metalEfekti, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    void AtesEtmeTeknikIslemler()
    {
        if (kovanCiksinmi)
        {
            GameObject obje = Instantiate(kovanObject, kovanCikisNoktasi.transform.position, kovanCikisNoktasi.transform.rotation);
            Rigidbody rb = obje.GetComponent<Rigidbody>();
            rb.AddRelativeForce(new Vector3(-10f, 1, 0) * 60);
        }

        Instantiate(bullet, bulletPoint.transform.position, bulletPoint.transform.rotation);
        StartCoroutine(CameraShake(.03f, .1f));
        atesSesi.Play();
        atesEfekt.Play();
        animator.Play("Fire");

        kalanMermi--;
        kalanMermi_Text.text = kalanMermi.ToString();
    }

    void MermiKaydet(string silahTuru, int mermiSayisi)
    {
        mermiAlmaSesi.Play();

        switch (silahTuru)
        {
            case "AK47":
                PlayerPrefs.SetInt("AK47_Mermi", PlayerPrefs.GetInt("AK47_Mermi") + mermiSayisi);
                break;

            case "Pompali":
                PlayerPrefs.SetInt("Pompali_Mermi", PlayerPrefs.GetInt("Pompali_Mermi") + mermiSayisi);
                break;

            case "Sniper":
                toplamMermiSayisi += mermiSayisi;
                PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);
                SarjorDoldurmaFonk("NormalYaz");
                break;

            case "Magnum":
                PlayerPrefs.SetInt("Magnum_Mermi", PlayerPrefs.GetInt("Magnum_Mermi") + mermiSayisi);
                break;


        }

    }

    void BaslangicMermiDoldur()
    {
        if (toplamMermiSayisi <= sarjorKapasitesi)
        {
            kalanMermi = toplamMermiSayisi;
            toplamMermiSayisi = 0;
            PlayerPrefs.SetInt(silahinAdi + "_Mermi", 0);
        }
        else
        {
            kalanMermi = sarjorKapasitesi;
            toplamMermiSayisi -= sarjorKapasitesi;
            PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);
        }
    }

    void SarjorDoldurmaFonk(string tur)
    {
        switch (tur)
        {
            case "MermiVar":
                if (toplamMermiSayisi <= sarjorKapasitesi)
                {
                    int olusanToplamDeger = kalanMermi + toplamMermiSayisi;

                    if (olusanToplamDeger > sarjorKapasitesi)
                    {
                        kalanMermi = sarjorKapasitesi;
                        toplamMermiSayisi = olusanToplamDeger - sarjorKapasitesi;
                        PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);
                    }
                    else
                    {
                        kalanMermi += toplamMermiSayisi;
                        toplamMermiSayisi = 0;
                        PlayerPrefs.SetInt(silahinAdi + "_Mermi", 0);
                    }
                }
                else
                {
                    toplamMermiSayisi -= sarjorKapasitesi - kalanMermi;
                    kalanMermi = sarjorKapasitesi;
                    PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);
                }

                toplamMermi_Text.text = toplamMermiSayisi.ToString();
                kalanMermi_Text.text = kalanMermi.ToString();
                break;

            case "MermiYok":
                if (toplamMermiSayisi <= sarjorKapasitesi)
                {
                    kalanMermi = toplamMermiSayisi;
                    toplamMermiSayisi = 0;
                    PlayerPrefs.SetInt(silahinAdi + "_Mermi", 0);
                }
                else
                {
                    toplamMermiSayisi -= sarjorKapasitesi;
                    PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);
                    kalanMermi = sarjorKapasitesi;                
                }

                toplamMermi_Text.text = toplamMermiSayisi.ToString();
                kalanMermi_Text.text = kalanMermi.ToString();
                break;

            case "NormalYaz":
                toplamMermi_Text.text = toplamMermiSayisi.ToString();
                kalanMermi_Text.text = kalanMermi.ToString();
                break;
        }
    }

    void ReloadSound()
    {
        ReloadingSound.Play();

        if (kalanMermi < sarjorKapasitesi && toplamMermiSayisi != 0)
        {
            if (kalanMermi != 0)
            {
                SarjorDoldurmaFonk("MermiVar");
            }
            else
            {
                SarjorDoldurmaFonk("MermiYok");
            }
        }
    }

    void MermiAl()
    {
        RaycastHit hit;

        if (Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, 3))
        {
            if (hit.transform.gameObject.CompareTag("AmmoBox"))
            {
                MermiKaydet(hit.transform.gameObject.GetComponent<AmmoBox>().olusanSilahinTuru, hit.transform.gameObject.GetComponent<AmmoBox>().olusanMermiSayisi);
                ammoBoxOlustur.NoktalariKaldirma(hit.transform.gameObject.GetComponent<AmmoBox>().noktasi);
                Destroy(hit.transform.parent.gameObject);
            }
        }
    }

    void KameraYaklastirScopeCikar(bool durum)
    {
        if (durum)
        {
            Cross.SetActive(false);
            benimCam.cullingMask = ~ (1 << 6);
            animator.SetBool("ZoomYap", durum);
            benimCam.fieldOfView = yaklasmaPov;
            Scope.SetActive(true);
        }
        else
        {
            Scope.SetActive(false);
            benimCam.cullingMask = -1;
            animator.SetBool("ZoomYap", durum);
            benimCam.fieldOfView = camFieldPov;
            Cross.SetActive(true);
        }
    }


}
