using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundLayerHandler : SingleBlockLayerHandler
{
    public BlockType undergroundBlockType;

    public UndergroundLayerHandler(BlockType blockType) : base(blockType) {}

    public override bool ShouldPlace(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeed) => pos.y < surfaceHeightNoise;
}
