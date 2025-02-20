using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UI_StartScene_Manager : MonoBehaviour
{
    //public UiData uiData;

    public UI_Welcome welcome;
    public UI_Menu_Main main;
    public UI_Menu_Create create;
    public UI_Loading loading;

    private void Start()
    {
        welcome.OnMainEnter += main.SetActive;
        
        main.OnWelEnter += welcome.SetActive;
        main.OnCreateEnter += create.SetActive;
        main.OnLoadEnter += loading.SetActive;

        create.OnMainEnter += main.SetActive;
        create.OnLoadEnter += loading.SetActive;
    }
}
