using UnityEngine;

public class WaterLayerHandler : BlockLayerHandler
{
    public int waterLevel = 1;
    protected override bool TryHandle(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeed) {
        if(pos.y > surfaceHeightNoise && pos.y <= waterLevel){
            Chunk.SetBlockInChunk(chunkData, pos, BlockType.Water);
            if (pos.y == surfaceHeightNoise + 1) {
                pos.y = surfaceHeightNoise;
                Chunk.SetBlockInChunk(chunkData, pos, BlockType.Sand);
            }
            return true;
        }

        return false; 
    }
}
