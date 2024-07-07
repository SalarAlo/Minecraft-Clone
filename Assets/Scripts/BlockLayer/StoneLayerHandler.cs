using UnityEngine;

public class StoneLayerHandler : SingleBlockLayerHandler
{
    private float stoneThreshold = .5f;
    private NoiseSettingsSO stoneNoiseSettings;

    public StoneLayerHandler(NoiseSettingsSO stoneNoiseSettings, float stoneThreshold) : base(BlockType.Stone) {
        this.stoneThreshold = stoneThreshold;
        this.stoneNoiseSettings = stoneNoiseSettings;
    }

    public override bool ShouldPlace(ChunkData chunkData, Vector3Int pos, int groundPos, Vector2Int mapSeed) {
        stoneNoiseSettings.seed = mapSeed;
        float stoneNoise = SelfNoise.OctavePerlinNoise(chunkData.worldPos.x + pos.x, chunkData.worldPos.z + pos.z, stoneNoiseSettings);
        return stoneNoise > stoneThreshold && pos.y <= groundPos;
    }
}
