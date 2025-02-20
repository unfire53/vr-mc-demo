using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// 生成地物专用
/// </summary>
public static class FeaturesGenerator
{
    public static void LoopVoxelMap(ChunkData c)
    {
        for (int z = WorldHelper.chunkSize - 1; z >= 0; z--)
        {
            for (int x = WorldHelper.chunkSize - 1; x >= 0; x--)
            {
                int terrainHeight = 0;
                if (GetHeight(c, x, ref terrainHeight, z))
                {
                    FeaturesGenerate(c, x, terrainHeight, z);
                }
            }
        }
    }
    public static void FeaturesGenerate(ChunkData c, int x, int y, int z)
    {
        if (c.voxelMap[x, y, z].blockName == BlockName.Water) return;
        Vector3Int Pos = c.chunkPos + new Vector3Int(x, y, z);
        bool has_LargePlant = PerlinNoise.Get2DPerlin(new Vector2(Pos.x, Pos.z), World.offsets[2], 0.1f) > 0.5f;
        float f = PerlinNoise.Get2DPerlin(new Vector2(Pos.x, Pos.z), World.offsets[2], 10f);
        if (has_LargePlant && f > 0.8f)
        {
            if (f > 0.93f)
            {
                c.voxelMap[x, y + 1, z].blockName = BlockName.Mushroom;
                c.structPos.Add(new Feature(new Vector3Int(x,y + 1,z),"Mushroom",c));
            }
            else
            {
                StructLibrary.BuildStructure("tree_1", new Vector3Int(x, y, z) + Vector3Int.up, c);
                c.structPos.Add(new Feature(new Vector3Int(x, y + 1, z), "tree_1", c));
            }
            
            return;
        }
        //if (f > 0.99f)
        //{
        //    StructLibrary.BuildStructure("Coal", new Vector3Int(x, y, z) + Vector3Int.up, c);
        //    c.structPos.Add(new Feature(new Vector3Int(x, y + 1, z), "Coal", c));
        //    return;
        //}
        //else if (f > 0.97f)
        //{
        //    StructLibrary.BuildStructure("Iron", new Vector3Int(x, y, z) + Vector3Int.up, c);
        //    c.structPos.Add(new Feature(new Vector3Int(x, y + 1, z), "Iron", c));
        //    return;
        //}

        float has_Smallplant = PerlinNoise.Get2DPerlin(new Vector2(Pos.x, Pos.z), 4000, 8f);
        if (has_Smallplant > 0.85f)
        {
            //c.structPos.Add(new Feature(new Vector3Int(x, y + 1, z), "Vegetable", c));
            c.voxelMap[x, y + 1, z].blockName = BlockName.Vegetable;
        }
        else if (has_Smallplant > 0.82f)
            c.voxelMap[x, y + 1, z].blockName = BlockName.flower;
        else if (has_Smallplant > 0.75f)
            c.voxelMap[x, y + 1, z].blockName = BlockName.Plant;
        return;
    }
    public static bool GetHeight(ChunkData c, int x, ref int y, int z)
    {
        for (int i = WorldHelper.chunkHeight - 1; i >= 0; i--)
        {
            switch (c.voxelMap[x, i, z].blockName)
            {
                case BlockName.Air:
                case BlockName.Leaves:
                case BlockName.Wood:
                    continue;
                case BlockName.Coal:
                case BlockName.Iron:
                    return false;
                default: y = i; return true;
            }
        }
        return false;
    }
}
