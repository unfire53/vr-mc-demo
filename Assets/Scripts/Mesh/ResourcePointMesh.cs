using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Mesh
{
    public class ResourcePointMesh
    {
        public static readonly Vector3[] Verts = new Vector3[]
        {
        new Vector3(-0.5f,0.6f,-0.5f) ,
        new Vector3(-0.5f,0.6f,0.5f) ,
        new Vector3(0.5f,0.6f,-0.5f) ,
        new Vector3(0.5f,0.6f,0.5f)
        };
        public static readonly int[] Tris = new int[]
        {
        0,1,2,3
        };
        public static readonly Vector2[] UVs = new Vector2[4]
        {
        new Vector2(0.0f,0.0f),
        new Vector2(0.0f,1.0f),
        new Vector2(1.0f,0.0f),
        new Vector2(1.0f,1.0f)
        };
    }
}
