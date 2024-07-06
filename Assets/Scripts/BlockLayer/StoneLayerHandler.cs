using UnityEngine;

public class StoneLayerHandler : BaseBlockLayerHandler
{
    [Range(0, 1), SerializeField] public float stoneThreshold = .5f;
    [SerializeField] private NoiseSettingsSO stoneNoiseSettings;
    protected override bool TryHandle(ChunkData chunkData, Vector3Int pos, int groundPos, Vector2Int mapSeed) {
        stoneNoiseSettings.seed = mapSeed;
        float stoneNoise = SelfNoise.OctavePerlinNoise(chunkData.worldPos.x + pos.x, chunkData.worldPos.z + pos.z, stoneNoiseSettings);

        if (stoneNoise > stoneThreshold) {
            for(int i = chunkData.worldPos.y; i <= groundPos; i++) {
                Vector3Int position = new(pos.x, i, pos.z);
                Chunk.SetBlockInChunk(chunkData, position, BlockType.Stone);
            }
            return true;
        }

        return false; 
    }
}
