public static class NoiseFilters
{
    public static INoiseFilter Create(NoiseProperties props)
    {
        switch (props.FilterType)
        {
            case NoiseProperties.NoiseFilterType.Basic:
                return new BasicNoiseFilter(props);
            case NoiseProperties.NoiseFilterType.Steep:
                return new SteepNoiseFilter(props);
            default:
                break;
        }

        throw new System.NotSupportedException("Noise filter type not supported");
    }
}
