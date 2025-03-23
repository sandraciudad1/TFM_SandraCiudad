using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera18Controller : MonoBehaviour
{
    float mouseSensitivity = 100f;
    float verticalClamp = 80f;
    float xRotation = 0f;
    public bool startMovement = false;


    // Handles camera movement based on mouse input.
    void Update()
    {
        if (startMovement)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

            transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y, 0f);
            transform.Rotate(Vector3.up * mouseX, Space.World);
        }
    }
}
