using Assets.Scripts.Mesh;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ResourceVisualize
{
    public List<ResourcePoint> points;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;
    public GameObject gameObject;
    public ChunkData chunkData;
    public void InsertPoint(Vector3Int point,BlockName num)
    {
        points.Add(new ResourcePoint(point,num));
    }
    // Start is called before the first frame update
    public ResourceVisualize(Chunk chunk)
    {
        points = new List<ResourcePoint>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();

        gameObject = new GameObject();
        gameObject.transform.parent = chunk.chunkObject.transform;
        gameObject.transform.position = Vector3.zero;
        gameObject.name = chunk.chunkObject.name + "Resource";
        chunkData = chunk.chunkData;
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = chunk.world.ResourceMaterial;
    }
    public void Visualize()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        if (points.Count == 0) return;
        Mesh mesh = new Mesh();
        foreach (ResourcePoint point in points)
        {
            LoopNeiborVoxelMap(point.position);
            CreateMesh(point.position);
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }
    public void LoopNeiborVoxelMap(Vector3Int Pos)
    {
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                if(i == 0 && j == 0) continue;
                Vector3Int vector3 = new Vector3Int(i, 0, j) + Pos;
                vector3 = FindNeiborIsAir(vector3);
                if(vector3 == Vector3.down) continue;
                CreateMesh(vector3);
            }
        }
    }
    public Vector3Int FindNeiborIsAir(Vector3Int Pos)
    {
        Vector3Int chunkPos = Vector3Int.zero;
        Vector3Int localPos = Vector3Int.zero;
        ChunkHelper.GetChunkPosAndLocalPos(Vector3Int.FloorToInt(Pos), ref chunkPos, ref localPos);
        int x = localPos.x;
        int y = localPos.y;
        int z = localPos.z;


        Vector3Int chunkoffset = Vector3Int.zero;
        ChunkData newchunkData;
        if(!ChunkHelper.GetChunkOffset(ref x, ref y, ref z, ref chunkoffset))
        {
            newchunkData = chunkData;
            Vector3Int vector3 = GetButtom(newchunkData, x, y, z);
            if (vector3 == Vector3Int.down) return vector3;
            return vector3 + newchunkData.chunkPos;
        }
        if (World.ChunkDatas.TryGetValue(chunkData.chunkPos + chunkoffset, out newchunkData))
        {
            Vector3Int vector3 = GetButtom(newchunkData, x, y, z);
            if (vector3 == Vector3Int.down) return vector3;
            return vector3 + newchunkData.chunkPos;
        }
        Debug.Log("no neibor");
        return Vector3Int.down;
    }
    public Vector3Int GetButtom(ChunkData chunkData,int x,int y,int z)
    {
        if (y < 0 && y >= WorldHelper.chunkHeight)
            return Vector3Int.down;
        if (chunkData.voxelMap[x,y,z].blockName == BlockName.Air)
        {
            return GetButtom(chunkData, x, y - 1, z);
        }else
        {
            if(chunkData.voxelMap[x, y + 1, z].blockName == BlockName.Air)
            {
                return new Vector3Int(x,y,z);
            }
            else
            {
                return GetButtom(chunkData,x,y + 1,z);
            }
        }
    }
    public void CreateMesh(Vector3 offset)
    {
        AddVertices(offset);
        AddTriangles();
        AddUvs();
    }
    public void AddUvs()
    {
        uvs.Add(ResourcePointMesh.UVs[0]);
        uvs.Add(ResourcePointMesh.UVs[1]);
        uvs.Add(ResourcePointMesh.UVs[2]);
        uvs.Add(ResourcePointMesh.UVs[3]);
    }
    public void AddVertices(Vector3 offsetVec3)
    {
        vertices.Add(ResourcePointMesh.Verts[0] + offsetVec3);
        vertices.Add(ResourcePointMesh.Verts[1] + offsetVec3);
        vertices.Add(ResourcePointMesh.Verts[2] + offsetVec3);
        vertices.Add(ResourcePointMesh.Verts[3] + offsetVec3);
    }
    public void AddTriangles()
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 1);
    }
}
public class ResourcePoint
{
    public Vector3Int position;
    public BlockName blockNum;
    public ResourcePoint (Vector3Int vector3, BlockName num)
    {
        position = vector3;
        blockNum = num;
    }
}
