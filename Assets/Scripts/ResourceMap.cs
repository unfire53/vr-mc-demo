using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class ResourceMap : MonoBehaviour
{
    public bool visualize = false;
    public Transform Ai; 
    public Queue<GameObject> poolQueue;
    public GameObject prefab;
    public List<GameObject> gameObjects;
    public Vector3 lastChunkPos;
    private void Start()
    {
        poolQueue = new Queue<GameObject>();
        gameObjects = new List<GameObject>();
    }
    private void Update()
    {
        if (visualize)
        {
            Draw(Ai);
        }
    }
    public void Set(Transform transform, bool b)
    {
        Ai = transform;
        visualize = b;
        if(!visualize)
        {
            foreach(GameObject go in gameObjects) {ReturnObjectToPool(go);}
            foreach (var obj in poolQueue)
            {
                if (gameObjects.Contains(obj))
                {
                    gameObjects.Remove(obj);
                }
            }
        }
    }
    public void Draw(Transform transform)
    {
        Chunk chunk;
        Vector3 pos = transform.position;
        Vector3Int chunkPos = ChunkHelper.GetChunkPos(pos);
        if (chunkPos == lastChunkPos) return;
        lastChunkPos = chunkPos;
        if (World.chunks.TryGetValue(chunkPos, out chunk))
        {
            if (chunk.chunkData.structPos.Count == 0) { return; }
            for(int i = 0;i<gameObjects.Count;i++)
            {
                int j;
                for (j = 0; j < chunk.chunkData.structPos.Count; j++)
                {
                    if (gameObjects[i].transform.position == chunkPos)
                        break;
                }
                if (j == chunk.chunkData.structPos.Count)
                {
                    ReturnObjectToPool(gameObjects[i]);
                }
            }
            foreach(var obj in poolQueue)
            {
                if(gameObjects.Contains(obj))
                {
                    gameObjects.Remove(obj);
                }
            }
            for (int i = 0; i < chunk.chunkData.structPos.Count; i++)
            {
                //GameObject game = GetObjectFromPool();
                //if (game != null)
                //{
                //    game.transform.position = chunk.chunkData.structPos[i].GetPos()+chunkPos ;
                //    game.transform.rotation = Quaternion.identity;
                //    gameObjects.Add(game);
                //}
                //else
                //{
                //    // 如果对象池中没有可用对象，则创建一个新对象
                //    game = Instantiate(prefab, chunk.chunkData.structPos[i].GetPos() + chunkPos, Quaternion.identity);
                //    gameObjects.Add(game);
                //}
            }
        }
    }

    private GameObject GetObjectFromPool()
    {
        if (poolQueue.Count > 0)
        {
            // 从对象池中获取一个对象
            GameObject obj = poolQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        // 如果对象池中没有可用对象，则返回null
        return null;
    }

    private void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}
