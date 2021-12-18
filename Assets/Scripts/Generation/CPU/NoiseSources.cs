public static class NoiseSources
{
    public static INoiseSource Create(NoiseSourceType type)
    {
        switch (type)
        {
            case NoiseSourceType.Simplex:
                return new SimplexNoiseSource3D();
            case NoiseSourceType.Nothing:
                return new NullNoiseSource3D();
            default:
                break;
        }

        throw new System.NotSupportedException("Noise source type not supported");
    }
}
