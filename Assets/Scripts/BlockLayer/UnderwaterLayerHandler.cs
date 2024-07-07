using UnityEngine;

public class UnderwaterLayerHandler : SingleBlockLayerHandler
{
    public UnderwaterLayerHandler(BlockType blockType) : base(blockType) { }

    public override bool ShouldPlace(ChunkData chunkData, Vector3Int pos, int groundPos, Vector2Int mapSeed) {
        return pos.y <= chunkData.worldRef.GetWaterLevel() && pos.y <= groundPos; 
    }
}
