using UnityEngine;

public class WaterLayerHandler : SingleBlockLayerHandler
{
    public int waterLevel = 8;
    public WaterLayerHandler(BlockType blockType) : base(blockType) {}

    public override bool ShouldPlace(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeed) {
        return pos.y > surfaceHeightNoise && pos.y <= waterLevel;
    }
}
