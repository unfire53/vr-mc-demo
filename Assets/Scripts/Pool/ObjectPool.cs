using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    Queue<Chunk> chunkList = new Queue<Chunk>();
    Queue<ChunkData> chunkDataList = new Queue<ChunkData>();
    public Chunk GetChunk(ChunkData chunkData,World world)
    {
        Chunk chunk;
        if(chunkList.Count > 0)
        {
            chunk = chunkList.Dequeue();
            chunk.DeQueue(chunkData);
        }
        else
        {
            chunk = new Chunk(chunkData,world);
        }
        return chunk;
    }
    public ChunkData GetChunkData(Vector3Int Pos)
    {
        ChunkData chunkData;
        if(chunkDataList.Count > 0)
        {
            chunkData = chunkDataList.Dequeue();
            chunkData.DeQueue(Pos);
        }else
        {
            chunkData = new ChunkData(Pos);
        }
        return chunkData;
    }
    public void EnQueue(Chunk chunk)
    {
        chunk.chunkObject.SetActive(false);
        chunkList.Enqueue(chunk);
    }
    public void EnQueue(ChunkData chunkData)
    {
        chunkDataList.Enqueue(chunkData);
    }
}
