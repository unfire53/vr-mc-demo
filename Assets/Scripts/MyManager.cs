using UnityEngine;
using UnityEngine.SceneManagement;

public class MyManager : MonoBehaviour
{
    public static MyManager instance;
    public string worldName;
    public int seed;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void Set(string name,int seed)
    {
        worldName = name;
        this.seed = seed;
    }
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
