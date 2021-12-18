using UnityEngine;

public class SimplexNoiseSource3D : INoiseSource
{
    private readonly Noise simplexNoise = new Noise();

    public float Get(Vector3 point)
    {
        return (simplexNoise.Evaluate(point) + 1) * 0.5f;
    }
}
