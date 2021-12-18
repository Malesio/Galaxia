using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlanetProperties : ScriptableObject
{
    [Range(8, 255)]
    public int Resolution = 64;

    public int Radius = 10;

    public Material BaseMaterial;

    public ComputeShader ComputeShader;

    public Gradient ColorGradient = new Gradient();

    public NoiseSourceType noiseSourceType = NoiseSourceType.Simplex;

    public bool FirstLayerIsMask = false;
    public List<NoiseLayer> NoiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool Enabled = true;
        public NoiseProperties Properties;
    }
}
