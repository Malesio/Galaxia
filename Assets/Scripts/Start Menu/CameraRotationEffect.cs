using UnityEngine;

public class CameraRotationEffect : MonoBehaviour
{
    public float RotationSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
    }
}
