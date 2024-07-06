using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BiomHandler : MonoBehaviour
{
    public void HandleSingleBlock(ChunkData data, Vector3Int position, int groundPos, Vector2Int seed) {
        foreach(var blockLayer in GetBlockLayerHandlers()) {
            if (blockLayer.ShouldPlace(data, position, groundPos, seed)){
                BlockType blockType = blockLayer.GetBlockType();
                Chunk.SetBlockInChunk(data, position, blockType);
                break;
            }
        }
    }

    protected abstract List<SingleBlockLayerHandler> GetBlockLayerHandlers();
    protected virtual List<SingleBlockLayerHandler> GetAdditionalBlockLayerHandlers() => null;
}
