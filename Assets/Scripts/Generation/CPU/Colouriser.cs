using System.Collections.Generic;
using UnityEngine;

public class Colouriser
{
    private readonly PlanetProperties properties;

    private readonly List<INoiseFilter> filters;
    private readonly INoiseSource noiseSource;

    private float minValue;
    private float maxValue;

    public float MinColour
    {
        get => minValue;
    }

    public float MaxColour
    {
        get => maxValue;
    }

    public Colouriser(PlanetProperties properties)
    {
        this.properties = properties;

        noiseSource = NoiseSources.Create(properties.noiseSourceType);

        foreach (var layer in properties.NoiseLayers)
        {
            filters.Add(NoiseFilters.Create(layer.Properties));
        }

        minValue = float.MaxValue;
        maxValue = float.MinValue;
    }

    public float ComputeColour(Vector3 point)
    {
        float value = 0.0f;
        float firstLayerValue = 0.0f;

        if (properties.FirstLayerIsMask)
        {
            firstLayerValue = filters[0].Evaluate(noiseSource, point);

            if (properties.NoiseLayers[0].Enabled)
            {
                value = firstLayerValue;
            }
        }

        for (int i = 1; i < filters.Count; i++)
        {
            if (properties.NoiseLayers[i].Enabled)
            {
                float mask = properties.FirstLayerIsMask ? firstLayerValue : 1.0f;
                value += filters[i].Evaluate(noiseSource, point) * mask;
            }
        }

        minValue = Mathf.Min(value, minValue);
        maxValue = Mathf.Max(value, maxValue);

        return value;
    }
}
