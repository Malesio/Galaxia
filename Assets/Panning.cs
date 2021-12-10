using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panning : MonoBehaviour
{
    public float PanningSpeed = 10.0f;
    public float RotationSensitivity = 3.0f;

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
            rotation += rotDelta * RotationSensitivity;

            transform.rotation = Quaternion.Euler(rotation);
        }

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
