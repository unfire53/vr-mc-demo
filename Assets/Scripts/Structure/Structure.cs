using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// 地物，建筑等等的重要模型
/// </summary>
public enum StructureType
{
    tree, mineral, farmland, house, Lumberyards
}
public class Structure_Node
{
    public int x,y,z;
    public VoxelMap voxelMap;
    public Structure_Node(Vector3Int nodePos, VoxelMap voxelMap)
    {
        x = nodePos.x;
        y = nodePos.y;
        z = nodePos.z;
        this.voxelMap = voxelMap;
    }
    public Structure_Node(int x,int y,int z, VoxelMap voxelMap)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.voxelMap = voxelMap;
    }
    public Vector3Int GetNodePos()
    {
        return new Vector3Int(x,y,z);
    }
}
public class Structure
{
    public StructureType structureType;
    public int x, y, z;
    public Vector3Int offest;
    public Structure_Node[] nodes;
    public Structure_Node[] funcNodes;
    public Structure(Vector3Int from,Vector3Int to, Structure_Node[] nodes, Structure_Node[] funcNodes, StructureType structureType)
    {
        int x = Mathf.Abs(from.x - to.x);
        int y = Mathf.Abs (from.y - to.y);
        int z = Mathf.Abs (from.z - to.z);

        this.x = x; this.y = y; this.z = z;
        offest = new Vector3Int(x / 2, 0, z / 2);
        this.nodes = nodes;
        this.funcNodes = funcNodes;
        this.structureType = structureType;
    }
    public Structure(Structure_Node[] Nodes, Structure_Node[] funcNodes, int x,int y,int z,Vector3Int offset,StructureType structureType)
    {
        this.nodes = Nodes;
        this.funcNodes = funcNodes;
        this.x = x; this.y = y; this.z = z;
        this.offest = offset;
        this .structureType = structureType;
    }
    public Vector3Int GetOffset()
    {
        return offest;
    }
    public StructureType GetStructureType()
    {
        return structureType;
    }
}

public class StructLibrary
{
    public static int modifyStructnum;
    public static Dictionary<string, Structure> dictionary = new Dictionary<string, Structure>()
    {
        {"tree_1", new Structure(null,Structure_Tree1.structure_Nodes,Structure_Tree1.x,Structure_Tree1.y,Structure_Tree1.z,Structure_Tree1.offset,StructureType.tree)},
        {"Coal"  , new Structure(null, Structure_Coal.structure_Nodes,Structure_Coal.x,Structure_Coal.y,Structure_Coal.z,Structure_Coal.offset, StructureType.mineral)},
        {"Iron"  , new Structure(null, Structure_Iron.structure_Nodes,Structure_Iron.x,Structure_Iron.y,Structure_Iron.z,Structure_Iron.offset, StructureType.mineral)}
    };
    public static void BuildStructure(string name, Vector3Int worldPos)
    {
        ChunkData chunkData;
        Vector3Int pos = worldPos;
        Vector3Int chunkPos = ChunkHelper.GetChunkPos(pos);
        if (World.ChunkDatas.TryGetValue(chunkPos, out chunkData))
        {
            pos = Vector3Int.FloorToInt(pos - chunkPos);
            BuildStructure(name, pos, chunkData);
        }
    }
    public static void BuildStructure(string name, Vector3Int localPos, ChunkData c)
    {
        dictionary.TryGetValue(name, out Structure structure);
        localPos -= structure.GetOffset();
        if ( structure.nodes != null&&structure.nodes.Length > 0)
        {
            foreach (Structure_Node node in structure.nodes)
            {
                AddVoxelInNeiber(c, localPos.x + node.x, localPos.y + node.y, localPos.z + node.z, node.voxelMap);
            }
        }
        if (structure.funcNodes != null &&structure.funcNodes.Length > 0)
        {
            foreach (Structure_Node node in structure.funcNodes)
            {
                AddVoxelInNeiber(c, localPos.x + node.x, localPos.y + node.y, localPos.z + node.z, node.voxelMap);
            }
        }
    }
    public static void AddVoxelInNeiber(ChunkData c, int x, int y, int z,VoxelMap v)
    {
        ChunkData c1 = c;
        Vector3Int chunkoffset = Vector3Int.zero;
        if(ChunkHelper.GetChunkOffset(ref x,ref y,ref z,ref chunkoffset))
        {
            if (!World.ChunkDatas.TryGetValue(c.chunkPos + chunkoffset, out c1)) return;
        }
        if (c1.voxelMap[x,y,z].blockName == BlockName.Air) 
        {
            c1.voxelMap[x, y, z].blockName = v.blockName;
            c1.voxelMap[x, y, z].blockState = v.blockState;
            return;
        }
    }
    public static void BuildModify(List<Structure_Node> nodes, Vector3Int worldPos ,out List<ChunkData> chunkDatas)
    {
        ChunkData c;
        Vector3Int pos = worldPos;
        Vector3Int chunkPos = ChunkHelper.GetChunkPos(pos);
        World.ChunkDatas.TryGetValue(chunkPos, out c);
        chunkDatas = new List<ChunkData>
        { c };
        Vector3Int localPos = worldPos - chunkPos;
        if (nodes != null && nodes.Count > 0)
        {
            foreach (Structure_Node node in nodes)
            {
                AddVoxelInNeiber(c, localPos.x + node.x, localPos.y + node.y, localPos.z + node.z, node.voxelMap, chunkDatas);
            }
        }
        foreach (ChunkData chunkdata in chunkDatas)
        {
            chunkdata.IsModify();
        }
    }
    public static void AddVoxelInNeiber(ChunkData c, int x, int y, int z, VoxelMap v,List<ChunkData> chunkDatas)
    {
        ChunkData c1 = c;
        Vector3Int chunkoffset = Vector3Int.zero;
        if (ChunkHelper.GetChunkOffset(ref x, ref y, ref z, ref chunkoffset))
        {
            if (!World.ChunkDatas.TryGetValue(c.chunkPos + chunkoffset, out c1)) return;
            if(!chunkDatas.Contains(c1))
                chunkDatas.Add(c1);
        }
        //c1.voxelMap[x, y, z].blockName = v.blockName;
        //c1.voxelMap[x, y, z].blockState = v.blockState;
        c1.voxelMap[x, y, z] = v;
        //if(v.blockName == BlockName.Bed)
        //{
        //    PopulationManager.instance.emptyBed.Enqueue(c1.chunkPos + new Vector3Int(x, y, z));
        //}
    }
}
