using System.Collections.Generic;
using UnityEngine;

public class GrasslandBiomHandler : BiomHandler
{
    [SerializeField] private float stoneThreshold;
    [SerializeField] private NoiseSettingsSO stoneNoiseSettings;

    protected override List<SingleBlockLayerHandler> GetBlockLayerHandlers() {
        return new List<SingleBlockLayerHandler> () {
            new UnderwaterLayerHandler(BlockType.Sand),
            new SurfaceLayerHandler(BlockType.Grass_Dirt),
            new UndergroundLayerHandler(BlockType.Dirt),
            new WaterLayerHandler(),
            new AirLayerHandler(),
        };
    }

    protected override List<SingleBlockLayerHandler> GetAdditionalBlockLayerHandlers() {
        return new List<SingleBlockLayerHandler> () {
            new StoneLayerHandler(stoneNoiseSettings, stoneThreshold),
        };
    }
}
