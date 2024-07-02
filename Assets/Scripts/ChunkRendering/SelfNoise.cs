using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Palmmedia.ReportGenerator.Core;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public static class SelfNoise 
{
    public static float RemapValue(float value, float initialMin, float initialMax, float outputMin, float outputMax)
    {
        return outputMin + (value - initialMin) * (outputMax - outputMin) / (initialMax - initialMin);
    }
    
    public static float RemapValue01(float value, float outputMin, float outputMax) {
        return outputMin + (value - 0) * (outputMax - outputMin) / (1 - 0);
    }

    public static int RemapValue01ToInt(float value, float outMin, float outMax) {
        return (int)RemapValue01(value, outMin, outMax);
    }

    public static float Redistribution(float noise, NoiseSettingsSO noiseSettingsSO) => 
        Mathf.Pow(noise * noiseSettingsSO.redistributionModifier, noiseSettingsSO.exponent);
    
    public static float OctavePerlin(float x, float z, NoiseSettingsSO settings){
        x *= settings.noiseZoom;
        z *= settings.noiseZoom;
        x += settings.noiseZoom;
        z += settings.noiseZoom;

        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float amplitudeSum = 0;  // Used for normalizing result to 0.0 - 1.0 range
        for (int i = 0; i < settings.octaves; i++)
        {
            total += Mathf.PerlinNoise((settings.offset.x + settings.worldOffset.x + x) * frequency, (settings.offset.y + settings.worldOffset.y + z) * frequency) * amplitude;

            amplitudeSum += amplitude;

            amplitude *= settings.persistance;
            frequency *= 2;
        }

        return total / amplitudeSum;
    }
}