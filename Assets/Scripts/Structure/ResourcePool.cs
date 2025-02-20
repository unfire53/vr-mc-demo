using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Resource
{
    public Feature feature;
    public GameObject gameObject;

    public Resource(Feature feature, GameObject @object)
    {
        this.feature = feature;
        gameObject = @object;
    }


}
/// <summary>
/// 事件的拥有者 resourcepool
/// 事件 OnResourceLoaded
/// 事件的响应者 Fsm
/// 事件响应函数 GetWorkPos
/// 订阅关系
/// </summary>
public class ResourcePool :MonoBehaviour
{
    public static ResourcePool Instance;
    public List<Resource> resources = new List<Resource>();
    //public List<Feature> resources = new List<Feature>();
    public event Action OnResourceLoaded;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else
        {
            if(Instance != this)
            {
                Destroy(this);
            }
        }
    }
    private void Start()
    {
        StartCoroutine(SendResourceMessage());
    }
    IEnumerator SendResourceMessage()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            if(resources.Count > 0 && OnResourceLoaded !=null)
            {
                OnResourceLoaded();
            }
        }
    }
    public void InsertResource(Feature feature, GameObject obj)
    {
        Resource resource = new Resource(feature, obj);
        resources.Insert(0, resource);
    }
    public void InsertResource(Feature feature)
    {
        GameObject obj = TipObj_Queue.instance.GetPool();
        Resource resource = new Resource(feature, obj);
        resources.Insert(0, resource);
    }
    public void RemoveResource(Feature feature)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].feature == feature)
            {
                TipObj_Queue.instance.ReleasePool(resources[i].gameObject);
                CubeWireRender.instance.features.Remove(resources[i].feature);
                resources.RemoveAt(i);
            }
        }
    }
    public void RemoveResource(List<Feature> features)
    {
        for (int i = 0; i < features.Count; i++)
        {
            RemoveResource(features[i]);
        }
    }
    public void RemoveResource()
    {
        int i = resources.Count - 1;
        TipObj_Queue.instance.ReleasePool(resources[i].gameObject);
        CubeWireRender.instance.features.Remove(resources[i].feature);
        resources.RemoveAt(i);
    }
    public void RemoveResource(Resource r)
    {
        TipObj_Queue.instance.ReleasePool(r.gameObject);
        CubeWireRender.instance.features.Remove(r.feature);
        resources.Remove(r);
    }
    public IResource GetResource()
    {
        for (int i = resources.Count - 1; i >= 0; i--)
        {
            if (Instance.resources[i].feature.GetInterface(out IResource resource))
            {
                return resource;
            }
        }
        return null;
        //if (resources.Count == 0)return null;
        //int i = resources.Count - 1;
        //IResource r = Instance.resources[i].feature;
        //return r;
    }

}
