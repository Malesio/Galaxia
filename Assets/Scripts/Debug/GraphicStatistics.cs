using UnityEngine;
using UnityEngine.UI;

public class GraphicStatistics : MonoBehaviour
{
    public bool showDebugInformation = false;
    private const int framesBeforeUpdate = 120;

    private int updateCounter = 0;
    private int frameCounter = 0;
    private float accumulatedTime = 0.0f;
    private float accumulatedUpdateTime = 0.0f;

    private Text guiLabel;

    void Start()
    {
        guiLabel = GetComponent<Text>();
        guiLabel.enabled = showDebugInformation;
    }

    public void ToggleDebugInformation()
    {
        showDebugInformation = !showDebugInformation;
        guiLabel.enabled = showDebugInformation;
    }

    private void ResetCounters()
    {
        frameCounter = 0;
        accumulatedTime = 0.0f;
        updateCounter = 0;
        accumulatedUpdateTime = 0.0f;
    }

    void Update()
    {
        if (showDebugInformation)
        {
            frameCounter++;
            accumulatedTime += Time.smoothDeltaTime;

            if (frameCounter == framesBeforeUpdate)
            {
                int totalVertices = 0;
                int totalTriangles = 0;

                foreach (MeshFilter filter in FindObjectsOfType<MeshFilter>())
                {
                    totalVertices += filter.sharedMesh.vertexCount;
                    totalTriangles += filter.sharedMesh.triangles.Length / 3;
                }

                int averageFps = (int)(frameCounter / accumulatedTime);
                int averageUps = (int)(updateCounter / accumulatedUpdateTime);

                guiLabel.text = string.Format("FPS: {0}\nUPS: {1}\nVertices: {2}\nTriangles: {3}", averageFps, averageUps, totalVertices, totalTriangles);

                ResetCounters();
            }
        }
    }

    private void FixedUpdate()
    {
        if (showDebugInformation)
        {
            updateCounter++;
            accumulatedUpdateTime += Time.fixedDeltaTime;
        }
    }
}
