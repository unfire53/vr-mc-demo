using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mesh
{
    
    public static class StepMesh
    {
        public static readonly Vector3[] Verts = new Vector3[]
     {
     //z = -0.5
        new Vector3(-0.5f,-0.5f,-0.5f) ,
        new Vector3(0.5f,-0.5f,-0.5f) ,
        new Vector3(0.5f,0,-0.5f) ,
        new Vector3(-0.5f,0,-0.5f) ,
    //z = 0
        new Vector3(-0.5f,0,0),
        new Vector3(0.5f,0,0),
        new Vector3(0.5f,0.5f,0),
        new Vector3(-0.5f,0.5f,0),
    //z = 0.5
        new Vector3(-0.5f,-0.5f,0.5f) ,
        new Vector3(0.5f,-0.5f,0.5f) ,
        new Vector3(0.5f,0,0.5f) ,
        new Vector3(-0.5f,0,0.5f) ,
        new Vector3(0.5f,0.5f,0.5f) ,
        new Vector3(-0.5f,0.5f,0.5f),
    //z = 0辅助顶点,辅助旋转
        new Vector3(-0.5f,-0.5f,0),
        new Vector3(0.5f,-0.5f,0)
     };
        public static readonly int[,] Tris = new int[,]
        {
            {0,3,1,2},//back up
            {4,7,5,6 },//back down
            {9,12,8,13},//front
            {3,4,2,5},//top up
            {7,13,6,12 },//top down
            {1,9,0,8},//bottom
            {11,13,4,7},//left up
            {8,11,0,3 },//left down
            {5,6,10,12},//right up
            {1,2,9,10}//right down
        };
        //贴图
        public static readonly Vector2[] UVs = new Vector2[4]
        {
            new Vector2(0.0f,0.0f),
            new Vector2(0.0f,1.0f),
            new Vector2(1.0f,0.0f),
            new Vector2(1.0f,1.0f)
        };
        public static readonly Vector3Int[] faceOrigin = new Vector3Int[]
        {
            new Vector3Int(0,0,-1),
            new Vector3Int(0,0,-1),
            new Vector3Int(0,0,1),
            new Vector3Int(0,1,0),
            new Vector3Int(0,1,0),
            new Vector3Int(0,-1,0),
            new Vector3Int(-1,0,0),
            new Vector3Int(-1,0,0),
            new Vector3Int(1,0,0),
            new Vector3Int(1,0,0)
        };
        public static readonly Vector3Int[] faceCheck = new Vector3Int[]
        {
            new Vector3Int(0,0,-1),
            new Vector3Int(0,0,-1),
            new Vector3Int(0,0,1),
            new Vector3Int(0,1,0),
            new Vector3Int(0,1,0),
            new Vector3Int(0,-1,0),
            new Vector3Int(-1,0,0),
            new Vector3Int(-1,0,0),
            new Vector3Int(1,0,0),
            new Vector3Int(1,0,0)
        };
        public static readonly Dictionary<Direction_Extended, BlockMesh> StepMeshes = new Dictionary<Direction_Extended, BlockMesh>()
    {
        { Direction_Extended.Step_Up_Front, new BlockMesh()
        {
            Verts_N = Verts,
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = faceCheck
        }
        },
        // 其他方向的网格数据...
        { Direction_Extended.Step_Up_Back, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(0, 180, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(0, 180, 0))
        }
        },
        { Direction_Extended.Step_Up_Left, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(0, 270, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(0, 270, 0))
        }
        },
        { Direction_Extended.Step_Up_Right, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(0, 90, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(0, 90, 0))
        }
        },
        { Direction_Extended.Step_Down_Front, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(270, 0, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(270, 0, 0))
        }
        },
        { Direction_Extended.Step_Down_Back, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(270, 180, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(270, 180, 0))
        }
        },
        { Direction_Extended.Step_Down_Left, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(270, 270, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(270, 270, 0))
        }
        },
        { Direction_Extended.Step_Down_Right, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(270, 90, 0)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(270, 90, 0))
        }
        },
        { Direction_Extended.Step_Mid_Front, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(0, 0, 90)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(0, 0, 90))
        }
        },
        { Direction_Extended.Step_Mid_Back, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(0, 180, 90)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(0, 180, 90))
        }
        },
        { Direction_Extended.Step_Mid_Left, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(0, 270, 90)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(0, 270, 90))
        }
        },
        { Direction_Extended.Step_Mid_Right, new BlockMesh()
        {
            Verts_N = MeshDataLibrary.RotateVerts(Verts, Quaternion.Euler(0, 90, 90)),
            Tris_N = Tris,
            UVs_N = UVs,
            faceOrigin_N = faceOrigin,
            faceCheck_N = MeshDataLibrary.RotateVerts(faceCheck, Quaternion.Euler(0, 90, 90))
        }
        }
    };
    }
}
