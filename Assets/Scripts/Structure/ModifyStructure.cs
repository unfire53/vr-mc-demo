using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer),typeof(MeshCollider))]
public class ModifyStructure : MonoBehaviour
{
    public World world;
    public Structure structure;
    public List<IResource> Poi;
    public List<Structure_Node> nodes;
    public List<ChunkData> chunkDatas;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    public ChunkMesh chunkMesh;
    public StructureType structureType;
    int x_low, x_high, y_low, y_high, z_low, z_high;
    public GameObject saveButton;
    public TextMeshProUGUI text;
    public int conditionCount;
    public int maxConditionCount;
    public int point;
    private void Start()
    {
        chunkMesh = new ChunkMesh(false);
        nodes = new List<Structure_Node>();
        Poi = new List<IResource> ();
        chunkDatas = new List<ChunkData> ();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }
    public void SetStructureType(string s)
    {
        switch (s)
        {
            case "House": structureType = StructureType.house;maxConditionCount = 1 ; UpdateUI(StructureType.house); break;
            case "Farmland": structureType= StructureType.farmland; maxConditionCount = 4; UpdateUI(StructureType.farmland); break;
            case "Tree": structureType = StructureType.Lumberyards; maxConditionCount = 1; UpdateUI(StructureType.Lumberyards); break;
            default: structureType = StructureType.house; Debug.Log("wrong in setstructtype"); break;
        }
    }
    public void UpdateUI(StructureType type)
    {
        string s;
        switch(type)
        {
            case StructureType.house: s = "bed";break;
            case StructureType.farmland: s = "wheat";break;
            case StructureType.Lumberyards: s = "tool";break;
            default: s = "null";break;
        }
        text.text = s + conditionCount + "/" + maxConditionCount;
    }
    public void UpdateUI()
    {
        UpdateUI(structureType);
    }
    public void ConditionAdd()
    {
        conditionCount++;
        UpdateUI();
        if(conditionCount >= maxConditionCount)
            saveButton.SetActive(true);
    }
    public void AddNode(Structure_Node node)
    {
        int x = node.x;
        int y = node.y;
        int z = node.z;
        if (nodes.Count == 0)
        {
            x_low = x_high = x;
            y_low = y_high = y;
            z_low = z_high = z;
        }
        else
        {
            if (x < x_low)
            {
                x_low = x;
            }
            else if (x > x_high)
            {
                x_high = x;
            }
            if (y < y_low)
            {
                y_low = y;
            }
            else if (y > y_high)
            {
                y_high = y;
            }
            if (z < z_low)
            {
                z_low = z;
            }
            else if (z > z_high)
            {
                z_high = z;
            }
        }
        Vector3Int v = ChunkHelper.GetChunkPos(new Vector3(node.x, node.y, node.z));
        World.ChunkDatas.TryGetValue(v,out ChunkData chunkData);
        switch(node.voxelMap.blockName)
        {
            case BlockName.Wheat_1: case BlockName.Wheat_2:case BlockName.Wheat_3: case BlockName.Bed:
                ResourceVoxel rv = new ResourceVoxel(x,y,z,node.voxelMap,chunkData); Poi.Add(rv); break;
        }
        nodes.Add(node);
    }
    public void CreateStructure()
    {
        if(nodes.Count > 0)
        {
            foreach (Structure_Node n in nodes)
            {
                n.x -= x_low;
                n.y -= y_low;
                n.z -= z_low;
            }
            foreach(ResourceVoxel rv in Poi)
            {
                rv.ResetPos(x_low, y_low, z_low);
            }
        }
        //if(funcNodes.Count > 0)
        //{
        //    foreach (Structure_Node n in funcNodes)
        //    {
        //        n.x -= x_low;
        //        n.y -= y_low;
        //        n.z -= z_low;
        //    }
        //}
        //structure = new Structure(nodes.ToArray(), funcNodes.ToArray(), x_high - x_low + 1, y_high - y_low + 1, z_high - z_low + 1, new Vector3Int((x_high - x_low + 1)/2, 0, (z_high - z_low + 1)/2), structureType);
        //string s = "modifyStruct" + (StructLibrary.modifyStructnum++).ToString();
        //StructLibrary.dictionary.Add(s, structure);
        Vector3Int point = new Vector3Int(x_low, y_low, z_low);
        float x = (float)x_low / (float)WorldHelper.chunkSize;
        float z = (float)z_low / (float)WorldHelper.chunkSize;
        Vector3Int chunkpos = Vector3Int.FloorToInt(new Vector3(x,0,z))*16;
        ChunkData chunkData;
        World.ChunkDatas.TryGetValue(chunkpos, out chunkData);
        
        StructLibrary.BuildModify(nodes, point,out chunkDatas);
        IResource modify;
        switch (structureType)
        {
            case StructureType.Lumberyards:
                modify = new Resourcesite(x_low, y_low, z_low, x_high, y_high, z_high, 0, chunkData, structureType, Poi, chunkDatas);
                Debug.Log(modify);
                break;
            default:
                modify = new WorkPlace(x_low, y_low, z_low, x_high, y_high, z_high, 0, chunkData, structureType, Poi, chunkDatas);
                break;
        }
        chunkData.structPos.Add(modify);
        NodeClear();
     }
    public void NodeClear()
    {
        //funcNodes.Clear();
        chunkDatas.Clear();
        Poi.Clear();
        nodes.Clear();
        conditionCount = 0;
    }
    public void clearMesh()
    {
        NodeClear();
        chunkMesh.Clear();
        meshFilter.mesh = null;
        meshCollider.sharedMesh = null;
    }
    public void UpdateMesh()
    {
        chunkMesh.Clear();
        foreach (var node in nodes)
        {
            VoxelMap voxelmap = node.voxelMap;
            BlockMesh block;
            block = MeshDataLibrary.GetMesh(voxelmap, world);
            if (block == null) { Debug.LogWarning("waring in chunkhelper.checkmesh"); return; }
            bool has_Collider = true;
            for (int face = 0; face < block.faceCheck_N.Length; face++)
            {
                CreateMesh(node.GetNodePos(), face, voxelmap.blockName, chunkMesh, world, block, has_Collider);
            }
        }
        //foreach (var node in funcNodes)
        //{
        //    VoxelMap voxelmap = node.voxelMap;
        //    BlockMesh block;
        //    block = MeshDataLibrary.GetMesh(voxelmap, world);
        //    if (block == null) { Debug.LogWarning("waring in chunkhelper.checkmesh"); return; }
        //    bool has_Collider = true;
        //    for (int face = 0; face < block.faceCheck_N.Length; face++)
        //    {
        //        CreateMesh(node.GetNodePos(), face, voxelmap.blockName, chunkMesh, world, block, has_Collider);
        //    }
        //}
        meshFilter.mesh.Clear();
        Mesh mesh = new Mesh();
        mesh.subMeshCount = 2;
        mesh.vertices = chunkMesh.vertices_Collider.Concat(chunkMesh.vertices).Concat(chunkMesh.chunkMesh_Water.vertices).ToArray();
        mesh.SetTriangles(chunkMesh.triangles_Collider
            .Concat(chunkMesh.triangles.Select((val) => val + chunkMesh.vertices_Collider.Count)).ToArray(), 0);
        mesh.SetTriangles(chunkMesh.chunkMesh_Water.triangles.Select(val => val + chunkMesh.vertices_Collider.Count + chunkMesh.vertices.Count).ToArray(), 1);
        mesh.uv = chunkMesh.uvs_Collider.Concat(chunkMesh.uvs).Concat(chunkMesh.chunkMesh_Water.uvs).ToArray();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = null;
        Mesh colliderMesh = new Mesh();
        colliderMesh.vertices = chunkMesh.vertices_Collider.ToArray();
        colliderMesh.triangles = chunkMesh.triangles_Collider.ToArray();
        colliderMesh.RecalculateNormals();
        meshCollider.sharedMesh = colliderMesh;
        meshFilter.mesh = mesh;
    }
    public static void CreateMesh(Vector3 offsetVec3, int face, BlockName blockName, ChunkMesh mesh, World world, BlockMesh newMesh, bool has_Collider)
    {
        int blocknum = (int)blockName;
        mesh.AddVertices(offsetVec3, face, newMesh, has_Collider);
        mesh.AddTriangles(has_Collider);
        mesh.AddUvs(world.blockTypes[blocknum].GetTextureID(GetMeshFace(newMesh.faceOrigin_N[face])), has_Collider);
    }
    public static void CreatWaterMesh(Vector3 offsetVec3, ChunkMesh mesh)
    {
        mesh.AddWaterVertices(offsetVec3);
        mesh.AddWaterTriangles();
        mesh.AddWaterUvs(22);
    }
    public static int GetMeshFace(Vector3 vector3)
    {
        if (vector3 == Vector3.forward) return 0;
        else if (vector3 == Vector3.back) return 1;
        else if (vector3 == Vector3.up) return 2;
        else if (vector3 == Vector3.down) return 3;
        else if (vector3 == Vector3.left) return 4;
        else if (vector3 == Vector3.right) return 5;
        else return 0;
    }
}
