using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.InputSystem.HID;

public class World : MonoBehaviour
{
    public string worldName;
    public int seed = 1;
    public static int[] offsets = new int[3];

    ObjectPool pool;
    WorldData worldData;
    public BlockType[] blockTypes;
    public Material material;
    public Material material1;
    public Material transparentMaterial;
    public Material ResourceMaterial;
    public Transform player;

    public static Dictionary<Vector3Int, ChunkData> ChunkDatas;
    public static Dictionary<Vector3Int, Chunk> chunks;

    public List<Vector3Int> newDrawChunks;
    public List<Vector3Int> chunkList;
    public List<Vector3Int> chunkDataList;

    public List<Chunk> draw;
    public List<Chunk> treeChunksList;

    public AnimationCurve Curve_Continentalness;
    public AnimationCurve Curve_PeakAndValleys;
    public AnimationCurve Curve_Erosion;

    public CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public ModifyStructure modifyStructure;

    public bool isModify = false;

    // Start is called before the first frame update
    void Start()
    {
        material.color = Color.white;
        pool = gameObject.GetComponent<ObjectPool>();
        worldName = MyManager.instance.worldName;
        seed = MyManager.instance.seed;
        ChunkDatas = new Dictionary<Vector3Int, ChunkData>();
        chunks = new Dictionary<Vector3Int, Chunk>();
        newDrawChunks = new List<Vector3Int>();
        chunkDataList = new List<Vector3Int>();
        chunkList = new List<Vector3Int>();
        draw = new List<Chunk>();
        treeChunksList = new List<Chunk>();
        UnityEngine.Random.InitState(seed);
        for (int i = 0; i < 3; i++)
        {
            offsets[i] = UnityEngine.Random.Range(1000, 10000);
        }
        if (!LoadWorld())
        {
            player.position = new Vector3(20, -0.7f, 20);
        }
        CheckView();
    }
    #region 动态加载,废弃
    //IEnumerator PlayerPos()
    //{
    //    playerCPos = new Vector3Int((int)player.position.x / WorldHelper.chunkSize,
    //                                0,
    //                                (int)player.position.z / WorldHelper.chunkSize);
    //    if (playerCPos != playerLastCPos)
    //    {
    //        CheckView();
    //    }
    //    playerLastCPos = playerCPos;
    //    yield return new WaitForSeconds(0.1f);
    //    StartCoroutine(PlayerPos());
    //}
    #endregion
    async void CheckView()//populate阶段，不绘制。
    {
        chunkDataList.Clear();
        chunkList.Clear();
        newDrawChunks.Clear();
        for (int x = -WorldHelper.populateRadius; x <= WorldHelper.populateRadius; x++)
        {
            for (int z = -WorldHelper.populateRadius; z <= WorldHelper.populateRadius; z++)
            {
                Vector3Int chunkPos = (new Vector3Int(x, 0, z)) * WorldHelper.chunkSize;
                chunkDataList.Add(chunkPos);
            }
        }
        for (int x = -WorldHelper.DrawRadius; x <= WorldHelper.DrawRadius; x++)
        {
            for (int z = -WorldHelper.DrawRadius; z <= WorldHelper.DrawRadius; z++)
            {
                Vector3Int chunkPos = (new Vector3Int(x, 0, z)) * WorldHelper.chunkSize;
                chunkList.Add(chunkPos);
            }
        }
        #region 动态加载,废弃
        //for (int x = 0; x < popRadiusChunks.Count; x++)
        //{
        //    if (!popChunkDatas.ContainsKey(popRadiusChunks[x]))
        //    {
        //        newPopChunks.Add(popRadiusChunks[x]);
        //    }
        //}
        //for (int x = 0; x < drawRadiusChunks.Count; x++)
        //{
        //    if (!chunks.ContainsKey(drawRadiusChunks[x]))
        //    {
        //        newDrawChunks.Add(drawRadiusChunks[x]);
        //    }
        //}
        //foreach (KeyValuePair<Vector3, ChunkData> c in popChunkDatas)
        //{
        //    if (!popRadiusChunks.Contains(c.Key))
        //    {
        //        destroyChunkDatasList.Add(c.Key);
        //    }
        //}
        //foreach (KeyValuePair<Vector3, Chunk> c in chunks)
        //{
        //    if (!drawRadiusChunks.Contains(c.Key))
        //    {
        //        destroyChunksList.Add(c.Key);
        //    }
        //}
        //foreach (Vector3 vector3 in destroyChunkDatasList)
        //{
        //    //popChunkDatas.TryRemove(vector3, out ChunkData chunkdata);
        //    pool.EnQueue(popChunkDatas[vector3]);
        //    popChunkDatas.Remove(vector3);
        //}
        //foreach (Vector3 vector3 in destroyChunksList)
        //{
        //    pool.EnQueue(chunks[vector3]);
        //    chunks.Remove(vector3);
        //}
        //List<ChunkData> chunkdatasToCreate = new List<ChunkData>();
        //foreach (Vector3 vector in popRadiusChunks)
        //{
        //    ChunkData chunkData = pool.GetChunkData(vector);
        //    chunkdatasToCreate.Add(chunkData);
        //    popChunkDatas.TryAdd(vector, chunkData);
        //}
        //List<ChunkData> drawChunksToCreate = new List<ChunkData>();
        //foreach (Vector3 vector in drawRadiusChunks)
        //{
        //    ChunkData chunkData = popChunkDatas[vector];
        //    drawChunksToCreate.Add(chunkData);
        //}
        //foreach (ChunkData data in drawChunksToCreate)
        //{
        //    Chunk chunk = pool.GetChunk(data, this);
        //    chunks.TryAdd(chunk.chunkPos, chunk);
        //}

        #endregion
        List<ChunkData> chunkdatasToCreate = new List<ChunkData>();
        foreach (Vector3Int vector in chunkDataList)
        {
            ChunkData chunkData = pool.GetChunkData(vector);
            chunkdatasToCreate.Add(chunkData);
            ChunkDatas.TryAdd(vector, chunkData);
        }
        List<ChunkData> drawChunksToCreate = new List<ChunkData>();
        foreach (Vector3Int vector in chunkList)
        {
            ChunkData chunkData = ChunkDatas[vector];
            drawChunksToCreate.Add(chunkData);
        }
        foreach (ChunkData data in drawChunksToCreate)
        {
            Chunk chunk = pool.GetChunk(data, this);
            chunks.TryAdd(chunk.chunkData.chunkPos, chunk);
        }
        ConcurrentDictionary<ChunkData, ChunkMesh> dictionary;
        try
        {
            await CalculateVoxelMap(chunkdatasToCreate);
            await CalculateTree(drawChunksToCreate);
            #region 动态加载,废弃
            //List<ChunkData> secoundDrawChunk = await CalculateTree(drawChunksToCreate);
            //foreach (var chunk in secoundDrawChunk)
            //{
            //    if (!drawChunksToCreate.Contains(chunk))
            //        drawChunksToCreate.Add(chunk);
            //}
            #endregion
            dictionary = await CalculateMeshData(drawChunksToCreate);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("task stop");
            return;
        }
        StartCoroutine(UpdateMesh(dictionary));
    }
    private async Task CalculateTree(List<ChunkData> drawChunkDatas)
    {

        await Task.Run(() =>
        {
            for (int y = 0; y < drawChunkDatas.Count; y++)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                }
                ChunkData c = drawChunkDatas[y];
                FeaturesGenerator.LoopVoxelMap(c);
            }
        }, cancellationTokenSource.Token);
    }

    private async Task CalculateVoxelMap(List<ChunkData> chunkdatas)
    {
        await Task.Run(() =>
        {
            foreach (ChunkData chunkData in chunkdatas)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                }
                TerrainGenerator.TerrainGenerate(chunkData,Curve_Continentalness,Curve_PeakAndValleys,Curve_Erosion);
            }
        }, cancellationTokenSource.Token);
    }

    private Task<ConcurrentDictionary<ChunkData, ChunkMesh>> CalculateMeshData(List<ChunkData> drawChunksToCreate)
    {
        ConcurrentDictionary<ChunkData, ChunkMesh> keyValuePairs = new ConcurrentDictionary<ChunkData, ChunkMesh>();
        return Task.Run(() =>
        {
            for (int i = 0; i < drawChunksToCreate.Count; i++)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                }
                ChunkMesh chunkMesh = Chunk.CreatChunkMesh(drawChunksToCreate[i], this);
                keyValuePairs.TryAdd(drawChunksToCreate[i], chunkMesh);
            }
            return keyValuePairs;
        }, cancellationTokenSource.Token);
    }
    IEnumerator UpdateMesh(ConcurrentDictionary<ChunkData, ChunkMesh> keyValuePairs)
    {
        while (keyValuePairs.Count > 0)
        {
            foreach (var kvp in keyValuePairs)
            {
                if (chunks.TryGetValue(kvp.Key.chunkPos, out Chunk chunk))
                {
                    chunk.chunkMesh = kvp.Value;
                    chunk.UpdateMesh();
                    if (!chunk.chunkObject.activeSelf)
                        chunk.chunkObject.SetActive(true);
                }
                keyValuePairs.TryRemove(kvp.Key, out ChunkMesh chunkMesh);
                yield return new WaitForEndOfFrame();
            }
        }
    }
    
    #region 不知道干啥的
    /// <summary>
    /// ///////
    /// </summary>
    /// <param name="blockNum"></param>
    /// <returns></returns>
    //public Vector3Int GetTargerPos(byte blockNum)
    //{
    //    Vector3Int pos = Vector3Int.zero;
    //    foreach (KeyValuePair<Vector3, ChunkData> keyValuePair in ChunkDatas)
    //    {
    //        pos = keyValuePair.Value.GetTargerPos(blockNum);
    //        if (pos != Vector3Int.zero)
    //            return pos;
    //    }
    //    return pos;
    //}
    //public static bool Node_buttom_Isempty(Vector3Int vector3)
    //{
    //    Vector3 chunkPos = new Vector3
    //        (Mathf.Floor(vector3.x / (float)WorldHelper.chunkSize), 0,
    //        Mathf.Floor(vector3.z / (float)WorldHelper.chunkSize));
    //    chunkPos = chunkPos * WorldHelper.chunkSize;
    //    Vector3Int LocalPos = vector3
    //        - new Vector3Int((int)chunkPos.x, (int)chunkPos.y, (int)chunkPos.z);
    //    int x = Mathf.Abs(LocalPos.x);
    //    int y = Mathf.Abs(LocalPos.y);
    //    int z = Mathf.Abs(LocalPos.z);
    //    ChunkData data;
    //    if (y < WorldHelper.chunkHeight && y > 0)
    //    {
    //        if (ChunkDatas.TryGetValue(chunkPos, out data))
    //        {
    //            if (data.voxelMap[x, y, z].blockName == BlockName.Step) //data.voxelMap[x,y,z].blocknum ！=water)    ///不能走水上
    //                return true;
    //        }
    //    }
    //    return false;
    //}
    #endregion
    public static bool CheckPosCanWalk(Vector3Int worldPos)
    {
        Vector3Int chunkPos = Vector3Int.zero;
        Vector3Int localPos = Vector3Int.zero;
        ChunkHelper.GetChunkPosAndLocalPos(worldPos, ref chunkPos, ref localPos);
        int x = localPos.x; int y = localPos.y; int z = localPos.z;
        ChunkData data;
        if (y < WorldHelper.chunkHeight && y > 0)
        {
            if (ChunkDatas.TryGetValue(chunkPos, out data))
            {
                switch (data.voxelMap[x, y, z].blockName)
                {
                    case BlockName.Air: case BlockName.Plant: case BlockName.Mushroom:
                    case BlockName.Vegetable: case BlockName.flower:case BlockName.Water:
                    case BlockName.Wheat_1:case BlockName.Wheat_2:case BlockName.Wheat_3:
                        return true;
                    default: break;
                }
            }
        }
        return false;
    }
    public static bool CheckOverHeight(Vector3Int worldPos)
    {
        Vector3Int chunkPos = Vector3Int.zero;
        Vector3Int localPos = Vector3Int.zero;
        ChunkHelper.GetChunkPosAndLocalPos(worldPos, ref chunkPos, ref localPos);
        int x = localPos.x; int y = localPos.y; int z = localPos.z;
        ChunkData data;
        if (y < WorldHelper.chunkHeight && y > 2)
        {
            if (ChunkDatas.TryGetValue(chunkPos, out data))
            {
                switch(data.voxelMap[x, y, z].blockName)
                {
                    case BlockName.Water:
                        if (data.voxelMap[x,y + 1, z].blockName == BlockName.Water)
                        {  return true; }
                        break;
                    default:
                        if ((data.voxelMap[x, y - 1, z].blockName == BlockName.Air || data.voxelMap[x, y - 1, z].blockName == BlockName.Water)
                            && (data.voxelMap[x, y - 2, z].blockName == BlockName.Air || data.voxelMap[x, y - 2, z].blockName == BlockName.Water))
                        {
                            return true;
                        }
                        break;
                }
            }
        }
        return false;
    }

    public static bool GetHeight(Vector3Int worldPos, ref int PosY)
    {
        Vector3Int chunkPos = Vector3Int.zero;
        Vector3Int localPos = Vector3Int.zero;
        ChunkHelper.GetChunkPosAndLocalPos(worldPos, ref chunkPos, ref localPos);
        int x = localPos.x; int y = localPos.y; int z = localPos.z;
        ChunkData data;
        if (ChunkDatas.TryGetValue(chunkPos, out data))
        {
            for (int j = WorldHelper.chunkHeight - 1; j >= 0; j--)
            {
                if(data.voxelMap[x, j, z].blockName == BlockName.Water)
                {
                    return false;
                }
                if (data.voxelMap[x, j, z].blockName == BlockName.Grass)
                {
                    PosY = j;
                    return true;
                }
            }
        }
        return false;
    }
    public static void BackToStartScene()
    {
        MyManager.instance.LoadStartScene();
    }

    private void OnDisable()
    {
        cancellationTokenSource.Cancel();
    }
    public bool LoadWorld()
    {
        string filepath = Application.persistentDataPath + "/SaveDatas/" + worldName;
        if (File.Exists(filepath + "/ world.text"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(filepath + "/ world.text", FileMode.Open);
            worldData = new WorldData();
            worldData = bf.Deserialize(stream) as WorldData;
            stream.Close();
            player.position = new Vector3(worldData.playerPosX, 0, worldData.playerPosY);
            player.localScale = new Vector3(worldData.playerScale, worldData.playerScale, worldData.playerScale);
            transform.rotation = Quaternion.Euler(worldData.playerRotationX, worldData.playerRotationY, worldData.playerRotationZ);
            return true;
        }
        else
            return false;
        //else
        //Debug.Log("failed");
    }
    public void SaveWorld()
    {
        string filepath = Application.persistentDataPath + "/SaveDatas/" + worldName;
        if (!File.Exists(filepath))
            Directory.CreateDirectory(filepath);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = File.Open(filepath + "/ world.text", FileMode.OpenOrCreate);
        worldData = new WorldData(worldName, seed, player.position.x, player.position.z, player.localScale.x, transform.rotation.eulerAngles);
        bf.Serialize(stream, worldData);
        stream.Close();
        Debug.Log("save!" + filepath);
        StartCoroutine("SaveChunk");

    }
    IEnumerator SaveChunk()
    {
        foreach (var item in ChunkDatas)
        {
            item.Value.SaveChunk();
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine("SaveChunk");
    }
    public void ModifyMode()
    {
        material.color = new Color(0.18f, 0.18f, 0.18f);
        isModify = true;
    }
    public void NormalMode()
    {
        material.color = Color.white;
        isModify = false;
        modifyStructure.clearMesh();
    }
}

[System.Serializable]
public class BlockType
{//方块类型
    [HideInInspector]
    public enum VoxelShape
    {
        Block, Block_dir, Slab, Step, Furniture, Glass, Plant
    };

    public string blockName;
    public bool isTransparent;
    public VoxelShape voxelShape;
    //1种方块6个面分别赋值
    [Header("Texture Value")]
    public int backFaceIndex;
    public int frontFaceIndex;
    public int topFaceIndex;
    public int bottomFaceIndex;
    public int leftFaceIndex;
    public int rightFaceIndex;

    public int GetTextureID(int faceIndex)
    {
        switch (faceIndex)
        {
            case 0:
                return backFaceIndex;
            case 1:
                return frontFaceIndex;
            case 2:
                return topFaceIndex;
            case 3:
                return bottomFaceIndex;
            case 4:
                return leftFaceIndex;
            case 5:
                return rightFaceIndex;
            default:
                Debug.Log("GetTextureID!");
                return 0;
        }
    }
}

[System.Serializable]
public class WorldData
{
    public string name;
    public int seed;
    public float playerPosX;
    public float playerPosY;
    public float playerScale;
    public float playerRotationX;
    public float playerRotationY;
    public float playerRotationZ;
    public WorldData() { }
    public WorldData(string _name, int _seed, float posX, float posY, float scale, Vector3 vector3)
    {
        name = _name;
        seed = _seed;
        playerPosX = posX;
        playerPosY = posY;
        playerScale = scale;
        playerRotationX = vector3.x;
        playerRotationY = vector3.y;
        playerRotationZ = vector3.z;
    }
    public WorldData(string _name, int _seed)
    {
        name = _name;
        seed = _seed;
        playerPosX = 0;
        playerPosY = 0;
        playerScale = 70;
        playerRotationX = 0;
        playerRotationY = 0;
        playerRotationZ = 0;
    }
}
