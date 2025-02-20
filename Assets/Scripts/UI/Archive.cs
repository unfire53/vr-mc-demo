using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Archive : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button arcButton;
    public Button deleteButton;
    public string worldName;
    public int seed;
    public int id;

    public event Action OnCreateEnter;
    public event Action OnLoadEnter;
    public event Action<Archive> OnDelete;
    private void Awake()
    {
        arcButton.onClick.AddListener(Check);
        deleteButton.onClick.AddListener(Delete);
    }
    private void OnDestroy()
    {
        arcButton.onClick.RemoveListener(Check);
        deleteButton.onClick.RemoveListener(Delete);
    }
    private void Delete()
    {
        if(SaveFunc.DeleteJson("Record/" + worldName))
        {
            Ui_Save.RemoveRecord(worldName);
            OnDelete(this);
            Destroy(gameObject);
        } 
    }

    private void Check()
    {
        if(worldName == "")
            OnCreateEnter();
        else
        {
            MyManager.instance.Set(worldName, seed);
            OnLoadEnter();
        }
        //∂¡»°∑Ω∑®
    }
    public void SetData(string name,int seed)
    {
        this.worldName = name;
        this.seed = seed;
        UiUpdate();
    }
    public void UiUpdate()
    {
        if (worldName == "")
            text.text = "Empty";
        else
            text.text = worldName;
    }
}
