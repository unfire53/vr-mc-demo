using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;



public class CubeWireRender : MonoBehaviour
{
    public static CubeWireRender instance;
    public Controler controler;
    public Transform cubeTrans;
    public Vector3 startPos;
    public Vector3 endPos;
    public GameObject Ui;
    public List<GameObject> gameObjects = new List<GameObject>();
    public List<ChunkData> chunkDatas = new List<ChunkData>();
    public List<Feature> features = new List<Feature>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }
    }
    private void Start()
    {
        controler.OnStartDraw += StartDraw;
        controler.OnDraw += OnDraw;
        controler.OnEndDraw += EndDraw;
    }
    void StartDraw(Vector3 startPos)
    {
        this.startPos = startPos;
        gameObjects.Clear();
        chunkDatas.Clear();
        cubeTrans.localScale = Vector3.zero;
        cubeTrans.gameObject.SetActive(true);
        Ui.SetActive(false);
    }
    void OnDraw(Vector3 endPos)
    {
        this.endPos = endPos;
        if (Vector3.Distance(startPos, endPos) < 3f) return;
        Vector3 scale = new Vector3(Mathf.Abs(startPos.x - endPos.x), Mathf.Abs(startPos.y - endPos.y), Mathf.Abs(startPos.z - endPos.z));
        Vector3 center = (startPos + endPos) / 2;
        cubeTrans.localScale = scale;
        cubeTrans.position = center;
    }
    private void EndDraw()
    {
        Vector3 extents = new Vector3(Mathf.Abs((startPos.x - endPos.x) / 2), Mathf.Abs(startPos.y - endPos.y) / 2, Mathf.Abs(startPos.z - endPos.z) / 2);
        Physics.queriesHitBackfaces = true;
        RaycastHit[] hits = Physics.BoxCastAll((startPos + endPos) / 2, extents, Vector3.down, Quaternion.identity, 0, LayerMask.GetMask("Terrain"));
        Physics.queriesHitBackfaces = false;
        Debug.Log(hits.Length);
        if (hits.Length <= 0)
        {
            cubeTrans.gameObject.SetActive(false);
            return;
        }
        foreach (RaycastHit hit in hits)
        {
            gameObjects.Add(hit.transform.gameObject);
            World.ChunkDatas.TryGetValue(Vector3Int.FloorToInt(hit.transform.position), out ChunkData c);
            chunkDatas.Add(c);
            Debug.Log(hit.transform.position);
        }
        float s = (extents.x + extents.y + extents.z) * 0.6f;
        Vector3 scale = new Vector3(s, s, s);
        Ui.transform.localScale = scale;
        Ui.SetActive(true);
    }

    public void GetWoodStruct()
    {
        Vector3 center = cubeTrans.position;
        float length = (0.5f * (startPos - endPos)).sqrMagnitude;
        foreach (ChunkData data in chunkDatas)
        {
            foreach (Feature feature in data.structPos)
            {
                if (Feature.GetSturctName(feature) == "tree_1" && !features.Contains(feature))
                {
                    Vector3 point = Feature.Get_WorldPoint(feature, data);
                    float dis = (point - center).sqrMagnitude;
                    if (dis >= length) continue;
                    features.Add(feature);
                    GameObject P = TipObj_Queue.instance.GetPool();
                    P.transform.position = point + new Vector3(0, 9, 0);
                    ResourcePool.Instance.InsertResource(feature, P);
                }
            }
        }
    }
    public void GetStoneStruct()
    {
        Vector3 center = cubeTrans.position;
        float length = (0.5f * (startPos - endPos)).sqrMagnitude;
        foreach (ChunkData data in chunkDatas)
        {
            foreach (Feature feature in data.structPos)
            {
                if ((Feature.GetSturctName(feature) == "Coal" || Feature.GetSturctName(feature) == "Iron") && !features.Contains(feature))
                {
                    Vector3 point = Feature.Get_WorldPoint(feature, data);
                    float dis = (point - center).sqrMagnitude;
                    if (dis >= length) continue;
                    features.Add(feature);
                    GameObject P = TipObj_Queue.instance.GetPool();
                    P.transform.position = point + new Vector3(0, 3, 0);
                    ResourcePool.Instance.InsertResource(feature, P);
                }
            }
        }
    }
    
    public int GetRectLength(int from, int to, int c, out int start_Pos)
    {
        int a1, a2;
        a1 = c;
        a2 = c + 16;
        int b1, b2;
        if (from < to)
        {
            b1 = from;
            b2 = to;
        }
        else
        {
            b1 = to;
            b2 = from;
        }
        int a = a1 > b1 ? a1 : b1;
        //取a1、b1的最大值作为重叠部分的左（或下）端点

        int b = a2 < b2 ? a2 : b2;
        //取a2、b2的最小值作为重叠部分的右（或上）端点
        start_Pos = a%16;
        if (start_Pos < 0)
            start_Pos = 16 + start_Pos;
        return b - a;
        //得出重叠部分的长度作为返回值
    }
    public void Clear()
    {
        Vector3 center = cubeTrans.position;
        float length = (0.5f * (startPos - endPos)).sqrMagnitude;
        List<Feature> f = new List<Feature>();
        foreach (ChunkData data in chunkDatas)
        {
            foreach (Feature feature in data.structPos)
            {
                float dis = (Feature.Get_WorldPoint(feature, data) - center).sqrMagnitude;
                if (dis >= length) continue;
                if (!features.Contains(feature)) continue;
                f.Add(feature);
            }
        }
        ResourcePool.Instance.RemoveResource(f);
    }
}
