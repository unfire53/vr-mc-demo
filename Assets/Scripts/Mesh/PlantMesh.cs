using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// 特指小型的，一格方块的植物
/// </summary>
public static class PlantMesh
{
    public static readonly Vector3[] Verts = new Vector3[]
        {
        new Vector3(-0.5f,-0.5f,-0.5f) ,
        new Vector3(0.5f,-0.5f,-0.5f) ,
        new Vector3(0.5f,0.5f,-0.5f) ,
        new Vector3(-0.5f,0.5f,-0.5f) ,
        new Vector3(-0.5f,-0.5f,0.5f) ,
        new Vector3(0.5f,-0.5f,0.5f) ,
        new Vector3(0.5f,0.5f,0.5f) ,
        new Vector3(-0.5f,0.5f,0.5f)
        };
    //每个面的三角形
    public static readonly int[,] Tris = new int[4, 4]
    {
        {5,6,0,3 },
        {1,2,4,7 },
        {0,3,5,6 },
        {4,7,1,2 }
    };
    //贴图
    public static readonly Vector2[] UVs = new Vector2[4]
    {
        new Vector2(0.0f,0.0f),
        new Vector2(0.0f,1.0f),
        new Vector2(1.0f,0.0f),
        new Vector2(1.0f,1.0f)
    };
    public static readonly Vector3Int[] faceOrigin = new Vector3Int[4]
    {
        new Vector3Int(0,0,-1),
        new Vector3Int(0,0,1),
        new Vector3Int(-1,0,0),
        new Vector3Int(1,0,0)
    };
    public static readonly Vector3Int[] faceCheck = new Vector3Int[4]
    {
        new Vector3Int(0,0,-1),
        new Vector3Int(0,0,1),
        new Vector3Int(-1,0,0),
        new Vector3Int(1,0,0)
    };
    public static readonly Dictionary<Direction_Extended, BlockMesh> CubeMeshes = new Dictionary<Direction_Extended, BlockMesh>()
        {
            {Direction_Extended.Up,new BlockMesh()
            {
                Verts_N = Verts,
                Tris_N = Tris,
                UVs_N = UVs,
                faceOrigin_N = faceOrigin,
                faceCheck_N = faceCheck
            }
            }
        };
}
