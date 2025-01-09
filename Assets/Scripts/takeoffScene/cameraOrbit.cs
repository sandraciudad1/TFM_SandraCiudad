using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraOrbit : MonoBehaviour
{
    public Transform target;  // El objeto alrededor del cual la cámara orbitara
    public float distance = 150f;  // Distancia de la cámara al objeto
    public float rotationSpeed = 20f;  // Velocidad de rotación de la cámara
    public float height = 3f;  // Altura desde donde la cámara observa el objeto

    private float currentAngle = 0f;  // Ángulo actual de la cámara

    void Update()
    {
        // Rotar alrededor del objeto en el eje Y
        currentAngle += rotationSpeed * Time.deltaTime;

        // Calcular la posición de la cámara utilizando coordenadas esféricas, añadiendo altura
        Vector3 offset = new Vector3(0f, height, -distance);
        Quaternion rotation = Quaternion.Euler(height, currentAngle, 0f);  // Añadimos la inclinación en el eje Y
        transform.position = target.position + rotation * offset;

        // Mantener la cámara mirando hacia el objeto
        transform.LookAt(target);
    }

}
