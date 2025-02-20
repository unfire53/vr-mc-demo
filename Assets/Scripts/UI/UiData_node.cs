using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class UiData
{
    public int count;
    public string[] nodeNames;
    public UiData(string[] strings)
    {
        count = strings.Length;
        nodeNames = strings;
    }
    public UiData() { }
}
[Serializable]
public class UiData_node
{
    public string name;
    public int seed;

    public UiData_node(string name,int seed)
    {
        this.name = name;
        this.seed = seed;
    }
    public UiData_node()
    {
        name = "";
        seed = 0;
    }
}
