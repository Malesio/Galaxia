using UnityEngine;

[RequireComponent(typeof(Light))]
public class Star : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color EmissionColour;

    public Color LightColour;

    [Range(500, 25000)]
    public int Temperature = 6500;

    public bool UseTemperatureAsColour;
    [Range(1, 5)]
    public float Intensity;

    private Light starLightSource;
    private Material starMaterial;

    /// <summary>
    /// Converts a temperature in Kelvin into an RGB colour according to black-body emission.
    /// https://tannerhelland.com/2012/09/18/convert-temperature-rgb-algorithm-code.html
    /// </summary>
    /// <param name="temp">The colour temperature in Kelvin</param>
    /// <returns>An RGBA colour</returns>
    private Color TemperatureToRGB(float temp)
    {
        var outColour = new Color();

        temp /= 100;

        outColour.r = temp <= 66 ? 1.0f : Mathf.Clamp01(1.29293618606f * Mathf.Pow(temp - 60, -0.1332047592f));
        outColour.g = Mathf.Clamp01((temp <= 66 ? 
                99.4708025861f * Mathf.Log(temp) - 161.1195681661f :
                288.1221695283f * Mathf.Pow(temp, -0.0755148492f)) / 255.0f);
        outColour.b = temp >= 66 ? 1.0f : (temp <= 19 ? 0.0f : Mathf.Clamp01((138.5177312231f * Mathf.Log(temp - 10) - 305.0447927307f) / 255.0f));

        return outColour;
    }

    private void Initialise()
    {
        starLightSource ??= GetComponent<Light>();
        starMaterial ??= GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void UpdateStar()
    {
        starLightSource.color = UseTemperatureAsColour ? TemperatureToRGB(Temperature) : LightColour;
        starMaterial.SetColor("_EmissionColor", UseTemperatureAsColour ? TemperatureToRGB(Temperature) * Intensity : EmissionColour);
    }

    private void Awake()
    {
        Initialise();
        UpdateStar();
    }

    private void OnValidate()
    {
        Initialise();
        UpdateStar();
    }
}
