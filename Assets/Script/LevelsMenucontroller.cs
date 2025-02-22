using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsMenucontroller : MonoBehaviour
{
    public Button level1_button, level2_button, level3_button, level4_button;

    public static bool level1, level2, level3, level4;

    public GameObject loadingPanel;

    public Slider loadingSlider;


    public void OyunBasla()
    {
        StartCoroutine(SceneLoading());
    }

    IEnumerator SceneLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);
        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float ilerleme = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = ilerleme;
            yield return null;
        }
    }

    private void Start()
    {
        level1 = true;
    }

    private void Update()
    {
        if (level2 == true)
        {
            level2_button.interactable = true;
        }

        if (level3 == true)
        {
            level3_button.interactable = true;
        }

        if (level4 == true)
        {
            level4_button.interactable = true;
        }
    }
}
