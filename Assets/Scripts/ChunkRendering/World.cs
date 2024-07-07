using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private int waterLevel;
    [SerializeField] private int mapSizeInChunks = 6;
    [SerializeField] private int chunkSize = 16, chunkHeight = 100;
    [SerializeField] private ChunkRenderer chunkRendererPrefab;
    [SerializeField] private TerrainGenerator terrainGenerator;

    private Dictionary<Vector3Int, ChunkData> chunkDataDict = new Dictionary<Vector3Int, ChunkData>();
    private Dictionary<Vector3Int, ChunkRenderer> chunkRendererDict = new Dictionary<Vector3Int, ChunkRenderer>();

    [SerializeField] private Vector2Int seed;
    internal Vector3Int wa;

    public int GetChunkSize() => chunkSize;
    public int GetChunkHeight() => chunkHeight;
    public int GetWaterLevel() => waterLevel;

    public void GenerateWorld() {
        ClearExistingChunkData();
        CreateChunks();

        foreach(ChunkData data in chunkDataDict.Values) {
            CreateChunkRenderer(data);
        }
    }

    private void ClearExistingChunkData() {
        chunkDataDict.Clear();
        foreach (ChunkRenderer chunkRenderer in chunkRendererDict.Values) {
            Destroy(chunkRenderer.gameObject);
        }
        chunkRendererDict.Clear();
    }

    private void CreateChunks() {
        for(int x = 0; x < mapSizeInChunks; x++) {
            for(int z = 0; z < mapSizeInChunks; z++) {
                ChunkData startData = new(chunkSize, chunkHeight, this, new Vector3Int(x * chunkSize, 0, z*chunkSize));
                var data = terrainGenerator.PopulateChunkData(startData, seed);
                chunkDataDict.Add(data.worldPos, data);
            }
        }
    }

    private void CreateChunkRenderer(ChunkData chunkData) {
        var chunkRenderer = Instantiate(chunkRendererPrefab, chunkData.worldPos, Quaternion.identity);
        chunkRenderer.SetAndRenderChunk(chunkData);
        chunkRendererDict.Add(chunkData.worldPos, chunkRenderer);
    }

    public BlockType GetBlockTypeFromBlockPos(Vector3Int worldBlockPos) {
        Vector3Int worldChunkPos = Chunk.ConvertWorldBlockCoordToWorldChunkCoord(this, worldBlockPos);

        if(!chunkDataDict.TryGetValue(worldChunkPos, out ChunkData containerChunk)) {
            return BlockType.Nothing;
        } else {
            Vector3Int blockInChunkCoords = Chunk.WorldBlockCoordToLocalBlockCoord(containerChunk, worldBlockPos);
            return Chunk.GetBlock(containerChunk, blockInChunkCoords);
        }
    }
}