using System.Collections;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public PlanetProperties Properties;

    private PlanetPart[] parts;

    private int lastRadius = 0;
    private int lastResolution = 0;

    private int textureResolution = 64;
    private Texture2D colourTexture;

    private float maxColouration;
    private float minColouration;

    private Material instanceMaterial;

    private bool MeshDirty()
    {
        var filters = GetComponentsInChildren<Transform>();

        return filters == null ||
                filters.Length == 0 ||
                parts == null ||
                lastRadius != Properties.Radius ||
                lastResolution != Properties.Resolution;
    }

    public void Initialise()
    {
        if (colourTexture == null)
        {
            textureResolution = Mathf.Max(1, textureResolution);
            colourTexture = new Texture2D(textureResolution, 1, TextureFormat.RGBA32, false);
        }

        if (MeshDirty())
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            parts = new PlanetPart[6];
            var directions = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

            instanceMaterial = new Material(Properties.BaseMaterial);

            for (int i = 0; i < 6; i++)
            {
                var face = new GameObject("face");
                face.transform.parent = transform;

                face.transform.position = transform.position;
                face.transform.rotation = transform.rotation;
                face.transform.localScale = transform.localScale;

                face.AddComponent<MeshRenderer>();
                var filter = face.AddComponent<MeshFilter>();

                filter.sharedMesh = new Mesh();

                filter.GetComponent<MeshRenderer>().sharedMaterial = instanceMaterial;

                parts[i] = new PlanetPart(directions[i], Properties.Radius, Properties.Resolution, filter.sharedMesh);
                parts[i].ComputeShader = Properties.ComputeShader;
                parts[i].Build();
            }

            lastRadius = Properties.Radius;
            lastResolution = Properties.Resolution;
        }
    }

    public void UpdateColourTexture()
    {
        Color[] colours = new Color[textureResolution];
        for (int i = 0; i < textureResolution; i++)
        {
            colours[i] = Properties.ColorGradient.Evaluate((float)i / (textureResolution - 1));
        }

        colourTexture.SetPixels(colours);
        colourTexture.Apply();
    }

    public void UpdateColours()
    {
        maxColouration = float.MinValue;
        minColouration = float.MaxValue;

        foreach (PlanetPart part in parts)
        {
            Vector2 extremes = part.UpdateColourData(Properties);

            minColouration = Mathf.Min(minColouration, extremes.x);
            maxColouration = Mathf.Max(maxColouration, extremes.y);
        }

        UpdateColourTexture();

        instanceMaterial.SetVector("_bounds", new Vector4(minColouration, maxColouration));
        instanceMaterial.SetTexture("_texture", colourTexture);
    }

    public void Regenerate()
    {
        Initialise();
        UpdateColours();
    }

    private void Start()
    {
        Regenerate();
    }

    public void OnPlanetPropertiesUpdated()
    {
        Regenerate();
    }
}
