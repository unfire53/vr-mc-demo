using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TipObj_Queue : MonoBehaviour
{
    public static TipObj_Queue instance;

    public Queue<GameObject> prefabs = new Queue<GameObject>();
    public GameObject Prefab;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(this);
            }
        }
    }
    public GameObject GetPool()
    {
        GameObject gameObject;
        if (prefabs.Count > 0)
        {
            gameObject = prefabs.Dequeue();
            gameObject.SetActive(true);
            return gameObject;
        }
        gameObject = Instantiate(Prefab);
        return gameObject;
    }
    public void ReleasePool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        prefabs.Enqueue(gameObject);
    }
}
