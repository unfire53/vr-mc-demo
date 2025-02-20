using System.Linq;
using UnityEngine;
//ChunkData 存储VoxelMap类（方块类型，朝向）
//ChunkMesh 存储网格
public class Chunk
{
    public bool has_water = false;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    public GameObject chunkObject;

    public World world;
    public ChunkData chunkData;
    public ChunkMesh chunkMesh;

    //public ResourceVisualize resourceVisualize;
    public Chunk(ChunkData data, World world)
    {
        this.chunkData = data;
        this.world = world;
        chunkObject = new GameObject();
        chunkObject.transform.parent = world.transform;
        chunkObject.transform.position = data.chunkPos;
        chunkObject.name = data.chunkPos.ToString();
        chunkObject.layer = 8;
        meshFilter = chunkObject.AddComponent<MeshFilter>();
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        meshCollider = chunkObject.AddComponent<MeshCollider>();
        meshFilter.mesh.subMeshCount = 2;
        meshRenderer.sharedMaterials = new Material[2]
        {
            world.material,world .transparentMaterial
        };
        chunkData.OnUpdateMesh += Update;
        //resourceVisualize = new ResourceVisualize(this);
    }
    public void DeQueue(ChunkData chunkData)
    {
        this.chunkData = chunkData;
        chunkObject.transform.position = chunkData.chunkPos;
        chunkObject.name = chunkData.chunkPos.ToString();
    }
    public void Update()
    {
        CreatChunkMesh();
        UpdateMesh();
        chunkData.ismodified = false;
    }
    public void CreatChunkMesh()
    {
        chunkMesh = CreatChunkMesh(chunkData, world);//, has_water);
    }
    public static ChunkMesh CreatChunkMesh(ChunkData data,World world)//,bool has_water)
    {
        ChunkMesh mesh = new ChunkMesh(true);
        for (int x = 0; x < WorldHelper.chunkSize;x++)
        {
            for (int z = 0; z < WorldHelper.chunkSize; z++)
            {
                for (int y = 0; y < WorldHelper.chunkHeight; y++)
                {
                    ChunkHelper.CheckMesh(new Vector3Int(x,y,z),data,mesh,world);
                }
            }
        }
        return mesh;
    }
    public void UpdateMesh()
    {
        UpdateMesh(chunkMesh);
    }
    public void UpdateMesh(ChunkMesh chunkMesh)
    {
        meshFilter.mesh.Clear();
        Mesh mesh = new Mesh();
        mesh.subMeshCount = 2;
        mesh.vertices = chunkMesh.vertices_Collider.Concat(chunkMesh.vertices).Concat(chunkMesh.chunkMesh_Water.vertices).ToArray();
        mesh.SetTriangles(chunkMesh.triangles_Collider
            .Concat(chunkMesh.triangles.Select((val) => val + chunkMesh.vertices_Collider.Count)).ToArray(),0);
        mesh.SetTriangles(chunkMesh.chunkMesh_Water.triangles.Select(val => val + chunkMesh.vertices_Collider.Count + chunkMesh.vertices.Count).ToArray(),1);
        mesh.uv = chunkMesh.uvs_Collider.Concat(chunkMesh.uvs).Concat(chunkMesh.chunkMesh_Water.uvs).ToArray();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = null;
        Mesh colliderMesh = new Mesh();
        colliderMesh.vertices = chunkMesh.vertices_Collider.ToArray();
        colliderMesh.triangles = chunkMesh.triangles_Collider.ToArray();
        colliderMesh.RecalculateNormals();
        meshCollider.sharedMesh = colliderMesh;
        meshFilter.mesh = mesh;
    }
}

