using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uv = new List<Vector2>();

    public List<Vector3> colliderVertices = new List<Vector3>();
    public List<int> colliderTriangles = new List<int>();
    
    public MeshData waterMesh;

    public MeshData(bool isMainMesh){
        if(isMainMesh)
            waterMesh = new MeshData(false);
    }

    public void AddVertex(Vector3 vertex, bool isColliderVertex) {
        vertices.Add(vertex);
        if(isColliderVertex) colliderVertices.Add(vertex);
    }

    public void AddQuadTriangles(bool isColliderQuad) {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        if(!isColliderQuad) return;

        colliderTriangles.Add(vertices.Count - 4);
        colliderTriangles.Add(vertices.Count - 3);
        colliderTriangles.Add(vertices.Count - 2);

        colliderTriangles.Add(vertices.Count - 4);
        colliderTriangles.Add(vertices.Count - 2);
        colliderTriangles.Add(vertices.Count - 1);
    }
}
