using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceLayerHandler : BaseBlockLayerHandler
{
    [SerializeField] private BlockType surfaceBlockType;

    protected override bool TryHandle(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeed) {
        if(pos.y == surfaceHeightNoise) {
            Chunk.SetBlockInChunk(chunkData, pos, surfaceBlockType);
            return true;
        }
        return false;
    }
}
