using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    // Relative rotation period
    public float RotationPeriod = 1.0f;

    // Relative revolution period
    public float RevolutionPeriod = 1.0f;

    private const float RotationSpeedScale = 30.0f;
    private const float RevolutionSpeedScale = 3.0f;

    private float _rotationAngularSpeed;
    private float _revolutionAngularSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _rotationAngularSpeed = -RotationSpeedScale / RotationPeriod;
        _revolutionAngularSpeed = -RevolutionSpeedScale / RevolutionPeriod;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, _rotationAngularSpeed * Time.deltaTime, Space.Self);
        transform.RotateAround(Vector3.zero, Vector3.up, _revolutionAngularSpeed * Time.deltaTime);
    }
}
