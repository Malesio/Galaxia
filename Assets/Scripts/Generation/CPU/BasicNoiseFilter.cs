using UnityEngine;

public class BasicNoiseFilter : INoiseFilter
{
    private readonly NoiseProperties properties;

    public BasicNoiseFilter(NoiseProperties props)
    {
        properties = props;
    }

    public float Evaluate(INoiseSource noiseSource, Vector3 point)
    {
        float totalValue = 0.0f;
        float harmonicFrequency = properties.Roughness;
        float harmonicAmplitude = 1;

        for (int i = 0; i < properties.Octaves; i++)
        {
            float harmonicValue = noiseSource.Get(harmonicFrequency * point + properties.Offset);
            totalValue += harmonicValue * harmonicAmplitude;

            harmonicFrequency *= properties.Lacunarity;
            harmonicAmplitude *= properties.Persistence;
        }

        return Mathf.Max(0, totalValue - properties.Threshold) * properties.Strength;
    }
}
