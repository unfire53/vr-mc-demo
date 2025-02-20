using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

    /// <summary>
    /// 事件的拥有者 ChunkData
    /// 事件 OnUpdateMesh
    /// 事件的响应者 
    /// 事件处理器
    /// 订阅关系
    /// </summary>
    public class ChunkData
    {
        public bool ismodified;
        public bool hasChunkData;

        public Vector3Int chunkPos;
        public List<IResource> structPos;
        public VoxelMap[,,] voxelMap;
        public ChunkDataSave chunkDataSave;
        public event Action OnUpdateMesh;
        public event Action<int,int,int,BlockName,BlockName,ChunkData> OnUpdateModify;
        public ChunkData() { }
        public ChunkData(Vector3Int _chunkPos)
        {
            voxelMap = new VoxelMap[WorldHelper.chunkSize, WorldHelper.chunkHeight, WorldHelper.chunkSize];
            for (int x = 0; x < WorldHelper.chunkSize; x++)
            {
                for (int y = 0; y < WorldHelper.chunkHeight; y++)
                {
                    for (int z = 0; z < WorldHelper.chunkSize; z++)
                    {
                        voxelMap[x, y, z] = new VoxelMap();
                    }
                }
            }
            chunkPos = _chunkPos;
            structPos = new List<IResource>();
            hasChunkData = LoadChunk();
        }

        public void SetVoxel(int x,int y,int z ,VoxelMap voxelMap)
        {
            BlockName name =  this.voxelMap[x, y, z].blockName;
            this.voxelMap[x, y, z] = voxelMap;
            if(OnUpdateModify != null)
                OnUpdateModify(x, y, z, name, voxelMap.blockName, this);
            OnUpdateMesh();
        }

        public void DeQueue(Vector3Int _chunkpos)
        {
            chunkPos = _chunkpos;
            structPos.Clear();
            hasChunkData = LoadChunk();
        }
        public void IsModify()
        {
            ismodified = true;
            OnUpdateMesh();
        }
        public bool LoadChunk()
        {
            string filepath = Application.persistentDataPath + "/SaveDatas/" + MyManager.instance.worldName;
            if (File.Exists(filepath + "/" + chunkPos.ToString() + ".text"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream stream = File.Open(filepath + "/" + chunkPos.ToString() + ".text", FileMode.Open);
                chunkDataSave = GetChunkDataSave(true);
                chunkDataSave = bf.Deserialize(stream) as ChunkDataSave;
                stream.Close();
                //Debug.Log("chunk load!");
                return true;
            }
            else
            {
                //Debug.Log("chunk failed");
                return false;
            }

        }
        public void SaveChunk()
        {
            string filepath = Application.persistentDataPath + "/SaveDatas/" + MyManager.instance.worldName;
            if (!File.Exists(filepath))
                Directory.CreateDirectory(filepath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(filepath + "/" + chunkPos.ToString() + ".text", FileMode.OpenOrCreate);
            chunkDataSave = GetChunkDataSave(false);
            bf.Serialize(stream, chunkDataSave);
            stream.Close();
            Debug.Log("chunk save!" + filepath);
        }
        public ChunkDataSave GetChunkDataSave(bool Load)
        {
            ChunkDataSave DataSave;
            if (chunkDataSave != null)
            {
                DataSave = chunkDataSave;
            }
            else if (Load)
                DataSave = new ChunkDataSave();
            else
                DataSave = new ChunkDataSave(voxelMap);
            return DataSave;
        }
    }
    [System.Serializable]
    public class ChunkDataSave
    {
        public VoxelMap[,,] voxelMap = new VoxelMap[WorldHelper.chunkSize, WorldHelper.chunkHeight, WorldHelper.chunkSize];
        public ChunkDataSave() { }
        public ChunkDataSave(VoxelMap[,,] _voxelMap)
        {
            for (int x = 0; x < WorldHelper.chunkSize; x++)
            {
                for (int y = 0; y < WorldHelper.chunkHeight; y++)
                {
                    for (int z = 0; z < WorldHelper.chunkSize; z++)
                    {
                        voxelMap[x, y, z] = new VoxelMap();
                    }
                }
            }
            for (int x = 0; x < WorldHelper.chunkSize; x++)
            {
                for (int y = 0; y < WorldHelper.chunkHeight; y++)
                {
                    for (int z = 0; z < WorldHelper.chunkSize; z++)
                    {
                        voxelMap[x, y, z].blockName = _voxelMap[x, y, z].blockName;
                    }
                }
            }
        }
    }

