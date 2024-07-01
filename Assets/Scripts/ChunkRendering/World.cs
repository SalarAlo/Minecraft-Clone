using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private int mapSizeInChunks = 6;
    [SerializeField] private int chunkSize = 16, chunkHeight = 100;
    [SerializeField] private int waterThreshold;
    [SerializeField] private float noiseScale = .03f;
    [SerializeField] private ChunkRenderer chunkRendererPrefab;

    private Dictionary<Vector3Int, ChunkData> chunkDataDict = new Dictionary<Vector3Int, ChunkData>();
    private Dictionary<Vector3Int, ChunkRenderer> chunkRendererDict = new Dictionary<Vector3Int, ChunkRenderer>();

    [SerializeField] private int seed;

    public int GetChunkSize() => chunkSize;
    public int GetChunkHeight() => chunkHeight;


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
                ChunkData data = new(chunkSize, chunkHeight, this, new Vector3Int(x * chunkSize, 0, z*chunkSize));
                PopulateChunkWithBlocks(data);
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

    private void PopulateChunkWithBlocks(ChunkData data) {
        for(int x = 0; x < data.chunkSize; x++) {
            for(int z = 0; z < data.chunkSize; z++) {
                int groundPos = GetGroundPos(data, x, z);

                for(int y = 0; y < chunkHeight; y++) {
                    BlockType voxelType = GetBlockType(y, groundPos); 
                    Chunk.SetBlockInChunk(data, new(x, y, z), voxelType);
                }
            }
        }
    }

    private int GetGroundPos(ChunkData data, int x, int z) {
        float noiseValue = Mathf.PerlinNoise((data.worldPos.x + x + seed) * noiseScale, (data.worldPos.z + z + seed) * noiseScale);
        return Mathf.RoundToInt(noiseValue * chunkHeight);
    }

    private BlockType GetBlockType(int y, int groundPos){
        // wont change if y is less then groundPos 
        BlockType voxelType = BlockType.Dirt;
        if(y > groundPos) {
            if (y < waterThreshold) {
                // y is bigger then ground but less then water so under water
                voxelType = BlockType.Water;
            } else {
                // y is bigger then groundPos and bigger then water so above surface
                voxelType = BlockType.Air;
            }
        } 
        else if (y == groundPos) {
            // y is equivalent to ground 
            voxelType = BlockType.Grass_Dirt;
        }
        return voxelType;
    }
}