using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Graph
{
    public List<Node> nodes = new List<Node>();
    public List<Edge> edges = new List<Edge>();
    Vector3Int[] dir = new Vector3Int[6]
    {
        Vector3Int.forward,
        Vector3Int.back,
        Vector3Int.left,
        Vector3Int.right,
        Vector3Int.up,
        Vector3Int.down
    };

    public Graph() { }
    public void AddNode(Vector3Int pos)
    {
        if (FindNode(pos) == null)
        {
            Node node = new Node(pos);
            nodes.Add(node);
        }
    }
    public Node FindNode(Vector3Int pos)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (pos == nodes[i].pos)
            {
                return nodes[i];
            }
        }
        return null;
    }
    public void AddEdge(Vector3Int start, Vector3Int end)
    {
        Node startNode = FindNode(start);
        Node endNode = FindNode(end);
        AddEdge(startNode, endNode);
    }
    public void AddEdge(Node startNode, Node endNode)
    {
        if (startNode != null && endNode != null)
        {
            Edge edge1 = new Edge(startNode, endNode);
            edges.Add(edge1);
            startNode.edges.Add(edge1);
            Edge edge2 = new Edge(endNode, startNode);
            edges.Add(edge2);
            endNode.edges.Add(edge2);
        }
    }

    public int GetPathLength(List<Node> path)
    {
        return path.Count;
    }
    public bool Astar(Vector3Int startPos, Vector3Int endPos, List<Node> path)
    {
        int count = 0;
        nodes.Clear();
        edges.Clear();
        AddNode(startPos);
        AddNode(endPos);
        Node startNode = FindNode(startPos);
        Node endNode = FindNode(endPos);
        path.Clear();
        List<Node> open = new List<Node>();
        List<Node> close = new List<Node>();
        float current_g = 0;
        bool current_isButter = false;

        startNode.g = 0;
        startNode.h = Vector3.SqrMagnitude(startNode.pos - endNode.pos);
        startNode.f = startNode.h;

        open.Add(startNode);
        while (open.Count > 0)
        {
            count++;
            if(count >= 1000)
            {
                return false;
            }
            int i = LowestF(open);
            Node thisNode = open[i];

            if (thisNode.pos == endNode.pos)
            {
                reconstructPath(path, startNode, endNode);
                return true;
            }
            open.RemoveAt(i);
            close.Add(thisNode);
            Node neighborNode;
            for (int j = dir.Length - 1; j >= 0; j--)
            {
                Vector3Int neighborPos = dir[j] + thisNode.pos;
                /////先检查是否为空气且没到终点
                if (!World.CheckPosCanWalk(neighborPos) && neighborPos != endNode.pos)
                    continue;
                //////检查是否太高
                if (World.CheckOverHeight(neighborPos))
                    continue;

                neighborNode = FindNode(neighborPos);
                if (neighborNode == null)
                {
                    AddNode(neighborPos);
                    neighborNode = FindNode(neighborPos);
                }
                AddEdge(thisNode, neighborNode);
                if(neighborNode.g == 0)
                    neighborNode.g = thisNode.g + 1;
                /////是否在close列表里
                if (close.IndexOf(neighborNode) > -1)
                {
                    continue;
                }

                /////其他情况
                current_g = thisNode.g + 1;
                if (open.IndexOf(neighborNode) == -1)
                {
                    open.Add(neighborNode);
                    current_isButter = true;
                }
                else if (current_g <= neighborNode.g )
                {
                    current_isButter = true;
                }
                else
                {
                    current_isButter = false;
                }
                if (current_isButter)
                {
                //    if (thisnode_buttom_isempty)
                //        neighborNode.frontNode = thisNode.frontNode;
                //    else
                    neighborNode.frontNode = thisNode;
                    neighborNode.g = current_g;
                    neighborNode.h = Vector3.SqrMagnitude(neighborNode.pos - endNode.pos);
                    neighborNode.f = neighborNode.g + neighborNode.h;
                    current_isButter = false;
                }

                //////如果是下面且不在close列表里，只添加下面节点
                if (j == dir.Length - 1)
                {
                    break;
                }
            }
        }
        return false;
    }
    public bool Astar_bidirectional(Vector3Int startPos, Vector3Int endPos, List<Node> path)
    {
        int count = 0;
        nodes.Clear();
        edges.Clear();
        AddNode(startPos);
        AddNode(endPos);
        Node startNode = FindNode(startPos);
        Node endNode = FindNode(endPos);
        path.Clear();
        List<Node> open_Front = new List<Node>();
        List<Node> close_Front = new List<Node>();
        List<Node> open_Back = new List<Node>();
        List<Node> close_Back = new List<Node>();
        float current_g = 0;
        bool current_isButter = false;

        startNode.g = 0;
        startNode.h = Vector3.SqrMagnitude(startNode.pos - endNode.pos);
        startNode.f = startNode.h;

        endNode.g = 0;
        endNode.h = startNode.h;
        endNode.f = endNode.h;

        open_Front.Add(startNode);
        open_Back.Add(endNode);

        Node thisNode_Front;
        Node thisNode_Back;
        while (open_Front.Count > 0 && open_Back.Count > 0)
        {
            count++;
            if (count >= 1000)
            {
                return false;
            }
            //front
            int i = LowestF(open_Front);
            thisNode_Front = open_Front[i];
            open_Front.RemoveAt(i);
            close_Front.Add(thisNode_Front);


            //back
            i = LowestF(open_Back);
            thisNode_Back = open_Back[i];


            Node neighborNode;
            for (int j = dir.Length - 1; j >= 0; j--)
            {
                Vector3Int neighborPos = dir[j] + thisNode_Front.pos;
                if(neighborPos == endNode.pos)
                {
                    reconstructPath(path, startNode, endNode, close_Front);
                    return true;
                }
                /////先检查是否为空气且没到终点
                if (!World.CheckPosCanWalk(neighborPos) && neighborPos != endNode.pos)
                    continue;
                //////检查是否太高
                if (World.CheckOverHeight(neighborPos))
                    continue;

                neighborNode = FindNode(neighborPos);
                if (neighborNode == null)
                {
                    AddNode(neighborPos);
                    neighborNode = FindNode(neighborPos);
                }
                AddEdge(thisNode_Front, neighborNode);
                if (neighborNode.g == 0)
                    neighborNode.g = thisNode_Front.g + 1;
                /////是否在close列表里
                if (close_Front.IndexOf(neighborNode) > -1)
                {
                    continue;
                }


                if (open_Back.Contains(neighborNode))
                {
                    ///back
                    Node p = neighborNode;
                    while (p != null && p != endNode)
                    {
                        path.Add(p);
                        p = p.frontNode;
                    }
                    //front
                    p = close_Front[close_Front.Count - 1];
                    while (p != null && p != startNode)
                    {
                        path.Insert(0, p);
                        p = p.frontNode;
                    }
                    return true;
                }
                /////其他情况
                current_g = thisNode_Front.g + 1;
                if (open_Front.IndexOf(neighborNode) == -1)
                {
                    open_Front.Add(neighborNode);
                    current_isButter = true;
                }
                else if (current_g <= neighborNode.g)
                {
                    current_isButter = true;
                }
                else
                {
                    current_isButter = false;
                }
                if (current_isButter)
                {
                    neighborNode.frontNode = thisNode_Front;
                    neighborNode.g = current_g;
                    neighborNode.h = Vector3.SqrMagnitude(neighborNode.pos - thisNode_Back.pos);
                    neighborNode.f = neighborNode.g + neighborNode.h;
                    current_isButter = false;
                }
                
                //////如果是下面且不在close列表里，只添加下面节点
                if (j == dir.Length - 1)
                {
                    break;
                }
            }


            open_Back.RemoveAt(i);
            close_Back.Add(thisNode_Back);

            for (int j = dir.Length - 1; j >= 0;j--) 
            {
                Vector3Int neighborPos = dir[j] + thisNode_Back.pos;

                if (neighborPos == startNode.pos)
                {
                    reconstructPath(path, endNode, startNode, close_Back);
                    return true;
                }

                if (!World.CheckPosCanWalk(neighborPos) && neighborPos != startNode.pos)
                    continue;
                //////检查是否太高
                if (World.CheckOverHeight(neighborPos))
                    continue;

                neighborNode = FindNode(neighborPos);
                if (neighborNode == null)
                {
                    AddNode(neighborPos);
                    neighborNode = FindNode(neighborPos);
                }
                AddEdge(thisNode_Back, neighborNode);
                if (neighborNode.g == 0)
                    neighborNode.g = thisNode_Back.g + 1;
                /////是否在close列表里
                if (close_Back.IndexOf(neighborNode) > -1)
                {
                    continue;
                }


                if (open_Front.Contains(neighborNode))
                {
                    ///front
                    Node p = neighborNode;
                    while (p != null && p != startNode)
                    {
                        path.Insert(0, p);
                        p = p.frontNode;
                    }
                    //back
                    p = close_Back[close_Back.Count - 1];
                    while (p != null && p != endNode)
                    {
                        path.Add(p);
                        p = p.frontNode;
                    }
                    return true;
                }

                /////其他情况
                current_g = thisNode_Back.g + 1;
                if (open_Back.IndexOf(neighborNode) == -1)
                {
                    open_Back.Add(neighborNode);
                    current_isButter = true;
                }
                else if (current_g <= neighborNode.g)
                {
                    current_isButter = true;
                }
                else
                {
                    current_isButter = false;
                }
                if (current_isButter)
                {
                    neighborNode.frontNode = thisNode_Back;
                    neighborNode.g = current_g;
                    neighborNode.h = Vector3.SqrMagnitude(neighborNode.pos - thisNode_Front.pos);
                    neighborNode.f = neighborNode.g + neighborNode.h;
                    current_isButter = false;
                }
                //////如果是下面且不在close列表里，只添加下面节点
                if (j == dir.Length - 1)
                {
                    break;
                }
            }
        }
        return false;
    }
    private void reconstructPath(List<Node> path, Node startNode, Node endNode,List<Node> closeList)
    {
        path.Clear();
        Node p = closeList[closeList.Count - 1];
        while (p != null && p != closeList[0])
        {
            path.Insert(0, p);
            p = p.frontNode;
        }
    }
    private void reconstructPath(List<Node> path, Node startNode, Node endNode)
    {
        path.Clear();
        Node p = endNode.frontNode;
        while (p != null && p != startNode)
        {
            path.Insert(0, p);
            p = p.frontNode;
        }
        path.Insert(0, startNode);
    }

    public int LowestF(List<Node> open)
    {
        int cur = 0;
        float lowestf = open[0].f;
        for (int i = 1; i < open.Count; i++)
        {
            if (open[i].f < lowestf)
            {
                cur = i;
                lowestf = open[i].f;
            }
        }
        return cur;
    }
    public void Draw()
    {
        Gizmos.color = new Color(0, 0, 1, 0.25f);
        foreach (Node node in nodes)
        {
            Vector3 center = node.pos;
            Gizmos.DrawCube(center, Vector3.one);
        }
    }
}