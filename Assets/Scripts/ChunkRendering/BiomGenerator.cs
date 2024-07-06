using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomGenerator : MonoBehaviour
{
    [SerializeField] private int waterThreshold;
    [SerializeField] private NoiseSettingsSO noiseSettingsSO;
    [SerializeField] private BiomHandler handler;

    public ChunkData ProcessChunkColumn(ChunkData data, int x, int z, Vector2Int seed) {
        noiseSettingsSO.seed = seed;

        int groundPos = GetGroundPos(data.chunkHeight, x + data.worldPos.x, z + data.worldPos.z);

        for(int y = 0; y < data.chunkHeight; y++) {
            handler.HandleSingleBlock(data, new Vector3Int(x, y, z), groundPos, seed);
        }

        // foreach(var layer in additionalLayerHandlers) {
        // layer.Handle(data, new(x, data.worldPos.y, z), groundPos, seed);
        // }

        return data;
    }

    private int GetGroundPos(int chunkHeight, int x, int z) {
        float height = SelfNoise.OctavePerlinNoise(x, z, noiseSettingsSO);
        height = SelfNoise.Redistribution(height, noiseSettingsSO); 

        return Mathf.FloorToInt(height * chunkHeight);
    }
}
