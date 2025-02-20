using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Edge> edges = new List<Edge>();

    public Vector3Int pos = new Vector3Int();

    public float h, g, f;
    public Node frontNode;
    public Node() { }
    public Node(Vector3 position)
    {
        pos.x = Mathf.FloorToInt(position.x);
        pos.y = Mathf.FloorToInt(position.y);
        pos.z = Mathf.FloorToInt(position.z);
    }
    public Node(int x, int y, int z)
    {
        pos.x = x;
        pos.y = y;
        pos.z = z;
    }
    
}
