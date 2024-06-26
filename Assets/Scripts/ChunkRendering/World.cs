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

    /// <summary>
    /// Clears and deletes the existing chunkData and destroys the chunkRenderes
    /// </summary>
    private void ClearExistingChunkData() {
        chunkDataDict.Clear();
        foreach (ChunkRenderer chunkRenderer in chunkRendererDict.Values) {
            Destroy(chunkRenderer.gameObject);
        }
        chunkRendererDict.Clear();
    }

    /// <summary>
    /// creates chunks and fills its data with the needed blocks. chunkDataDictionary is also filled
    /// </summary>
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
        chunkRendererDict.Add(chunkData.worldPos, chunkRenderer);
        chunkRenderer.SetChunkData(chunkData);
        chunkRenderer.UpdateChunk(meshData);
    }

    /// <summary>
    /// return the type of block by retrieving its value based on its worldBlockPosition
    /// </summary>
    /// <param name="worldBlockPos"></param>
    /// <returns></returns>
    public BlockType GetBlockTypeFromWorldBlockPos(Vector3Int worldBlockPos) {
        Vector3Int worldChunkPos = Chunk.ConvertWorldBlockCoordToWorldChunkCoord(this, worldBlockPos);
        ChunkData containerChunk = null;

        if(!chunkDataDict.TryGetValue(worldChunkPos, out containerChunk)) {
            return BlockType.Nothing;
        } else {
            Vector3Int blockInChunkCoords = Chunk.WorldBlockCoordToLocalBlockCoord(containerChunk, worldBlockPos);
            return Chunk.GetBlock(containerChunk, blockInChunkCoords);
        }
    }

    /// <summary>
    /// takes in a chunk and populates each of its blocks with a blockType based on a procedurally generated groundPos foreach cell in the chunk
    /// </summary>
    /// <param name="data"></param>
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

    /// <summary>
    /// returns a ground Position based on the x and z coordinates. this ground pos is generated using perlin noise 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private int GetGroundPos(ChunkData data, int x, int z) {
        float noiseValue = Mathf.PerlinNoise((data.worldPos.x + x + seed) * noiseScale, (data.worldPos.z + z + seed) * noiseScale);
        return Mathf.RoundToInt(noiseValue * chunkHeight);
    }

    /// <summary>
    /// computes a blocktype based on a ground Pos and a y pos
    /// </summary>
    /// <param name="y"></param>
    /// <param name="groundPos"></param>
    /// <returns></returns>
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