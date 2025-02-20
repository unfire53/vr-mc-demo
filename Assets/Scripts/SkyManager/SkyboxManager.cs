using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public Material daySkybox;
    public Material nightSkybox;

    TimeManager timeManager;

    private Material currentSkybox;
    private float transitionProgress = 0f;
    private float transitionSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        currentSkybox = new Material(daySkybox);
        timeManager = TimeManager.instance;
        RenderSettings.skybox = currentSkybox;
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = timeManager.GetGameTime();
        float hour = currentTime % 24;

        if(hour >= 6 && hour < 20)
        {
            transitionProgress = Mathf.Lerp(transitionProgress, 0, transitionSpeed * Time.deltaTime);
        }else
        {
            transitionProgress = Mathf.Lerp(transitionProgress, 1f, transitionSpeed * Time.deltaTime);
        }
        currentSkybox.Lerp(daySkybox,nightSkybox, transitionProgress);
    }

}
