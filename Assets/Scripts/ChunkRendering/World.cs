using System;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private int mapSizeInChunks = 6;
    [SerializeField] private int chunkSize = 16, chunkHeight = 100;
    [SerializeField] private int waterThreshold;
    [SerializeField] private float noiseScale = .03f;
    [SerializeField] private GameObject chunkPrefab;

    private Dictionary<Vector3Int, ChunkData> chunkDataDict = new Dictionary<Vector3Int, ChunkData>();
    private Dictionary<Vector3Int, ChunkRenderer> chunkDict = new Dictionary<Vector3Int, ChunkRenderer>();

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
        foreach (ChunkRenderer chunkRenderer in chunkDict.Values) {
            Destroy(chunkRenderer.gameObject);
        }
        chunkDict.Clear();
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
        MeshData meshData = Chunk.GetChunkMeshData(chunkData);
        GameObject chunkObj = Instantiate(chunkPrefab, chunkData.worldPos, Quaternion.identity);
        ChunkRenderer chunkRenderer = chunkObj.GetComponent<ChunkRenderer>();
        chunkRenderer.SetChunkData(chunkData);
        chunkRenderer.UpdateChunk(meshData);
    }

    public BlockType GetBlockFromChunkCoords(ChunkData chunkData, Vector3Int pos) {
        Vector3Int localChunkPos = Chunk.ChunkPositionFromBlockCoords(this, pos);
        ChunkData containerChunk = null;

        if(!chunkDataDict.TryGetValue(localChunkPos, out containerChunk)) {
            return BlockType.Nothing;
        } else {
            Vector3Int blockInChunkCoords = Chunk.WorldPositionToChunkPosition(containerChunk, pos);
            return Chunk.GetBlockFromLocalChunkCoordinates(containerChunk, blockInChunkCoords);
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
        float noiseValue = Mathf.PerlinNoise((data.worldPos.x + x) * noiseScale, (data.worldPos.z + z) * noiseScale);
        return Mathf.RoundToInt(noiseValue * chunkHeight);
    }

    private BlockType GetBlockType(int y, int groundPos){
        BlockType voxelType = BlockType.Dirt;
        if(y > groundPos) {
            if (y < waterThreshold) {
                voxelType = BlockType.Water;
            } else {
                voxelType = BlockType.Air;
            }
        }
        else if (y == groundPos) {
            voxelType = BlockType.Grass_Dirt;
        }
        return voxelType;
    }
}