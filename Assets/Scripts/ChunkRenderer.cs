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

        // water can also exist
        // Each sub-mesh corresponds to a Material in a Renderer. A submesh consist of a list of triangles which refers to the vertices.
        mesh.subMeshCount = 2;
        
        // becouse we also include water when setting its triangles we need to refer to its vertices. Thus we concatenate it to the main vertices
        mesh.vertices = meshData.vertices.Concat(meshData.waterMesh.vertices).ToArray();

        // define the shapes of our via triangles (arrays of indicees of our verts). second arg is like sub mesh layer so which mesh we're refering to
        mesh.SetTriangles(meshData.triangles.ToArray(), 0);
        // this is for subMesh 1 (water) we get the vertex of the water mesh and offset them by meshData.verts.Count becouse they are concatenated
        // to the normal vertices in our mesh
        mesh.SetTriangles(meshData.waterMesh.triangles.Select(vertexInd => vertexInd + meshData.vertices.Count).ToArray(), 1);

        // concatenate for water again
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

    public void UpdateChunk(MeshData meshData) {
        RenderMesh(meshData);
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
}