using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Ui_Save : MonoBehaviour
{
    const string save_Name = "RecordData";
    public static List<Archive> archives = new List<Archive>();
    public GameObject prefab;
    public GameObject ui_Main;
    public static List<string> names;
    static List<UiData_node> nodes = new List<UiData_node>();
    private void Awake()
    {
        var Data = SaveFunc.LoadJson<UiData>(save_Name);
        if (Data == null) { Data = new UiData(); names = new List<string>(); }
        else
        {
            foreach (var node in Data.nodeNames)
            {
                var nodedata = SaveFunc.LoadJson<UiData_node>("Record/" + node);
                nodes.Add(nodedata);
            }
            names = Data.nodeNames.ToList();
        }
        
        GameObject g;
        for (int i = 0; i < Data.count; i++)
        {
            g = Instantiate(prefab, ui_Main.transform);
            g.TryGetComponent<Archive>(out Archive component);
            component.SetData(nodes[i].name, nodes[i].seed);
            archives.Add(component);
        }
        g = Instantiate(prefab, ui_Main.transform);
        archives.Add(g.GetComponent<Archive>());
    }
    private void OnDestroy()
    {
        archives.Clear();
    }
    public static void RemoveRecord(string name)
    {
        nodes.RemoveAll(i => i.name == name);
        names.RemoveAll(i => i == name);
        Save();
    }
    public static void AddRecord(string name, int seed)
    {
        UiData_node n = new UiData_node(name, seed);
        nodes.Add(n);
        names.Add(name);
        Save();
        SaveFunc.SaveJson("Record/" + name, n );
    }
    public static void Save()
    {
        SaveFunc.SaveJson(save_Name, new UiData(names.ToArray()));
    }
    public static void SaveNode()
    {
        foreach (UiData_node node in nodes)
        {
            SaveFunc.SaveJson("Record/" + node.name, node);
        }
    }
}