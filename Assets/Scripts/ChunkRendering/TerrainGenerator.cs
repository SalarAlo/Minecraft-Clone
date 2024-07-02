using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public BiomGenerator biomGenerator;

    public ChunkData PopulateChunkData(ChunkData data, Vector2Int seed) {
        for(int x = 0; x < data.chunkSize; x++) {
            for(int z = 0; z < data.chunkSize; z++) {

                data = biomGenerator.ProcessChunkColumn(data, x, z, seed);
            }
        }
        return data;
    }

}
