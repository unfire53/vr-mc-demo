using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using static UnityEngine.Rendering.PostProcessing.PostProcessLayer;

public class GraphicsSetting : MonoBehaviour
{
    public PXR_Manager manager;
    public PostProcessLayer volume;
    public void SuperResolution()
    {
        manager.enableSuperResolution = true;
        Debug.Log("manager.enableSuperResolution:  " + manager.enableSuperResolution);
    }
    public void DisableSuperResolution()
    {
        manager.enableSuperResolution = false;
        Debug.Log("manager.enableSuperResolution:  " + manager.enableSuperResolution);
    }
    public void Msaa()
    {
        manager.useRecommendedAntiAliasingLevel = !manager.useRecommendedAntiAliasingLevel;
        Debug.Log("manager.useRecommendedAntiAliasingLeve:  " + manager.useRecommendedAntiAliasingLevel);
    }
    public void Fxaa()
    {
        volume.antialiasingMode = Antialiasing.FastApproximateAntialiasing;
        volume.fastApproximateAntialiasing.keepAlpha = true;
        Debug.Log(volume.antialiasingMode.ToString());
    }
    public void CloseFxaa()
    {
        volume.antialiasingMode = Antialiasing.None;
        Debug.Log(volume.antialiasingMode.ToString());
    }
    public void AnisotropicFiltering()
    {
        QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.ForceEnable;
        Debug.Log(QualitySettings.anisotropicFiltering.ToString());
    }
    public void CloseAnisotropicFiltering()
    {
        QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.Disable;
        Debug.Log(QualitySettings.anisotropicFiltering.ToString());
    }
}
