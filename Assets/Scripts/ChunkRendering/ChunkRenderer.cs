using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ChunkRenderer : MonoBehaviour 
{
    [SerializeField] private bool showGizmo = false;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Mesh mesh;

    private ChunkData chunkData;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>(); 
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
    }

    private void RenderMesh(MeshData meshData) {
        mesh.Clear();

        mesh.subMeshCount = 2;

        mesh.vertices = meshData.vertices.Concat(meshData.waterMesh.vertices).ToArray();

        mesh.SetTriangles(meshData.triangles.ToArray(), 0);
        mesh.SetTriangles(meshData.waterMesh.triangles.Select(vertexInd => vertexInd + meshData.vertices.Count).ToArray(), 1);

        mesh.uv = meshData.uv.Concat(meshData.waterMesh.uv).ToArray();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = null;

        Mesh collisionMesh = new Mesh {
            vertices = meshData.colliderVertices.ToArray(),
            triangles = meshData.colliderTriangles.ToArray()
        };
        
        collisionMesh.RecalculateNormals();
        meshCollider.sharedMesh = collisionMesh;
    }

    public void UpdateChunk() {
        RenderMesh(Chunk.GetChunkMeshData(chunkData));
    }

    #if UNITY_EDITOR
        private void OnDrawGizmos() {
            if(!showGizmo) return;
            if(!Application.isPlaying || chunkData == null) return;
            Gizmos.color = Selection.activeGameObject == gameObject ? new(0, 1, 0, .4f) : new(1, 0, 1, .4f);
            Gizmos.DrawCube(
                transform.position + new Vector3(chunkData.chunkSize / 2, chunkData.chunkHeight / 2, chunkData.chunkSize / 2), 
                new(chunkData.chunkSize, chunkData.chunkHeight, chunkData.chunkSize)
            );
        }
    #endif
    public void SetChunkData(ChunkData chunkData) => this.chunkData = chunkData;
    public ChunkData GetChunkData() => chunkData;
    public void SetAndRenderChunk(ChunkData chunkData) {
        SetChunkData(chunkData);
        UpdateChunk();
    }
}