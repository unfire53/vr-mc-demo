using Assets.Scripts.Mesh;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum Direction_Extended : int
{
    Up,
    Down,
    North,
    South,
    East,
    West,
    // Cube的方向
    Cube_Up,
    Cube_Down,
    Cube_Front,
    Cube_Back,
    Cube_Left,
    Cube_Right,
    // Slab的方向
    Slab_Up,
    Slab_Down,
    Slab_Left,
    Slab_Right,
    Slab_Front,
    Slab_Back,
    // Step的方向
    Step_Up_Front,
    Step_Up_Back,
    Step_Up_Left,
    Step_Up_Right,
    Step_Down_Front,
    Step_Down_Back,
    Step_Down_Left,
    Step_Down_Right,
    Step_Mid_Front,
    Step_Mid_Back,
    Step_Mid_Left,
    Step_Mid_Right,
}

public static class MeshDataLibrary
{
    public static Dictionary<BlockType.VoxelShape, Dictionary<Direction_Extended,BlockMesh>> keyValuePairs = new Dictionary<BlockType.VoxelShape, Dictionary<Direction_Extended, BlockMesh>>()
    {
        {BlockType.VoxelShape.Block, CubeMesh.CubeMeshes},
        {BlockType.VoxelShape.Block_dir,CubeMesh_Dir.CubeMeshes },
        {BlockType.VoxelShape.Slab, SlabMesh.SlabMeshes },
        {BlockType.VoxelShape.Step, StepMesh.StepMeshes },
        {BlockType.VoxelShape.Furniture,FurnitureMesh_Dir.CubeMeshes },
        {BlockType.VoxelShape.Glass, GlassMesh.GlassMeshes },
        {BlockType.VoxelShape.Plant,PlantMesh.CubeMeshes}
    };
    
    public static BlockMesh GetMesh(VoxelMap voxelmap, World world)
    {
        BlockMesh block;
        Dictionary<Direction_Extended, BlockMesh> Dictionary_kid;
        if (keyValuePairs.TryGetValue(world.blockTypes[(int)voxelmap.blockName].voxelShape, out Dictionary_kid))
        {
            switch(world.blockTypes[(int)voxelmap.blockName].voxelShape)
            {
                case BlockType.VoxelShape.Block: case BlockType.VoxelShape.Plant:
                    if (Dictionary_kid.TryGetValue(Direction_Extended.Up, out block)) return block;
                    else return Dictionary_kid.First().Value;
                case BlockType.VoxelShape.Block_dir: case BlockType.VoxelShape.Slab:case BlockType.VoxelShape.Step: case BlockType.VoxelShape.Furniture : case BlockType.VoxelShape.Glass:
                    if (Dictionary_kid.TryGetValue((Direction_Extended)voxelmap.GetExtend<DirAttribute_Extend>().GetData(), out block)) return block;
                    else return Dictionary_kid.First().Value;
                default:return CubeMesh.CubeMeshes.First().Value;
            }
        }
        Debug.Log("??");
        return null;
    }
    public static Vector3[] RotateVerts(Vector3[] verts, Quaternion rotation)
    {
        // ... 旋转顶点的方法 ...
        Vector3[] rotatedVerts = new Vector3[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            rotatedVerts[i] = rotation * verts[i];
        }
        return rotatedVerts;
    }
    public static Vector3Int[] RotateVerts(Vector3Int[] verts, Quaternion rotation)
    {
        // ... 旋转顶点的方法 ...
        Vector3Int[] rotatedVerts = new Vector3Int[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 v = (rotation * verts[i]);
            rotatedVerts[i] = Vector3Int.RoundToInt(v);
        }
        return rotatedVerts;
    }
}