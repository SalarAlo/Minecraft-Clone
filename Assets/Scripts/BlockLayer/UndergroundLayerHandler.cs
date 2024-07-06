using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundLayerHandler : BaseBlockLayerHandler
{
    public BlockType undergroundBlockType;

    protected override bool TryHandle(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeed) {
        if(pos.y < surfaceHeightNoise) {
            Chunk.SetBlockInChunk(chunkData, pos, undergroundBlockType);
            return true;
        }
        return false;
    }
}
