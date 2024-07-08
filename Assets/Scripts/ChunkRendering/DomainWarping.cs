using System;
using UnityEngine;

public class DomainWarping : MonoBehaviour
{
    [SerializeField] private NoiseSettingsSO noiseDomainX, noiseDomainY;
    [SerializeField] private int amplitudeX = 20, amplitudeY = 20;

    public float GenerateDomainNoise(int x, int z, NoiseSettingsSO defaultNoiseSettings) {
        Vector2 domainOffset = GenerateDomainOffset(x, z);
        return SelfNoise.OctavePerlinNoise(x + domainOffset.x, z + domainOffset.y, defaultNoiseSettings);
    }

    private Vector2 GenerateDomainOffset(int x, int z)
    {
        float noiseX = SelfNoise.OctavePerlinNoise(x, z, noiseDomainX) * amplitudeX;
        float noiseY = SelfNoise.OctavePerlinNoise(x, z, noiseDomainY) * amplitudeY;
        return new(noiseX, noiseY);
    }

    public Vector2Int GenerateDomainOffsetInt(int x, int z){
        return Vector2Int.RoundToInt(GenerateDomainOffset(x, z));
    }
}
