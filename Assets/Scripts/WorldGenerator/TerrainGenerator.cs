using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// 生成地形专用，生成地形时，先populate出来，要在边缘留一圈chunk不画，以免后面经常重复改voxelMap。
/// </summary>
public static class TerrainGenerator
{
    public static void TerrainGenerate(ChunkData c, AnimationCurve Curve_Continentalness, AnimationCurve Curve_PeakAndValleys, AnimationCurve Curve_Erosion)
    {
        if (!c.hasChunkData)//检查是否有过存档
        {
            for (int z = WorldHelper.chunkSize - 1; z >= 0; z--)
            {
                for (int x = WorldHelper.chunkSize - 1; x >= 0; x--)
                {
                    Vector3 PosTerrain = new Vector3(x, 0, z) + c.chunkPos;
                    //float Continentalness = PerlinNoise.Continentalness(PerlinNoise.OctavePerlin(new Vector2(PosTerrain.x, PosTerrain.z), World.offsets[0], 0.01f)) * WorldHelper.Continentalnesslevel;
                    float PeakAndValleys = PerlinNoise.Spline_Evaluate(PerlinNoise.OctavePerlin(new Vector2(PosTerrain.x, PosTerrain.z), World.offsets[1], 0.1f), Curve_PeakAndValleys) * WorldHelper.PeakAndValleyslevel;
                    float Continentalness = PerlinNoise.Spline_Evaluate(PerlinNoise.OctavePerlin(new Vector2(PosTerrain.x, PosTerrain.z), World.offsets[1], 0.1f), Curve_Continentalness) * WorldHelper.Continentalnesslevel;
                    float Erosion = PerlinNoise.Spline_Evaluate(PerlinNoise.OctavePerlin(new Vector2(PosTerrain.x, PosTerrain.z), World.offsets[1], 0.1f), Curve_Erosion) * WorldHelper.Erosionlevel;
                    int terrainHeight = Mathf.FloorToInt(Continentalness + PeakAndValleys + Erosion);
                    for (int y = WorldHelper.chunkHeight - 1; y >= 0; y--)
                    {
                        BiomeGenerate(c, x, y, z, terrainHeight);
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < WorldHelper.chunkSize; x++)
            {
                for (int y = 0; y < WorldHelper.chunkHeight; y++)
                {
                    for (int z = 0; z < WorldHelper.chunkSize; z++)
                    {
                        c.voxelMap[x, y, z] = c.chunkDataSave.voxelMap[x, y, z];
                    }
                }
            }
        }
    }
    //生成地形时，先populate出来，要在边缘留一圈chunk不画，以免后面经常重复改voxelMap。
    public static void BiomeGenerate(ChunkData c, int x, int y, int z, int terrainHeight)
    {
        Vector3 Pos = c.chunkPos + new Vector3(x, y, z);
        float biomeNum = PerlinNoise.Get2DPerlin(new Vector2(Pos.x, Pos.z), World.offsets[2], 0.01f);
        if (biomeNum > 0.7)
        {
            if (y > terrainHeight)
            {
                if (y <= WorldHelper.seaLevel)
                    c.voxelMap[x, y, z].blockName = BlockName.Water;
                else
                    c.voxelMap[x, y, z].blockName = BlockName.Air;
            }
            else if (y == terrainHeight)
            {
                c.voxelMap[x, y, z].blockName = BlockName.Snow;
            }
            else if (y >= terrainHeight - 4 && y < terrainHeight)
            {
                c.voxelMap[x, y, z].blockName = BlockName.Snow;
            }
            else if (y < terrainHeight - 4)
                c.voxelMap[x, y, z].blockName = BlockName.Stone;
        }
        else if (biomeNum > 0.3)
        {
            if (y > terrainHeight)
            {
                if (y <= WorldHelper.seaLevel)
                    c.voxelMap[x, y, z].blockName = BlockName.Water;
                else
                    c.voxelMap[x, y, z].blockName = BlockName.Air;
            }
            else if (y == terrainHeight)
            {
                c.voxelMap[x, y, z].blockName = BlockName.Grass;
            }
            else if (y >= terrainHeight - 2 && y < terrainHeight)
            {
                c.voxelMap[x, y, z].blockName = BlockName.Dirt;
            }
            else if (y < terrainHeight - 2)
                c.voxelMap[x, y, z].blockName = BlockName.Stone;

        }
        else
        {
            if (y > terrainHeight)
            {
                if (y <= WorldHelper.seaLevel)
                    c.voxelMap[x, y, z].blockName = BlockName.Water;
                else
                    c.voxelMap[x, y, z].blockName = BlockName.Air;
            }
            else if (y == terrainHeight)
            {
                c.voxelMap[x, y, z].blockName = BlockName.Sand;
            }
            else if (y >= terrainHeight - 4 && y < terrainHeight)
            {
                c.voxelMap[x, y, z].blockName = BlockName.Sand;
            }
            else if (y < terrainHeight - 4)
                c.voxelMap[x, y, z].blockName = BlockName.Stone;
        }

    }
}
