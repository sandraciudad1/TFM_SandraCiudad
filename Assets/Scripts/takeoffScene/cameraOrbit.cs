using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraOrbit : MonoBehaviour
{
    public Transform target;  // El objeto alrededor del cual la c�mara orbitara
    public float distance = 150f;  // Distancia de la c�mara al objeto
    public float rotationSpeed = 20f;  // Velocidad de rotaci�n de la c�mara
    public float height = 3f;  // Altura desde donde la c�mara observa el objeto

    private float currentAngle = 0f;  // �ngulo actual de la c�mara

    void Update()
    {
        // Rotar alrededor del objeto en el eje Y
        currentAngle += rotationSpeed * Time.deltaTime;

        // Calcular la posici�n de la c�mara utilizando coordenadas esf�ricas, a�adiendo altura
        Vector3 offset = new Vector3(0f, height, -distance);
        Quaternion rotation = Quaternion.Euler(height, currentAngle, 0f);  // A�adimos la inclinaci�n en el eje Y
        transform.position = target.position + rotation * offset;

        // Mantener la c�mara mirando hacia el objeto
        transform.LookAt(target);
    }

}
