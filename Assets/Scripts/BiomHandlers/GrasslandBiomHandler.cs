using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrasslandBiomHandler : BiomHandler
{
    protected override List<SingleBlockLayerHandler> GetBlockLayerHandlers() {
        return new List<SingleBlockLayerHandler> () {
            new SurfaceLayerHandler(BlockType.Grass_Dirt),
            new UndergroundLayerHandler(BlockType.Dirt),
            new WaterLayerHandler(BlockType.Water),
            new AirLayerHandler(BlockType.Air),
        };
    }
}
