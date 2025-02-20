using Assets.Scripts.Mesh;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMesh
{
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector3> vertices_Collider;
    public List<int> triangles_Collider;
    public List<Vector2> uvs_Collider;
    public List<Vector2> uvs;
    public bool collider;
    public ChunkMesh_Water chunkMesh_Water;

    public ChunkMesh(bool _collider)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        vertices_Collider = new List<Vector3>();
        triangles_Collider = new List<int>();
        uvs_Collider = new List<Vector2>();
        collider = _collider;
        chunkMesh_Water = new ChunkMesh_Water();
    }

    public void Clear()
    {
        vertices_Collider.Clear(); triangles_Collider.Clear();
        vertices.Clear();
        triangles.Clear();
        uvs_Collider.Clear();
        uvs.Clear();
        if (chunkMesh_Water != null)
        {
            chunkMesh_Water.Clear();
        }
    }

    public void AddUvs(int textureID, bool has_Collider)
    {
        float y = textureID / WorldHelper.TextureAtlasSizeInBlocks;
        float x = textureID - (y * WorldHelper.TextureAtlasSizeInBlocks);

        y *= WorldHelper.NormalizedBlockTextureSize;
        x *= WorldHelper.NormalizedBlockTextureSize;

        y = 1f - y - WorldHelper.NormalizedBlockTextureSize;
        if (has_Collider)
        {
            uvs_Collider.Add(new Vector2(x, y));
            uvs_Collider.Add(new Vector2(x, y + WorldHelper.NormalizedBlockTextureSize));
            uvs_Collider.Add(new Vector2(x + WorldHelper.NormalizedBlockTextureSize, y));
            uvs_Collider.Add(new Vector2(x + WorldHelper.NormalizedBlockTextureSize, y + WorldHelper.NormalizedBlockTextureSize));
        }
        else
        {
            uvs.Add(new Vector2(x, y));
            uvs.Add(new Vector2(x, y + WorldHelper.NormalizedBlockTextureSize));
            uvs.Add(new Vector2(x + WorldHelper.NormalizedBlockTextureSize, y));
            uvs.Add(new Vector2(x + WorldHelper.NormalizedBlockTextureSize, y + WorldHelper.NormalizedBlockTextureSize));
        }
    }
    public void AddWaterUvs(int textureID)
    {
        chunkMesh_Water.AddUvs(textureID);
    }
    public void AddVertices(Vector3 offsetVec3, int face, BlockMesh newMesh, bool has_Collider)
    {
        if (has_Collider)
        {
            vertices_Collider.Add(newMesh.Verts_N[newMesh.Tris_N[face, 0]] + offsetVec3);
            vertices_Collider.Add(newMesh.Verts_N[newMesh.Tris_N[face, 1]] + offsetVec3);
            vertices_Collider.Add(newMesh.Verts_N[newMesh.Tris_N[face, 2]] + offsetVec3);
            vertices_Collider.Add(newMesh.Verts_N[newMesh.Tris_N[face, 3]] + offsetVec3);
        }
        else
        {
            vertices.Add(newMesh.Verts_N[newMesh.Tris_N[face, 0]] + offsetVec3);
            vertices.Add(newMesh.Verts_N[newMesh.Tris_N[face, 1]] + offsetVec3);
            vertices.Add(newMesh.Verts_N[newMesh.Tris_N[face, 2]] + offsetVec3);
            vertices.Add(newMesh.Verts_N[newMesh.Tris_N[face, 3]] + offsetVec3);
        }
    }
    public void AddWaterVertices(Vector3 offsetVec3)
    {
        chunkMesh_Water.AddVertices(offsetVec3);
    }
    public void AddWaterTriangles()
    {
        chunkMesh_Water.AddWaterTriangles();
    }
    public void AddTriangles(bool has_Collider)
    {
        if (has_Collider)
        {
            triangles_Collider.Add(vertices_Collider.Count - 4);
            triangles_Collider.Add(vertices_Collider.Count - 3);
            triangles_Collider.Add(vertices_Collider.Count - 2);

            triangles_Collider.Add(vertices_Collider.Count - 2);
            triangles_Collider.Add(vertices_Collider.Count - 3);
            triangles_Collider.Add(vertices_Collider.Count - 1);
        }
        else
        {
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 1);
        }
    }


}
public class ChunkMesh_Water
{
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;
    public ChunkMesh_Water()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
    }
    public void Clear()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
    }
    public void AddVertices(Vector3 offsetVec3)
    {
        vertices.Add(WaterMesh.Verts[WaterMesh.Tris[0]] + offsetVec3);
        vertices.Add(WaterMesh.Verts[WaterMesh.Tris[1]] + offsetVec3);
        vertices.Add(WaterMesh.Verts[WaterMesh.Tris[2]] + offsetVec3);
        vertices.Add(WaterMesh.Verts[WaterMesh.Tris[3]] + offsetVec3);
    }
    public void AddWaterTriangles()
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 1);
    }
    public void AddUvs(int textureID)
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
