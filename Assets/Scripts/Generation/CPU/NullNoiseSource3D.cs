using UnityEngine;

public class NullNoiseSource3D : INoiseSource
{
    public float Get(Vector3 point)
    {
        return 0.0f;
    }
}
