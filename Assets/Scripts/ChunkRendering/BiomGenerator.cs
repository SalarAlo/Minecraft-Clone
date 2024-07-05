using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomGenerator : MonoBehaviour
{

    [SerializeField] private int waterThreshold;
    [SerializeField] private NoiseSettingsSO noiseSettingsSO;
    [SerializeField] private BlockLayerHandler startLayer;

    public ChunkData ProcessChunkColumn(ChunkData data, int x, int z, Vector2Int seed) {
        noiseSettingsSO.seed = seed;
        int groundPos = GetGroundPos(data.chunkHeight, x + data.worldPos.x, z + data.worldPos.z);

        for(int y = 0; y < data.chunkHeight; y++) {
            startLayer.Handle(data, new(x, y, z), groundPos, seed);
        }

        return data;
    }

    private int GetGroundPos(int chunkHeight, int x, int z) {
        float height = SelfNoise.OctavePerlinNoise(x, z, noiseSettingsSO);
        height = SelfNoise.Redistribution(height, noiseSettingsSO); 

        return Mathf.FloorToInt(height * chunkHeight);
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
        }else if(y <= waterThreshold) {
            voxelType = BlockType.Sand;
        }
        else if (y == groundPos) {
            // y is equivalent to ground 
            voxelType = BlockType.Grass_Dirt;
        } 
        return voxelType;
    }
}
