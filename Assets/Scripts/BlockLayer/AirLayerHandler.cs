using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLayerHandler : BaseBlockLayerHandler
{
    protected override bool TryHandle(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeed) {
        if(pos.y > surfaceHeightNoise) {
            Chunk.SetBlockInChunk(chunkData, pos, BlockType.Air);
            return true;
        }
        return false;
    }
}
