using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    public GameObject loadingPanel;
    public GameObject quitPanel;

    public Slider loadingSlider;



    public void OyunBasla()
    {
        StartCoroutine(SceneLoading());
    }

    IEnumerator SceneLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float ilerleme = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = ilerleme;
            yield return null;
        }
    }

    public void OyundanCik()
    {
        quitPanel.SetActive(true);
        Application.Quit();
    }

    public void Tercih(string buttonDeger)
    {
        if (buttonDeger == "yeap")
        {
            Application.Quit();
        }
        else
        {
            quitPanel.SetActive(false);
        }
    }
}
