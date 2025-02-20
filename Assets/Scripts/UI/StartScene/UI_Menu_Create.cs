using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class UI_Menu_Create:MonoBehaviour
{
    [Header("parent")]
    public Button @return;
    public GameObject child;

    public event Action OnMainEnter;
    public event Action OnLoadEnter;

    [Header("creat button")]
    public Button create;
    public TMP_InputField worldNameText;
    public TMP_InputField seedText;
    
    private void Start()
    {
        @return.onClick.AddListener(MainEnter);
        create.onClick.AddListener(CreateThisWorld);
    }
    public void CreateThisWorld()
    {
        string name = worldNameText.text;
        int seed = int.Parse(seedText.text);
        Debug.Log("create method here");
        MyManager.instance.Set(name,seed);
        Ui_Save.AddRecord(name, seed);
        child.SetActive(false);
        OnLoadEnter();
    }
    void MainEnter()
    {
        child.SetActive(false);
        OnMainEnter();
    }
    public void SetActive()
    {
        child.SetActive(true);
    }
}
