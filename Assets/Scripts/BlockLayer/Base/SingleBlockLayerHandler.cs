using UnityEngine;

public abstract class SingleBlockLayerHandler 
{
    protected BlockType blockType;

    public SingleBlockLayerHandler(BlockType blockType) {
        this.blockType = blockType;
    }

    public BlockType GetBlockType() => blockType;


    public abstract bool ShouldPlace(ChunkData chunkData, Vector3Int pos, int groundPos, Vector2Int mapSeed);
}
