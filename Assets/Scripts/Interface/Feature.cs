using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Feature : IResource
{
    int x, y, z;
    string structName;
    ChunkData ChunkData;
    bool has_Distribute;

    public event Action OnWorkFinish;

    public Feature(Vector3Int vector3, string blockName, ChunkData chunkData)
    {
        this.x = vector3.x;
        this.y = vector3.y;
        this.z = vector3.z;
        this.structName = blockName;
        this.ChunkData = chunkData;
    }
    public static string GetSturctName(Feature feature)
    {
        return feature.structName;
    }
    public static Vector3Int Get_LcoalPoint(Feature feature)
    {
        return new Vector3Int(feature.x, feature.y, feature.z);
    }
    public static Vector3Int Get_WorldPoint(Feature feature, ChunkData c)
    {
        return c.chunkPos + Get_LcoalPoint(feature);
    }
    public static Vector3Int Get_WorldPoint(Feature feature)
    {
        return feature.ChunkData.chunkPos + Get_LcoalPoint(feature);
    }
    public static ChunkData Get_Chunkdata(Feature feature)
    {
        return feature.ChunkData;
    }


    public bool GetPos(out Vector3Int vector3)
    {
        vector3 = Get_WorldPoint(this);
        return true;
    }
    public IEnumerator Interact()
    {
        Structure structure;
        Vector3Int localPos = Get_LcoalPoint(this);
        StructLibrary.dictionary.TryGetValue(GetSturctName(this), out structure);
        Vector3Int offset = structure.GetOffset();
        ChunkData chunk = Get_Chunkdata(this);
        foreach (Structure_Node node in structure.funcNodes)
        {
            int x = localPos.x + node.x - offset.x;
            int y = localPos.y + node.y - offset.y;
            int z = localPos.z + node.z - offset.z;
            Vector3Int chunkOffset = Vector3Int.zero;
            if (ChunkHelper.GetChunkOffset(ref x, ref y, ref z, ref chunkOffset))
            {
                if (!World.ChunkDatas.TryGetValue(chunk.chunkPos + chunkOffset, out chunk))
                {
                    continue;
                }
            }
            if (chunk.voxelMap[x, y, z].blockName != BlockName.Air && chunk.voxelMap[x, y, z].blockName == node.voxelMap.blockName)
            {
                chunk.voxelMap[x, y, z].blockName = BlockName.Air;
                chunk.voxelMap[x, y, z].blockState = null;
                chunk.IsModify();
                yield return new WaitForSeconds(0.5f);
            }
            chunk = Get_Chunkdata(this);
        }
        chunk.structPos.Remove(this);
        ResourcePool.Instance.RemoveResource(this);
        OnWorkFinish();
    }

    public bool GetInterface(out IResource resource)
    {
        resource = null;
        if (!has_Distribute)
        {
            has_Distribute = true;
            resource = this;
            return true;
        }
        return false;
    }
}
