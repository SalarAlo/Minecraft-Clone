using UnityEngine;

public static class SelfNoise 
{
    public static float RemapValue(float value, float initialMin, float initialMax, float outputMin, float outputMax)
    {
        return outputMin + (value - initialMin) * (outputMax - outputMin) / (initialMax - initialMin);
    }
    
    public static float RemapValue01(float value, float outputMin, float outputMax) {
        return outputMin + value * (outputMax - outputMin);
    }

    public static int RemapValue01ToInt(float value, float outMin, float outMax) {
        return (int)RemapValue01(value, outMin, outMax);
    }

    public static float Redistribution(float noise, NoiseSettingsSO noiseSettingsSO) => 
        Mathf.Pow(noise * noiseSettingsSO.redistributionModifier, noiseSettingsSO.exponent);
    
    public static float OctavePerlinNoise(float x, float z, NoiseSettingsSO settings){
        x *= settings.noiseZoom;
        z *= settings.noiseZoom;

        float noiseSum = 0;
        float frequency = 1;
        float totalAmplitude = 0;
        float amplitude = 1;
        
        for (int i = 0; i < settings.octaves; i++) {
            float perlinValue = Mathf.PerlinNoise(
                (settings.offset.x + settings.seed.x + x) * frequency, 
                (settings.offset.y + settings.seed.y + z) * frequency
            );

            noiseSum += perlinValue * amplitude;

            totalAmplitude += amplitude;
            amplitude *= settings.persistance;
            frequency *= settings.lacunarity;
        }

        return noiseSum / totalAmplitude;
    }
}