using Assets.Scripts.Mesh;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BulidVoxel : MonoBehaviour
{
    int x;
    int y;
    int z;
    int minI;
    float minLength;
    public BlockName voxelNum;
    public World world;
    Chunk chunkHit;
    public XRGrabInteractable xRGrab;
    bool is_grab = false;
    public Transform parent;

    Vector3[] directions = new Vector3[6]
    {
        Vector3.down,Vector3.up,Vector3.left,Vector3.right,Vector3.forward,Vector3.back
    };
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public Material material;
    public Controler controler;

    PredictVoxel predictVoxel;

    Vector3 hitVoxel;
    //初始化
    public void Set(World _world, BlockName blockType)
    {
        gameObject.name = blockType.ToString();
        this.parent = transform.parent;
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        xRGrab = gameObject.GetComponent<XRGrabInteractable>();
        //xRGrab.activated.AddListener(CreateVoxel);
        controler = GameObject.Find("XR Origin (XR Rig)").GetComponent<Controler>();
        
        world = _world;
        voxelNum = blockType;
        material = world.material1;
        CreatMesh();
        //predictVoxel = new PredictVoxel(vertices, triangles);
    }
    public void Set(World _world)
    {
        gameObject.name = voxelNum.ToString();
        this.parent = transform.parent;
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        xRGrab = gameObject.GetComponent<XRGrabInteractable>();
        controler = GameObject.Find("XR Origin (XR Rig)").GetComponent<Controler>();

        world = _world;
        material = world.material1;
        CreatMesh();
    }
    private void Start()
    {

        controler.OnSelectActive += CreateVoxel;
        xRGrab.selectEntered.AddListener(SlectEnter);
        xRGrab.selectExited.AddListener(SelectExit);
        
    }
    private void OnEnable()
    {
        StartCoroutine(Check());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator Check()
    {
        while (true) 
        {
            yield return new WaitForSeconds(0.1f);
            if (is_grab && transform.position.y < WorldHelper.chunkHeight)
            {
                Ray();
            }
        }
    }
    void Ray()
    {
        minI = 0;
        minLength = float.MaxValue;
        for (int i = 0; i < 6; i++)
        {
            RaycastHit hit1;
            if (Physics.Raycast(transform.position, directions[i], out hit1, 1, LayerMask.GetMask("Terrain")))
            {
                float nowLength = hit1.distance;

                if (nowLength < minLength)
                {
                    minLength = nowLength;
                    minI = i;
                }
            }
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directions[minI], out hit, 1, LayerMask.GetMask("Terrain")))
        {
            //找到方块pos
            hitVoxel = Vector3Int.RoundToInt(transform.position);
            //对应区块pos
            Vector3Int chunkPos = ChunkHelper.GetChunkPos(hitVoxel);


            if (World.chunks.TryGetValue(chunkPos, out chunkHit))
            {
                //找到voxelmap
                x = (int)Mathf.Abs(hitVoxel.x - chunkPos.x);
                y = (int)hitVoxel.y;
                z = (int)Mathf.Abs(hitVoxel.z - chunkPos.z);

                predictVoxel.predictVoxelObject.transform.position = hitVoxel;
                Vector3 rotation = TransformRotationToBlockRotation();
                predictVoxel.predictVoxelObject.transform.rotation = Quaternion.Euler(rotation);
                predictVoxel.renderer.enabled = true;
            }
        }
        else
        {
            predictVoxel.renderer.enabled = false;
        }
    }
    //IEnumerator Raycast()
    //{
    //    minI = 0;
    //    minLength = float.MaxValue;
    //    for (int i = 0; i < 6; i++)
    //    {
    //        RaycastHit hit1;
    //        if (Physics.Raycast(transform.position, directions[i], out hit1, 1,LayerMask.GetMask("Terrain")))
    //        {
    //            float nowLength = hit1.distance;

    //            if (nowLength < minLength)
    //            {
    //                minLength = nowLength;
    //                minI = i;
    //            }
    //        }
    //    }
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, directions[minI], out hit, 1, LayerMask.GetMask("Terrain")))
    //    {
    //        //找到方块pos
    //        hitVoxel = Vector3Int.RoundToInt(transform.position);
    //        //对应区块pos
    //        Vector3Int chunkPos = ChunkHelper.GetChunkPos(hitVoxel);


    //        if (World.chunks.TryGetValue(chunkPos, out chunkHit))
    //        {
    //            //找到voxelmap
    //            x = (int)Mathf.Abs(hitVoxel.x - chunkPos.x);
    //            y = (int)hitVoxel.y;
    //            z = (int)Mathf.Abs(hitVoxel.z - chunkPos.z);

    //            predictVoxel.predictVoxelObject.transform.position = hitVoxel;
    //            Vector3 rotation = TransformRotationToBlockRotation();
    //            predictVoxel.predictVoxelObject.transform.rotation = Quaternion.Euler(rotation);
    //            predictVoxel.renderer.enabled = true;
    //        }
    //    }
    //    else
    //    {
    //        predictVoxel.renderer.enabled = false;
    //    }
    //    yield return new WaitForSeconds(.1f);
    //}
    public Vector3 TransformRotationToBlockRotation()
    {
        Vector3 rotation = transform.rotation.eulerAngles / 90;
        int x = Mathf.RoundToInt(rotation.x);
        int y = Mathf.RoundToInt(rotation.y);
        int z = Mathf.RoundToInt(rotation.z);
        if (x == 4) x = 0;
        if (y == 4) y = 0;
        if (z == 4) z = 0;
        return new Vector3(x * 90, y * 90, z * 90);
    }
    private static Vector3[] RotateVerts(Vector3[] verts, Quaternion rotation)
    {
        // ... 旋转顶点的方法 ...
        Vector3[] rotatedVerts = new Vector3[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            rotatedVerts[i] = rotation * verts[i];
        }
        return rotatedVerts;
    }
    public static int VectorSort(Vector3 v1, Vector3 v2)
    {
        float epsilon = 0.0001f;
        float diff = Mathf.Abs(v1.z - v2.z);
        //相等
        if (diff < epsilon)
        {
            diff = Mathf.Abs(v1.x - v2.x);
            //相等
            if (diff < epsilon)
            {
                diff = Mathf.Abs(v1.y - v2.y);
                //相等
                if (diff < epsilon)
                {
                    return 0;
                }
                else if (v1.y > v2.y)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else if (v1.x > v2.x)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        else if (v1.z > v2.z)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
    public Direction_Extended GetDirection(Vector3 vector3)
    {
        bool find = false;
        Dictionary<Direction_Extended, BlockMesh> keyValuePairs;
        BlockMesh newMesh;
        BlockType.VoxelShape voxelShape = world.blockTypes[(int)voxelNum].voxelShape;
        switch (voxelShape)
        {
            case BlockType.VoxelShape.Block: return Direction_Extended.Up;
            case BlockType.VoxelShape.Block_dir:
                keyValuePairs = CubeMesh_Dir.CubeMeshes;
                keyValuePairs.TryGetValue(Direction_Extended.Cube_Up, out newMesh); break;
            case BlockType.VoxelShape.Slab:
                keyValuePairs = SlabMesh.SlabMeshes;
                keyValuePairs.TryGetValue(Direction_Extended.Slab_Up, out newMesh); break;
            case BlockType.VoxelShape.Step:
                keyValuePairs = StepMesh.StepMeshes;
                keyValuePairs.TryGetValue(Direction_Extended.Step_Up_Front, out newMesh); break;
            case BlockType.VoxelShape.Furniture:
                keyValuePairs = FurnitureMesh_Dir.CubeMeshes;
                keyValuePairs.TryGetValue(Direction_Extended.Cube_Left, out newMesh); break;
            case BlockType.VoxelShape.Glass:
                keyValuePairs = GlassMesh.GlassMeshes;
                keyValuePairs.TryGetValue(Direction_Extended.Cube_Front, out newMesh); break;
            case BlockType.VoxelShape.Plant:
                keyValuePairs = PlantMesh.CubeMeshes;
                keyValuePairs.TryGetValue(Direction_Extended.Up, out newMesh); break;
            //错误情况
            default: keyValuePairs = CubeMesh.CubeMeshes; Debug.Log("getdirection"); keyValuePairs.TryGetValue(Direction_Extended.Up, out newMesh); break;
        }
        Vector3[] vector3s = RotateVerts(newMesh.Verts_N, Quaternion.Euler(vector3));
        Array.Sort(vector3s, VectorSort);
        foreach (KeyValuePair<Direction_Extended, BlockMesh> kvp in keyValuePairs)
        {
            Vector3[] sortVert = new Vector3[kvp.Value.Verts_N.Length];
            Array.Copy(kvp.Value.Verts_N, sortVert, kvp.Value.Verts_N.Length);
            Array.Sort(sortVert, VectorSort);
            for (int i = 0; i < sortVert.Length; i++)
            {
                if (sortVert[i] != vector3s[i])
                {
                    break;
                }
                if (i == sortVert.Length - 1)
                {
                    find = true;
                }
            }
            if (find)
            {
                return kvp.Key;
            }
        }
        return Direction_Extended.Up;
    }

    private void CreateVoxel(IXRInteractor xrInteractor)
    {
        if (xrInteractor != xRGrab.firstInteractorSelecting) { return; }
        if (predictVoxel.renderer.enabled)
        {
            if (!world.isModify)
            {
                VoxelMap voxelMap = new VoxelMap(voxelNum);
                Vector3 vector3 = TransformRotationToBlockRotation();
                Direction_Extended direction = GetDirection(vector3);
                voxelMap.GetBlock(direction);
                chunkHit.chunkData.SetVoxel(x, y, z, voxelMap);
            }
            else
            {
                switch (world.modifyStructure.structureType)
                {
                    case StructureType.house:
                        if (voxelNum == BlockName.Bed) { world.modifyStructure.ConditionAdd(); }
                        break;
                    case StructureType.farmland:
                        if (voxelNum == BlockName.Wheat_1) { world.modifyStructure.ConditionAdd(); }
                        break;
                    case StructureType.Lumberyards:
                        if (voxelNum == BlockName.Board) { world.modifyStructure.ConditionAdd(); }
                        break;
                }
                VoxelMap voxelMap = new VoxelMap(voxelNum);
                Vector3 vector3 = TransformRotationToBlockRotation();
                Direction_Extended direction = GetDirection(vector3);
                voxelMap.GetBlock(direction);
                Vector3Int pos = new Vector3Int(x, y, z) + chunkHit.chunkData.chunkPos;
                Debug.Log(pos.ToString());
                Structure_Node structure_Node = new Structure_Node(pos, voxelMap);
                world.modifyStructure.AddNode(structure_Node);
                world.modifyStructure.UpdateMesh();
            }

        }
    }
    public void SlectEnter(SelectEnterEventArgs args)
    {
        is_grab = true;
        gameObject.transform.DetachChildren();
        xRGrab.SetTargetLocalScale(Vector3.one);
        predictVoxel = PredictVoxel_Pool.instance.DeQueue(vertices,triangles);
        //controler.OnSelectActive += CreateVoxel;
    }
    public void SelectExit(SelectExitEventArgs args)
    {
        is_grab = false;
        ReturnVoxel();
        gameObject.transform.parent = parent;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = Vector3.one;
        //controler.OnSelectActive -= CreateVoxel;
    }
    public void ReturnVoxel()
    {
        if (predictVoxel == null) return;
        PredictVoxel_Pool.instance.EnQueue(predictVoxel);
        predictVoxel = null;
    }
    public void CreatMesh()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        BlockMesh newMesh;
        BlockType.VoxelShape voxelShape = world.blockTypes[(int)voxelNum].voxelShape;
        switch (voxelShape)
        {
            case BlockType.VoxelShape.Furniture:
            case BlockType.VoxelShape.Block_dir:
            case BlockType.VoxelShape.Block:
                CubeMesh.CubeMeshes.TryGetValue(Direction_Extended.Up, out newMesh); break;
            case BlockType.VoxelShape.Slab: SlabMesh.SlabMeshes.TryGetValue(Direction_Extended.Slab_Up, out newMesh); break;
            case BlockType.VoxelShape.Step: StepMesh.StepMeshes.TryGetValue(Direction_Extended.Step_Up_Front, out newMesh); break;
            case BlockType.VoxelShape.Glass: GlassMesh.GlassMeshes.TryGetValue(Direction_Extended.Cube_Front, out newMesh); break;
            case BlockType.VoxelShape.Plant: PlantMesh.CubeMeshes.TryGetValue(Direction_Extended.Up, out newMesh); break;
            default: CubeMesh.CubeMeshes.TryGetValue(Direction_Extended.Up, out newMesh); break;
        }

        for (int face = 0; face < newMesh.faceCheck_N.Length; face++)
        {

            vertices.Add(newMesh.Verts_N[newMesh.Tris_N[face, 0]]);
            vertices.Add(newMesh.Verts_N[newMesh.Tris_N[face, 1]]);
            vertices.Add(newMesh.Verts_N[newMesh.Tris_N[face, 2]]);
            vertices.Add(newMesh.Verts_N[newMesh.Tris_N[face, 3]]);
            if (newMesh.faceCheck_N.Length > 6)
                AddUvs(world.blockTypes[(int)voxelNum].GetTextureID(ChunkHelper.GetMeshFace(newMesh.faceOrigin_N[face])));
            else
                AddUvs(world.blockTypes[(int)voxelNum].GetTextureID(face));
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 1);

        }
        meshRenderer.material = material;
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
    void AddUvs(int textureID)
    {
        float y = textureID / WorldHelper.TextureAtlasSizeInBlocks;
        float x = textureID - (y * WorldHelper.TextureAtlasSizeInBlocks);

        y *= WorldHelper.NormalizedBlockTextureSize;
        x *= WorldHelper.NormalizedBlockTextureSize;

        y = 1f - y - WorldHelper.NormalizedBlockTextureSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + WorldHelper.NormalizedBlockTextureSize));
        uvs.Add(new Vector2(x + WorldHelper.NormalizedBlockTextureSize, y));
        uvs.Add(new Vector2(x + WorldHelper.NormalizedBlockTextureSize, y + WorldHelper.NormalizedBlockTextureSize));
    }
}

