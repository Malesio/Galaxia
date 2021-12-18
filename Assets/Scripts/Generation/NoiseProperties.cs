using UnityEngine;

[System.Serializable]
public class NoiseProperties 
{
    public enum NoiseFilterType
    {
        Basic,
        Steep
    }

    public NoiseFilterType FilterType = NoiseFilterType.Basic;

    [Min(0)]
    public float Strength = 1.0f;
    [Min(0.01f)]
    public float Roughness = 1.0f;
    public Vector3 Offset = new Vector3();
    [Range(1, 20)]
    public int Octaves = 1;
    [Min(1)]
    public float Lacunarity = 2.0f;
    [Range(0, 1)]
    public float Persistence = 0.5f;
    [Min(0)]
    public float Threshold = 1.0f;

    [Range(0, 1)]
    public float WeightFactor = 0.5f;

    [Range(0, 5)]
    public int Sharpness = 1;
}
