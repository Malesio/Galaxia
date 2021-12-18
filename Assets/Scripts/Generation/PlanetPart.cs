using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlanetPart
{
    private Vector3[] baseVectors;
    private Mesh mesh;
    private int radius;
    private int resolution;

    public struct ComputeNoiseProperties
    {
        public float Strength;
        public float Roughness;
        public int Octaves;
        public float Persistence;
        public float Lacunarity;
        public byte IsSteep;
        public int Sharpness;
        public float WeightFactor;
        public Vector3 Centre;
        public byte Enabled;
    }

    public ComputeShader ComputeShader { get; set; }

    public PlanetPart(Vector3 orientation, int radius, int resolution, Mesh mesh)
    {
        baseVectors = new Vector3[3];

        baseVectors[0] = orientation;
        baseVectors[1] = new Vector3(orientation.y, orientation.z, orientation.x);
        baseVectors[2] = Vector3.Cross(orientation, baseVectors[1]);

        this.resolution = resolution;
        this.mesh = mesh;
        this.radius = radius;
    }

    /**
     * <summary>Build vertices for a cube and normalise each vertex to form a pseudo-sphere.</summary>
     * <remarks>Process inspired from Sebastian Lague at https://github.com/SebLague/Procedural-Planets </remarks>
     */
    public void Build()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        var vertices = new Vector3[resolution * resolution];
        var indices = new int[(resolution - 1) * (resolution - 1) * 6];
        int currentIndex = 0;
        int currentVertex = 0;

        var uv = mesh?.uv.Length == vertices.Length ? mesh.uv : new Vector2[vertices.Length];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                // Vector scaled w/ coordinates b/w 0 and 1
                var scaled = new Vector2(x, y) / (resolution - 1);

                var cubePoint = baseVectors[0] + (scaled.x - 0.5f) * 2 * baseVectors[1] + (scaled.y - 0.5f) * 2 * baseVectors[2];
                var spherePoint = cubePoint.normalized;

                vertices[currentVertex] = spherePoint * radius;

                if (x != resolution - 1 && y != resolution - 1)
                {
                    indices[currentIndex] = currentVertex;
                    indices[currentIndex + 1] = currentVertex + resolution + 1;
                    indices[currentIndex + 2] = currentVertex + resolution;

                    indices[currentIndex + 3] = currentVertex;
                    indices[currentIndex + 4] = currentVertex + 1;
                    indices[currentIndex + 5] = currentVertex + resolution + 1;

                    currentIndex += 6;
                }

                currentVertex++;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.uv = uv;

        sw.Stop();
        // Debug.Log("Mesh rebuilding took " + sw.ElapsedMilliseconds + " ms");
    }

    public Vector2 UpdateColourData(PlanetProperties properties)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        Vector2[] uv = mesh.uv;
        var extremes = new Vector2(float.MaxValue, float.MinValue);

        if (SystemInfo.supportsComputeShaders)
        {
            // Output buffer
            using var noiseBuffer = new ComputeBuffer(mesh.vertexCount, sizeof(float) * 2);

            noiseBuffer.SetData(uv);

            // Input buffers
            using var vertexBuffer = new ComputeBuffer(mesh.vertexCount, sizeof(float) * 3);
            using var noisePropertiesBuffer = new ComputeBuffer(properties.NoiseLayers.Count, Marshal.SizeOf<ComputeNoiseProperties>());

            vertexBuffer.SetData(mesh.vertices);

            var computeNoiseProps = new ComputeNoiseProperties[properties.NoiseLayers.Count];
            var noiseProps = properties.NoiseLayers.Select(layer => layer.Properties).ToArray();

            for (int i = 0; i < computeNoiseProps.Length; i++)
            {
                computeNoiseProps[i] = new ComputeNoiseProperties();

                computeNoiseProps[i].Strength = noiseProps[i].Strength;
                computeNoiseProps[i].Roughness = noiseProps[i].Roughness;
                computeNoiseProps[i].Octaves = noiseProps[i].Octaves;
                computeNoiseProps[i].Persistence = noiseProps[i].Persistence;
                computeNoiseProps[i].Lacunarity = noiseProps[i].Lacunarity;
                computeNoiseProps[i].IsSteep = (byte)(noiseProps[i].FilterType == NoiseProperties.NoiseFilterType.Steep ? 1 : 0);
                computeNoiseProps[i].Sharpness = noiseProps[i].Sharpness;
                computeNoiseProps[i].WeightFactor = noiseProps[i].WeightFactor;
                computeNoiseProps[i].Centre = noiseProps[i].Offset;
                computeNoiseProps[i].Enabled = (byte)(properties.NoiseLayers[i].Enabled ? 1 : 0);
            }

            noisePropertiesBuffer.SetData(computeNoiseProps);

            ComputeShader.SetBuffer(0, "NoiseMap", noiseBuffer);
            ComputeShader.SetBuffer(0, "Vertices", vertexBuffer);
            ComputeShader.SetBuffer(0, "Layers", noisePropertiesBuffer);
            ComputeShader.SetBool("FirstLayerIsMask", properties.FirstLayerIsMask);
            ComputeShader.SetInt("LayerCount", properties.NoiseLayers.Count);

            ComputeShader.Dispatch(0, vertexBuffer.count / 20, 1, 1);

            noiseBuffer.GetData(uv);

            foreach (var sample in uv)
            {
                extremes.x = Mathf.Min(sample.x, extremes.x);
                extremes.y = Mathf.Max(sample.x, extremes.y);
            }
        }
        else
        {
            var cpuColouriser = new Colouriser(properties);
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                uv[i].x = cpuColouriser.ComputeColour(vertices[i]);
            }

            extremes.x = cpuColouriser.MinColour;
            extremes.y = cpuColouriser.MaxColour;
        }

        mesh.uv = uv;

        sw.Stop();
        // Debug.Log("Procedural colouring took " + sw.ElapsedMilliseconds + " ms");

        return extremes;
    }
}
