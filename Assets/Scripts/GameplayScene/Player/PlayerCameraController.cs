using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] Transform playerBody;  
    
    float mouseSensitivity = 100f;  
    float verticalClamp = 80f;  
    float xRotation = 0f;

    [SerializeField] GameObject inventory;
    inventoryController inventoryCont;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  
        Cursor.visible = false;
        inventoryCont = inventory.GetComponent<inventoryController>();
    }

    void Update()
    {
        if (inventoryCont != null && inventoryCont.playerMov)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        
    }
}
