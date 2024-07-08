using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomGenerator : MonoBehaviour
{
    [SerializeField] private NoiseSettingsSO noiseSettingsSO;
    [SerializeField] private BiomHandler handler;
    [SerializeField] private bool useDomainWarping;
    private DomainWarping domainWarping;

    private void Awake() {
        domainWarping = GetComponent<DomainWarping>();
    }

    public ChunkData ProcessChunkColumn(ChunkData data, int x, int z, Vector2Int seed) {
        noiseSettingsSO.seed = seed;

        int groundPos = GetGroundPos(data.chunkHeight, x + data.worldPos.x, z + data.worldPos.z);

        for(int y = 0; y < data.chunkHeight; y++) {
            handler.HandleSingleBlock(data, new Vector3Int(x, y, z), groundPos, seed);
        }

        return data;
    }

    private int GetGroundPos(int chunkHeight, int x, int z) {
        float height = useDomainWarping ? 
            domainWarping.GenerateDomainNoise(x, z, noiseSettingsSO) : 
            SelfNoise.OctavePerlinNoise(x, z, noiseSettingsSO);
        
        height = SelfNoise.Redistribution(height, noiseSettingsSO); 

        return Mathf.FloorToInt(height * chunkHeight);
    }
}
