using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public static class ChunkHelper
{
    /// <summary>
    /// /获取结构体方块所在的chunkdata
    /// </summary>
    /// <returns></returns>
    public static bool GetChunkOffset(ref int x,ref int y,ref int z, ref Vector3Int chunkoffset)
    {
        if (x >= 0 && x < 16 && z >= 0 && z < 16)
        {
            return false;
        }
        Vector3Int offset = Vector3Int.zero;
        if (x < 0)
        {
            x += 16;
            offset.x -= 16;
        }
        else if (x >= 16)
        {
            x -= 16;
            offset.x += 16;
        }
        if (z < 0)
        {
            z += 16;
            offset.z -= 16;
        }
        else if (z >= 16)
        {
            z -= 16;
            offset.z += 16;
        }
        chunkoffset = offset;
        return true;
    }
    public static void GetLocalPos(Vector3Int worldPos, ref Vector3Int localPos)
    {
        Vector3 chunk = new Vector3
            (Mathf.Floor(worldPos.x / (float)WorldHelper.chunkSize), 0,
            Mathf.Floor(worldPos.z / (float)WorldHelper.chunkSize));
        Vector3Int Local = worldPos - Vector3Int.FloorToInt(chunk * WorldHelper.chunkSize);
        int x = Mathf.Abs(Local.x);
        int y = Mathf.Abs(Local.y);
        int z = Mathf.Abs(Local.z);

        localPos = new Vector3Int(x, y, z);
    }
    public static void GetChunkPosAndLocalPos(Vector3Int worldPos,ref Vector3Int chunkPos,ref Vector3Int localPos)
    {
        Vector3 chunk = new Vector3
            (Mathf.Floor(worldPos.x / (float)WorldHelper.chunkSize), 0,
            Mathf.Floor(worldPos.z / (float)WorldHelper.chunkSize));
        chunkPos = Vector3Int.FloorToInt(chunk * WorldHelper.chunkSize);
        Vector3Int Local = worldPos - chunkPos;
        int x = Mathf.Abs(Local.x);
        int y = Mathf.Abs(Local.y);
        int z = Mathf.Abs(Local.z);

        localPos = new Vector3Int(x, y, z);
    }
    public static Vector3Int GetChunkPos(Vector3 worldPos)
    {
        Vector3 p = new Vector3(Mathf.Floor(worldPos.x / WorldHelper.chunkSize), 0, Mathf.Floor(worldPos.z / WorldHelper.chunkSize));
        p = new Vector3(p.x * WorldHelper.chunkSize, 0, p.z * WorldHelper.chunkSize);
        return Vector3Int.FloorToInt(p);
    }
    public static void CheckMesh(Vector3Int offsetVec3, ChunkData chunkData, ChunkMesh mesh, World world)
    {
        VoxelMap voxelmap = chunkData.voxelMap[(int)offsetVec3.x, (int)offsetVec3.y, (int)offsetVec3.z];

        if (voxelmap.blockName != BlockName.Air && voxelmap.blockName != BlockName.Water)
        {
            BlockMesh block;
            block = MeshDataLibrary.GetMesh(voxelmap, world);
            if(block == null) { Debug.LogWarning("waring in chunkhelper.checkmesh"); return; }
            bool has_Collider;
            switch(voxelmap.blockName)
            {
                case BlockName.Vegetable:
                case BlockName.Mushroom:
                case BlockName.Plant:
                case BlockName.flower:
                case BlockName.Wheat_1:
                case BlockName.Wheat_2:case BlockName.Wheat_3:
                    has_Collider = false; break;
                default: has_Collider = true; break;
            }
            if (world.blockTypes[(int)voxelmap.blockName].isTransparent)
            {
                for (int face = 0; face < block.faceCheck_N.Length; face++)
                {
                    CreateMesh(offsetVec3, face, voxelmap.blockName, mesh, world, block,has_Collider);
                }
            }
            else
            {
                for (int face = 0; face < block.faceCheck_N.Length; face++)
                {
                    if (CheckTransparent(offsetVec3 + block.faceCheck_N[face], offsetVec3, chunkData, world))
                    {
                        CreateMesh(offsetVec3, face, voxelmap.blockName, mesh, world, block, has_Collider);
                    }
                }
            }
        }
        else if (offsetVec3.y == WorldHelper.seaLevel && voxelmap.blockName == BlockName.Water)
        {
            CreatWaterMesh(offsetVec3, mesh);
        }
    }

    public static bool CheckTransparent(Vector3Int pos, Vector3Int offset, ChunkData c, World world)
    {
        VoxelMap[,,] voxelMapToCheck;
        if (pos.x < 0 || pos.x >= WorldHelper.chunkSize ||
           pos.z < 0 || pos.z >= WorldHelper.chunkSize)
        {
            Vector3Int neighbourChunkPos = c.chunkPos + new Vector3Int(
                (pos.x - offset.x) * WorldHelper.chunkSize,
                0,
                (pos.z - offset.z) * WorldHelper.chunkSize
                );

            pos.x = ConvertBlockIndexToLocal((int)pos.x);
            pos.z = ConvertBlockIndexToLocal((int)pos.z);

            ChunkData bChunk;
            if (World.ChunkDatas.TryGetValue(neighbourChunkPos, out bChunk))
            {
                voxelMapToCheck = bChunk.voxelMap;
            }
            else
                return false;
        }
        else
            voxelMapToCheck = c.voxelMap;
        if (pos.y < 0)
            return false;
        try
        {
            return world.blockTypes[(int)voxelMapToCheck[(int)pos.x, (int)pos.y, (int)pos.z].blockName].isTransparent;
        }
        catch (System.IndexOutOfRangeException)
        { }
        return true;
    }//以后可能要改
    public static int ConvertBlockIndexToLocal(int i)
    {
        if (i == -1)
            i = WorldHelper.chunkSize - 1;
        else if (i == WorldHelper.chunkSize)
            i = 0;

        return i;
    }
    public static void CreateMesh(Vector3 offsetVec3, int face, BlockName blockName, ChunkMesh mesh, World world, BlockMesh newMesh,bool has_Collider)
    {
        int blocknum = (int)blockName;
        mesh.AddVertices(offsetVec3, face, newMesh,has_Collider);
        mesh.AddTriangles(has_Collider);
        mesh.AddUvs(world.blockTypes[blocknum].GetTextureID(GetMeshFace(newMesh.faceOrigin_N[face])),has_Collider);
    }
    public static void CreatWaterMesh(Vector3 offsetVec3, ChunkMesh mesh)
    {
        mesh.AddWaterVertices(offsetVec3);
        mesh.AddWaterTriangles();
        mesh.AddWaterUvs(22);
    }
    public static int GetMeshFace(Vector3 vector3)
    {
        if(vector3 == Vector3.forward) return 0;
        else if(vector3 == Vector3.back) return 1;
        else if(vector3 == Vector3.up) return 2;
        else if(vector3 == Vector3.down) return 3;
        else if(vector3 == Vector3.left) return 4;
        else if(vector3 == Vector3.right) return 5;
        else return 0;
    }
}

