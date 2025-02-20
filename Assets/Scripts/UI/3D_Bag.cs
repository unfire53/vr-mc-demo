using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

public class _3D_Bag : MonoBehaviour
{
    public List<GameObject> socketPres = new List<GameObject>();
    public BlockType[] blockTypes;
    public World world;
    public GameObject socketPre;
    int num;
    int page;
    int pace = 0;
    public void GetBlockTypes()
    {
        blockTypes = world.blockTypes;
    }
    public void DestoryOldObject()
    {
        for (int i = socketPres.Count - 1; i >= 0; i--)
        {
            GameObject obj = socketPres[i];
            if (obj != null)
            {
                DestroyImmediate(obj);
            }
        }
        socketPres.Clear(); // 清空列表

        blockTypes = null;
        num = 0;
        page = 0;
    }
    public void Generate()
    {
        for (int i = 0; i < blockTypes.Length; i++)
        {
            BlockName name = (BlockName)i;
            if (name == BlockName.Air || name == BlockName.Water || name == BlockName.Bed || name == BlockName.Wheat_1 || name == BlockName.Wheat_2 || name == BlockName.Wheat_3) continue;
            GameObject gameObject = Instantiate(socketPre, transform);
            gameObject.name = num++.ToString();
            socketPres.Add(gameObject);
            Transform BulidVoxel = gameObject.transform.GetChild(1);
            if (BulidVoxel.gameObject.TryGetComponent<BulidVoxel>(out BulidVoxel component)) component.Set(world, name);
        }
        Show();
    }
    public void AddPage()
    {
        page++;
        if (page > socketPres.Count / 11) { page = socketPres.Count / 11; return; }

        Vector3 vector3 = transform.localPosition;
        vector3.y -= 3;
        transform.localPosition = vector3;

        Show();
    }
    public void MinusPage()
    {
        page--;
        if (page < 0) { page = 0; return; }
        Vector3 vector3 = transform.localPosition;
        vector3.y += 3;
        transform.localPosition = vector3;

        Show();
    }
    public void Show()
    {
        foreach (GameObject @object in socketPres)
        {
            @object.SetActive(false);
        }
        for (int i = 0; i < 10 && i + 10 * page < socketPres.Count; i++)
        {
            socketPres[page * 10 + i].SetActive(true);
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(_3D_Bag))]
public class _3D_Bag_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _3D_Bag bag = (_3D_Bag)target;
        if (GUILayout.Button("Update BlockType"))
        {
            bag.DestoryOldObject();
            bag.GetBlockTypes();
            bag.Generate();
        }
        if (GUILayout.Button("add"))
        {
            bag.AddPage();
        }
        if (GUILayout.Button("minus"))
        {
            bag.MinusPage();
        }
    }
}


#endif
