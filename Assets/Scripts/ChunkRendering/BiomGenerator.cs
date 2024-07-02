using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomGenerator : MonoBehaviour
{

    [SerializeField] private int waterThreshold;
    [SerializeField] private float noiseScale = .03f;
    public ChunkData ProcessChunkColumn(ChunkData data, int x, int z, Vector2Int seed) {
        int groundPos = GetGroundPos(data, x + seed.y, z + seed.x);

        for(int y = 0; y < data.chunkHeight; y++) {
            BlockType voxelType = GetBlockType(y, groundPos); 
            Chunk.SetBlockInChunk(data, new(x, y, z), voxelType);
        }

        return data;
    }

    private int GetGroundPos(ChunkData data, int x, int z) {
        float noiseValue = Mathf.PerlinNoise((data.worldPos.x + x) * noiseScale, (data.worldPos.z + z) * noiseScale);
        return Mathf.RoundToInt(noiseValue * data.chunkHeight);
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
