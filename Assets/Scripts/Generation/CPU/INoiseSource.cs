using UnityEngine;

public interface INoiseSource 
{
    /**
     * <summary>Get a noise sample at the the specified 3D point.</summary>
     * <returns>A noise sample between 0.0 and 1.0.</returns>
     */
    float Get(Vector3 point);
}
