using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UI_Welcome : MonoBehaviour
{
    public Button startButton;
    public Button quitButtton;
    public GameObject child;

    public event Action OnMainEnter;
    private void Start()
    {
        startButton.onClick.AddListener(Close);
        quitButtton.onClick.AddListener(Quit);
    }
    void Close()
    {
        child.SetActive(false);
        OnMainEnter();
    }
    public void SetActive()
    {
        child.SetActive(true);
    }
    void Quit()
    {
        Application.Quit();
    }
}
