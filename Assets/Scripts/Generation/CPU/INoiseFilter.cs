using UnityEngine;

public interface INoiseFilter
{
    float Evaluate(INoiseSource noiseSource, Vector3 point);
}
