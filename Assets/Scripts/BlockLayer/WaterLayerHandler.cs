using UnityEngine;

public class WaterLayerHandler : SingleBlockLayerHandler
{
    public WaterLayerHandler() : base(BlockType.Water) {}

    public override bool ShouldPlace(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeed) {
        return pos.y > surfaceHeightNoise && pos.y <= chunkData.worldRef.GetWaterLevel();
    }
}
