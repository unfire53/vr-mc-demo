using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictVoxel
{
    public GameObject predictVoxelObject;
    public MeshFilter filter;
    public MeshRenderer renderer;

    public PredictVoxel(List<Vector3> _vertices,List<int> _triangles,World world)
    {
        predictVoxelObject = new GameObject();
        filter = predictVoxelObject.AddComponent<MeshFilter>();
        renderer = predictVoxelObject.AddComponent<MeshRenderer>();
        Set(_vertices, _triangles);
        renderer.material = world.ResourceMaterial;
    }
    public void Set(List<Vector3> _vertices, List<int> _triangles)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        filter.mesh = mesh;
    }
}

