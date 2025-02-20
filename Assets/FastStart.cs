using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FastStart : MonoBehaviour
{
    public GameObject Loading;
    public Slider slider;
    public void FastStartMethod()
    {
        MyManager.instance.worldName = "0";
        MyManager.instance.seed = 0;
        Loading.SetActive(true);
        StartCoroutine(AsyncLoading());
    }
    IEnumerator AsyncLoading()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("SampleScene");
        asyncOperation.allowSceneActivation = true;
        while (!asyncOperation.isDone)
        {
            slider.value = asyncOperation.progress;

            yield return null;
        }
    }
}
