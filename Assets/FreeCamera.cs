using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public float PanningSpeed = 10.0f;
    public float RotationSensitivity = 3.0f;

    public float ZoomSpeed = 300.0f;
    [Range(5, 170)]
    public float MinFOV = 5.0f;
    [Range(5, 170)]
    public float MaxFOV = 170.0f;

    private Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        var pan = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetButton("Fire2"))
        {
            var rotDelta = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
            rotation += rotDelta * RotationSensitivity * Camera.main.fieldOfView / 1000.0f;

            transform.rotation = Quaternion.Euler(rotation);
        }

        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime, MinFOV, MaxFOV);

        if (Input.GetButton("Jump"))
        {
            pan.y = 1;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            pan.y = -1;
        }

        transform.Translate(PanningSpeed * Time.deltaTime * pan);
    }
}
