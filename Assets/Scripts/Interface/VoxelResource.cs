using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceVoxel : IResource
{
    int x, y, z;
    VoxelMap voxelMap;
    bool has_Distribute;
    ChunkData chunkData;

    public event Action OnWorkFinish;

    public ResourceVoxel(int x, int y, int z, VoxelMap voxelMap, ChunkData chunkData)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.voxelMap = voxelMap;
        this.has_Distribute = false;
        this.chunkData = chunkData;
    }
    public void ResetPos(int x_low, int y_low, int z_low)
    {
        x = x - x_low;
        y = y - y_low;
        z = z - z_low;
    }
    public Vector3Int GetLocalPos()
    {
        return new Vector3Int(x, y, z);
    }
    public BlockName GetBlockName()
    {
        return voxelMap.blockName;
    }
    public bool GetPos(out Vector3Int vector3)
    {
        vector3 = Vector3Int.zero;
        if (!has_Distribute)
        {
            has_Distribute = true;
            vector3 = GetLocalPos();
            return true;
        }
        return false;
    }
    public IEnumerator Interact()
    {
        yield return new WaitForSeconds(1f);
        if (voxelMap.blockName == BlockName.Wheat_1 || voxelMap.blockName == BlockName.Wheat_2)
        {
            if (voxelMap.GetExtend<MatureAttribute_Extend>().Grow())
            {
                voxelMap.blockName += 1;
                chunkData.IsModify();
            }
        }
        else if (voxelMap.blockName == BlockName.Wheat_3)
        {
            voxelMap.blockName = BlockName.Wheat_1;
            chunkData.IsModify();
        }
        has_Distribute = false;

        if (OnWorkFinish != null)
            OnWorkFinish();
        yield break;
    }

    public bool GetInterface(out IResource resource)
    {
        resource = this;
        return true;
    }
}
