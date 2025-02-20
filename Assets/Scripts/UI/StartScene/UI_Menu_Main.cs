using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class UI_Menu_Main:MonoBehaviour
{
    public Button @return;
    public List<Archive> archives;
    public GameObject child;
    public int page;

    public event Action OnWelEnter;
    public event Action OnCreateEnter;
    public event Action OnLoadEnter;
    private void Start()
    {
        archives = Ui_Save.archives;
        @return.onClick.AddListener(WelEnter);
        foreach(Archive archive in archives)
        {
            archive.OnCreateEnter += CreateEnter;
            archive.OnLoadEnter += LoadEnter;
            archive.OnDelete += DeleteEnter;
        }
        Show();
    }

    private void DeleteEnter(Archive archive)
    {
        archives.Remove(archive);
        Show();
    }

    public void AddPage()
    {
        page++;
        if (page > archives.Count / 6)
        {
            page = archives.Count / 6;
            return;
        }
        Show();
    }
    public void MinusPage()
    {
        page--;
        if (page < 0)
        {
            page = 0;
            return;
        }
        Show();
    }

    private void Show()
    {
        foreach (Archive archive in archives)
        {
            archive.gameObject.SetActive(false);
        }
        for (int i = 0; i < 5 && i + page * 5 < archives.Count; i++)
        {
            archives[i+page * 5].gameObject.SetActive(true);
        }
    }

    void WelEnter()
    {
        child.SetActive(false);
        OnWelEnter();
    }
    private void CreateEnter()
    {
        child.SetActive(false);
        OnCreateEnter();
    }
    private void LoadEnter()
    {
        child.SetActive(false);
        OnLoadEnter();
    }
    public void SetActive()
    {
        child.SetActive(true);
    }
}
