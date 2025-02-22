using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GameController : MonoBehaviour
{
    int aktifSira;
    float health = 100;

    [Header("Health Setting")]
    public Image HealthBar;

    [Header("Weapon Setting")]
    public GameObject[] weapons;
    public AudioSource changingSound;
    public GameObject Bomba;
    public GameObject BombPoint;
    public Camera bemimCam;

    [Header("Enemy Setting")]
    public GameObject[] enemies;
    public GameObject[] cikisNoktalari;
    public GameObject[] hedefNoktalari;
    public float dusmanCikmaSuresi;
    public Text kalanDusman_Text;
    public int baslangicDusmanSayisi;
    public static int kalanDusmanSayisi;

    [Header("Other Setting")]
    public GameObject GameOverCanvas;
    public GameObject YouWinCanvas;
    public GameObject PauseCanvas;
    public AudioSource InGameSound;
    public Text saglikSayi_Text;
    public Text bombaSayi_Text;
    public AudioSource ItemEnd;

    public static bool OyunDurdumu;


    void Start()
    {
        BaslangicIslemleri();
        Pause();
    }

    IEnumerator DusmanCikar()
    {
        while (true)
        {
            yield return new WaitForSeconds(dusmanCikmaSuresi);

            if (baslangicDusmanSayisi != 0)
            {
                int enemy = Random.Range(0, 5);
                int cikisNoktasi = Random.Range(0, 2);
                int hedefNoktasi = Random.Range(0, 2);

                GameObject Obje = Instantiate(enemies[enemy], cikisNoktalari[cikisNoktasi].transform.position, Quaternion.identity);
                Obje.GetComponent<Enemy>().HedefBelirle(hedefNoktalari[hedefNoktasi]);
                baslangicDusmanSayisi --;
            }
        }
    }

    void BaslangicIslemleri()
    {
        OyunDurdumu = false;


        if (!PlayerPrefs.HasKey("OyunBasladimi"))
        {
            PlayerPrefs.SetInt("AK47_Mermi", 100);
            PlayerPrefs.SetInt("Pompali_Mermi", 90);
            PlayerPrefs.SetInt("Sniper_Mermi", 80);
            PlayerPrefs.SetInt("Magnum_Mermi", 60);
            PlayerPrefs.SetInt("Saglik_Sayi", 2);
            PlayerPrefs.SetInt("Bomba_Sayi", 4);
            PlayerPrefs.SetInt("OyunBasladimi", 1);
        }

        kalanDusman_Text.text = baslangicDusmanSayisi.ToString();
        kalanDusmanSayisi = baslangicDusmanSayisi;

        saglikSayi_Text.text = PlayerPrefs.GetInt("Saglik_Sayi").ToString();
        bombaSayi_Text.text = PlayerPrefs.GetInt("Bomba_Sayi").ToString();

        aktifSira = 0;

        InGameSound.Play();

        StartCoroutine(DusmanCikar());
    }

    public void Dusman_sayisi_guncelle()
    {
        kalanDusmanSayisi--;

        if (kalanDusmanSayisi <= 0)
        {
            YouWinCanvas.SetActive(true);
            kalanDusman_Text.text = "0";
            Time.timeScale = 0;
            OyunDurdumu = true;
            Cursor.visible = true;
            GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            kalanDusman_Text.text = kalanDusmanSayisi.ToString();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !OyunDurdumu)
        {
            ChangeWeapons(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !OyunDurdumu)
        {
            ChangeWeapons(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !OyunDurdumu)
        {
            ChangeWeapons(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && !OyunDurdumu)
        {
            ChangeWeapons(3);
        }
        if (Input.GetKeyDown(KeyCode.Q) && !OyunDurdumu)
        {
            ChangeWeaponsWithQ();
        }
        if (Input.GetKeyDown(KeyCode.G) && !OyunDurdumu)
        {
            BombaAt();
        }
        if (Input.GetKeyDown(KeyCode.H) && !OyunDurdumu)
        {
            HealthFully();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void DarbeAl(float darbeGucu)
    {
        health -= darbeGucu;
        HealthBar.fillAmount = health / 100;
        if (health <= 0)
        {
            GameOver();
        }
    }

    public void HealthFully()
    {
        if (PlayerPrefs.GetInt("Saglik_Sayi")!=0 && health != 100)
        {
            health = 100;
            HealthBar.fillAmount = health / 100;
            PlayerPrefs.SetInt("Saglik_Sayi", PlayerPrefs.GetInt("Saglik_Sayi") - 1);
            saglikSayi_Text.text = PlayerPrefs.GetInt("Saglik_Sayi").ToString();
        }
        else
        {
            ItemEnd.Play();
        }
    }

    void BombaAt()
    {
        if (PlayerPrefs.GetInt("Bomba_Sayi") != 0)
        {
            GameObject obje = Instantiate(Bomba, BombPoint.transform.position, BombPoint.transform.rotation);
            Rigidbody rg = obje.GetComponent<Rigidbody>();
            Vector3 acimiz = Quaternion.AngleAxis(90, bemimCam.transform.forward) * bemimCam.transform.forward;
            rg.AddForce(acimiz * 250f);

            PlayerPrefs.SetInt("Bomba_Sayi", PlayerPrefs.GetInt("Bomba_Sayi") - 1);
            bombaSayi_Text.text = PlayerPrefs.GetInt("Bomba_Sayi").ToString();
        }
        else
        {
            ItemEnd.Play();
        }

    }

    public void GetHealth()
    {
        PlayerPrefs.SetInt("Saglik_Sayi", PlayerPrefs.GetInt("Saglik_Sayi") + 1);
        saglikSayi_Text.text = PlayerPrefs.GetInt("Saglik_Sayi").ToString();
    }

    public void GetBomb()
    {
        PlayerPrefs.SetInt("Bomba_Sayi", PlayerPrefs.GetInt("Bomba_Sayi") + 1);
        bombaSayi_Text.text = PlayerPrefs.GetInt("Bomba_Sayi").ToString();
    }

    void GameOver()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        OyunDurdumu = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }

    void ChangeWeapons(int siraNumarasi)
    {
        changingSound.Play();
        foreach(GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        aktifSira = siraNumarasi;   
        weapons[siraNumarasi].SetActive(true);
    }

    void ChangeWeaponsWithQ()
    {
        changingSound.Play();
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        if (aktifSira == 3)
        {
            aktifSira = 0;
            weapons[aktifSira].SetActive(true);
        }
        else
        {
            aktifSira++;
            weapons[aktifSira].SetActive(true);
        }
    }

    public void Reply()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        OyunDurdumu = false;
        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
        OyunDurdumu = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DevamEt()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        OyunDurdumu = false;
        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
