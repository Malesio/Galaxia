using UnityEngine;

public class SteepNoiseFilter : INoiseFilter
{
    private readonly NoiseProperties properties;

    public SteepNoiseFilter(NoiseProperties props)
    {
        properties = props;
    }

    private void Sharpen(ref float val)
    {
        for (int i = 0; i < properties.Sharpness; i++)
        {
            val *= val;
        }
    }

    public float Evaluate(INoiseSource noiseSource, Vector3 point)
    {
        float totalValue = 0.0f;
        float harmonicFrequency = properties.Roughness;
        float harmonicAmplitude = 1;
        float weight = 1.0f;

        for (int i = 0; i < properties.Octaves; i++)
        {
            float harmonicValue = 1 - Mathf.Abs(noiseSource.Get(harmonicFrequency * point + properties.Offset));
            Sharpen(ref harmonicValue);
            harmonicValue *= weight;
            weight *= Mathf.Clamp01(harmonicValue * properties.WeightFactor);

            totalValue += harmonicValue * harmonicAmplitude;

            harmonicFrequency *= properties.Lacunarity;
            harmonicAmplitude *= properties.Persistence;
        }

        return Mathf.Max(0, totalValue - properties.Threshold) * properties.Strength;
    }
}
