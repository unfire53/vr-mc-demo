using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Loading : MonoBehaviour
{
    public GameObject child;
    public Slider slider;
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
    public void SetActive()
    {
        child.SetActive(true);
        StartCoroutine(AsyncLoading());
    }
}
