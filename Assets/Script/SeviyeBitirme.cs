using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeviyeBitirme : MonoBehaviour
{

    public GameObject loadingPanel;
    public Slider loadingSlider;

    public void level1_bitir()
    {
        LevelsMenucontroller.level2 = true;
        StartCoroutine(SceneLoading2());
        GameController.OyunDurdumu = false;
    }

    public void level2_bitir()
    {
        LevelsMenucontroller.level3 = true;
        StartCoroutine(SceneLoading3());
    }

    public void level3_bitir()
    {
        LevelsMenucontroller.level4 = true;
        StartCoroutine(SceneLoading4());
    }

    public void level4_bitir()
    {
        StartCoroutine(SceneLoading5());
    }

    IEnumerator SceneLoading2()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(3);
        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float ilerleme = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = ilerleme;
            yield return null;
        }
    }

    IEnumerator SceneLoading3()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(4);
        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float ilerleme = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = ilerleme;
            yield return null;
        }
    }

    IEnumerator SceneLoading4()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(5);
        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float ilerleme = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = ilerleme;
            yield return null;
        }
    }

    IEnumerator SceneLoading5()
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
}
