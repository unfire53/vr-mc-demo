using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mesh
{
    
    public static class SlabMesh
    {
        public static readonly Vector3[] Verts = new Vector3[8]
         {
        new Vector3(-0.5f,-0.5f,-0.5f) ,
        new Vector3(0.5f,-0.5f,-0.5f) ,
        new Vector3(0.5f,0,-0.5f) ,
        new Vector3(-0.5f,0,-0.5f) ,
        new Vector3(-0.5f,-0.5f,0.5f) ,
        new Vector3(0.5f,-0.5f,0.5f) ,
        new Vector3(0.5f,0,0.5f) ,
        new Vector3(-0.5f,0,0.5f)
         };
        //每个面的三角形
        public static readonly int[,] Tris = new int[6, 4]
        {
        {0,3,1,2},//back
        {5,6,4,7},//front
        {3,7,2,6},//top
        {1,5,0,4},//bottom
        {4,7,0,3},//left
        {1,2,5,6}//right
        };
        //贴图
        public static readonly Vector2[] UVs = new Vector2[4]
        {
        new Vector2(0.0f,0.0f),
        new Vector2(0.0f,1.0f),
        new Vector2(1.0f,0.0f),
        new Vector2(1.0f,1.0f)
        };
        public static readonly Vector3Int[] faceOrigin = new Vector3Int[6]
        {
        new Vector3Int(0,0,-1),
        new Vector3Int(0,0,1),
        new Vector3Int(0,1,0),
        new Vector3Int(0,-1,0),
        new Vector3Int(-1,0,0),
        new Vector3Int(1,0,0)
        };
        public static readonly Vector3Int[] faceCheck = new Vector3Int[6]
        {
        new Vector3Int(0,0,-1),
        new Vector3Int(0,0,1),
        new Vector3Int(0,1,0),
        new Vector3Int(0,-1,0),
        new Vector3Int(-1,0,0),
        new Vector3Int(1,0,0)
        };
        public static readonly Dictionary<Direction_Extended, BlockMesh> SlabMeshes = new Dictionary<Direction_Extended, BlockMesh>()
    {
        { Direction_Extended.Slab_Up, new BlockMesh()
        {
            Verts_N = Verts,
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = faceCheck
        }
        },
        // 其他方向的网格数据...
        { Direction_Extended.Slab_Down, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(180, 0, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(180, 0, 0))
        }
        },
        { Direction_Extended.Slab_Left, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(90, 270, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(90, 270, 0))
        }
        },
        { Direction_Extended.Slab_Right, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(90, 90, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(90, 90, 0))
        }
        },
        { Direction_Extended.Slab_Front, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(90, 0, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(90, 0, 0))
        }
        },
        { Direction_Extended.Slab_Back, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(90, 180, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(90, 180, 0))
        }
        }
    };
    }
}
