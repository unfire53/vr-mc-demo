using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WorkPlace : IResource
{
    int x, y, z, point, cur, population, population_max;
    int x_size, y_size, z_size;
    ChunkData chunkdata;
    StructureType structureType;
    List<ResourceVoxel> Poi = new List<ResourceVoxel>();

    public event Action OnWorkFinish;

    public WorkPlace(int x_low, int y_low, int z_low, int x_high, int y_high, int z_high, int point, ChunkData chunkdata, StructureType structureType, List<IResource> poi, List<ChunkData> chunkDatas)
    {
        population_max = poi.Count / 4 + 1;
        this.x_size = x_high - x_low + 1;
        this.y_size = y_high - y_low + 1;
        this.z_size = z_high - z_low + 1;
        this.point = point;
        this.chunkdata = chunkdata;
        this.x = x_low - chunkdata.chunkPos.x;
        this.y = y_low - chunkdata.chunkPos.y;
        this.z = z_low - chunkdata.chunkPos.z;
        this.structureType = structureType;
        foreach (ResourceVoxel resource in poi)
        {
            this.Poi.Add(resource);
            if (resource.GetBlockName() == BlockName.Bed)
            {
                resource.GetPos(out Vector3Int vector3);
                PopulationManager.instance.emptyBed.Enqueue(vector3 + GetGobalPos());
            }
        }
        foreach (ChunkData chunkData in chunkDatas)
        {
            chunkData.OnUpdateModify += CheckSelf;
        }
    }

    private void CheckSelf(int arg1, int arg2, int arg3, BlockName name1, BlockName name2, ChunkData data)
    {
        Debug.Log("CheckSelf");
    }

    public Vector3Int GetLocalPos()
    {
        return new Vector3Int(x, y, z);
    }
    public Vector3Int GetGobalPos()
    {
        return chunkdata.chunkPos + GetLocalPos();
    }
    public StructureType GetStructureType()
    {
        return structureType;
    }


    public bool GetPos(out Vector3Int vector3)
    {
        vector3 = default(Vector3Int);
        for (int i = 0; i < Poi.Count; i++)
        {
            int w = cur + i;
            w %= Poi.Count;
            if (Poi[w].GetPos(out vector3))
            {
                vector3 += GetGobalPos();
                cur = w + 1;
                return true;
            }
        }
        return false;
        //foreach(ResourceVoxel resourceVoxel in Poi)
        //{
        //    if(resourceVoxel.GetPos(out vector3))
        //    {
        //        vector3 += GetGobalPos();
        //        return true;
        //    }
        //}
        //vector3 = default(Vector3Int);
        //return false;
    }
    public IEnumerator Interact()
    {
        throw new NotImplementedException();
    }

    public bool GetInterface(out IResource resource)
    {
        resource = Poi[cur - 1];
        return true;
    }
}
public class Resourcesite: IResource
{
    int x, y, z, point, cur, population, population_max;
    int x_size, y_size, z_size;
    ChunkData chunkdata;
    StructureType structureType;
    IResource iresource;
    ChunkData[] datas;

    public event Action OnWorkFinish;

    public Resourcesite(int x_low, int y_low, int z_low, int x_high, int y_high, int z_high, int point, ChunkData chunkdata, StructureType structureType, List<IResource> poi, List<ChunkData> chunkDatas)
    {
        population_max = poi.Count / 4 + 1;
        this.x_size = x_high - x_low + 1;
        this.y_size = y_high - y_low + 1;
        this.z_size = z_high - z_low + 1;
        this.point = point;
        this.chunkdata = chunkdata;
        this.x = x_low - chunkdata.chunkPos.x;
        this.y = y_low - chunkdata.chunkPos.y;
        this.z = z_low - chunkdata.chunkPos.z;
        this.structureType = structureType;
        List<ChunkData> chunks = new List<ChunkData>();
        for (int i = 0; i < vector3Ints.Length; i++)
        {
            Vector3Int chunkpos = vector3Ints[i] + chunkdata.chunkPos;
            ChunkData data = chunkdata;
            if (!World.ChunkDatas.TryGetValue(chunkpos, out data)) continue;
            chunks.Add(data);
        }
        datas = chunks.ToArray();

        foreach (ChunkData chunkData in chunkDatas)
        {
            chunkData.OnUpdateModify += CheckSelf;
        }
    }

    private void CheckSelf(int arg1, int arg2, int arg3, BlockName name1, BlockName name2, ChunkData data)
    {
        Debug.Log("CheckSelf");
    }

    public Vector3Int GetLocalPos()
    {
        return new Vector3Int(x, y, z);
    }
    public Vector3Int GetGobalPos()
    {
        return chunkdata.chunkPos + GetLocalPos();
    }
    public StructureType GetStructureType()
    {
        return structureType;
    }

    public Vector3Int[] vector3Ints = new Vector3Int[9]
        { 
            new Vector3Int(-16, 0, -16),
            new Vector3Int(0, 0, -16),
            new Vector3Int(16, 0, -16),
            new Vector3Int(-16, 0, 0),
            new Vector3Int(0, 0, 0),
            new Vector3Int(16, 0, 0),
            new Vector3Int(-16, 0, 16),
            new Vector3Int(0, 0, 16),
            new Vector3Int(16, 0, 16)
        };
    public bool GetPos(out Vector3Int vector3)
    {
        vector3 = default(Vector3Int);
        foreach (ChunkData data in datas) 
        {
            foreach(IResource resource in data.structPos)
            {
                if(resource.GetInterface(out iresource) && iresource is Feature)
                {
                    iresource.GetPos(out vector3);
                    return true;
                }
            }
        }
        return false;
    }
    public IEnumerator Interact()
    {
        throw new NotImplementedException();
    }

    public bool GetInterface(out IResource resource)
    {
        resource = iresource;
        return true;
    }
}
