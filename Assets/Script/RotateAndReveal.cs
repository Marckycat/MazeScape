using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndReveal : MonoBehaviour
{
    public GameObject plane; // El objeto padre (el Plane)
    public KeyCode triggerKey = KeyCode.Space; // La tecla que activará el giro
    public float rotationSpeed = 90f; // Velocidad del giro en grados por segundo
    public float targetRotation = 180f; // El ángulo objetivo de rotación
    private bool isRotating = false; // Si está rotando o no
    private bool isAtTarget = false; // Si el objeto ha alcanzado los 180 grados

    private Quaternion startRotation; // Rotación inicial del objeto
    private Quaternion endRotation; // Rotación final del objeto


    void Start()
    {
        // Desactivar los hijos al inicio
        //SetChildrenActive(false);

        // Definir la rotación inicial y final
        startRotation = plane.transform.rotation;
        endRotation = Quaternion.Euler(plane.transform.eulerAngles + new Vector3(0, 0, targetRotation));

        //Revisar la rotacion actual al iniciar el script
        CheckRotation();

    }

    // Update is called once per frame
    void Update()
    {
        // Si se presiona la tecla y no se ha iniciado la rotación
        if (Input.GetKeyDown(triggerKey) && !isRotating)
        {
            isRotating = true;
        }

        // Si está rotando, ejecutar la rotación
        if (isRotating)
        {
            RotatePlane();
        }
    }

    // Función para rotar el Plane
    private void RotatePlane()
    {
        if (!isAtTarget) // Si no está en la rotación objetivo, rotar hacia 180 grados
        {
            plane.transform.rotation = Quaternion.RotateTowards(plane.transform.rotation, endRotation, rotationSpeed * Time.deltaTime);

            // Verificar si ha alcanzado la rotación objetivo (180 grados)
            if (Quaternion.Angle(plane.transform.rotation, endRotation) < 0.1f)
            {
                plane.transform.rotation = endRotation;
                isRotating = false;
                isAtTarget = true;

                // Revelar los hijos cuando se completa la rotación a 180 grados
                SetChildrenActive(true);
            }
        }
        else // Si ya está en la rotación objetivo, rotar de vuelta a 0 grados
        {
            plane.transform.rotation = Quaternion.RotateTowards(plane.transform.rotation, startRotation, rotationSpeed * Time.deltaTime);

            // Verificar si ha alcanzado la rotación inicial (0 grados)
            if (Quaternion.Angle(plane.transform.rotation, startRotation) < 0.1f)
            {
                plane.transform.rotation = startRotation;
                isRotating = false;
                isAtTarget = false;

                // Ocultar los hijos cuando vuelve a 0 grados
                SetChildrenActive(false);
            }
        }
    }

    //Funcion para verificar la rotacion actual y manejar los hijos
    private void CheckRotation()
    {
        float currentZRotation = plane.transform.eulerAngles.z;

        //Si el objeto esta en 0 grados, los hijos no desaparecen
        if(Mathf.Approximately(currentZRotation, 0f))
        {
            SetChildrenActive(false);
        }
        //Si el objeto esta en 180 grados, los hijos desaparecen
        else if(Mathf.Approximately(currentZRotation, 180f))
        {
            SetChildrenActive(false);
        }
    }

    // Función para activar/desactivar los hijos
    private void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in plane.transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}
