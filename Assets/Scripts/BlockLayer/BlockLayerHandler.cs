using System;
using UnityEngine;

public abstract class BlockLayerHandler : MonoBehaviour
{
    [SerializeField] private BlockLayerHandler next;

    public bool Handle(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeed) {
        if(TryHandle(chunkData, pos, surfaceHeightNoise, mapSeed)) {
            return true;
        }
        if(next != null) 
            return next.Handle(chunkData, pos, surfaceHeightNoise, mapSeed);
        return false;
    }

    protected abstract bool TryHandle(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeed);
}
