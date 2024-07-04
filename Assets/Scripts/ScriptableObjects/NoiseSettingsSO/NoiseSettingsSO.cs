using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/new NoiseSettings")]
public class NoiseSettingsSO : ScriptableObject {
    public float noiseZoom;
    public int octaves;
    public Vector2Int offset;
    public Vector2Int seed;
    public float redistributionModifier;
    public float exponent;
}
