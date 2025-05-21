using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndReveal : MonoBehaviour
{
    public GameObject plane; // El objeto padre (el Plane)
    public KeyCode triggerKey = KeyCode.Space; // La tecla que activar� el giro
    public float rotationSpeed = 90f; // Velocidad del giro en grados por segundo
    public float targetRotation = 180f; // El �ngulo objetivo de rotaci�n
    private bool isRotating = false; // Si est� rotando o no
    private bool isAtTarget = false; // Si el objeto ha alcanzado los 180 grados

    private Quaternion startRotation; // Rotaci�n inicial del objeto
    private Quaternion endRotation; // Rotaci�n final del objeto


    void Start()
    {
        // Desactivar los hijos al inicio
        //SetChildrenActive(false);

        // Definir la rotaci�n inicial y final
        startRotation = plane.transform.rotation;
        endRotation = Quaternion.Euler(plane.transform.eulerAngles + new Vector3(0, 0, targetRotation));

        //Revisar la rotacion actual al iniciar el script
        CheckRotation();

    }

    // Update is called once per frame
    void Update()
    {
        // Si se presiona la tecla y no se ha iniciado la rotaci�n
        if (Input.GetKeyDown(triggerKey) && !isRotating)
        {
            isRotating = true;
        }

        // Si est� rotando, ejecutar la rotaci�n
        if (isRotating)
        {
            RotatePlane();
        }
    }

    // Funci�n para rotar el Plane
    private void RotatePlane()
    {
        if (!isAtTarget) // Si no est� en la rotaci�n objetivo, rotar hacia 180 grados
        {
            plane.transform.rotation = Quaternion.RotateTowards(plane.transform.rotation, endRotation, rotationSpeed * Time.deltaTime);

            // Verificar si ha alcanzado la rotaci�n objetivo (180 grados)
            if (Quaternion.Angle(plane.transform.rotation, endRotation) < 0.1f)
            {
                plane.transform.rotation = endRotation;
                isRotating = false;
                isAtTarget = true;

                // Revelar los hijos cuando se completa la rotaci�n a 180 grados
                SetChildrenActive(true);
            }
        }
        else // Si ya est� en la rotaci�n objetivo, rotar de vuelta a 0 grados
        {
            plane.transform.rotation = Quaternion.RotateTowards(plane.transform.rotation, startRotation, rotationSpeed * Time.deltaTime);

            // Verificar si ha alcanzado la rotaci�n inicial (0 grados)
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

    // Funci�n para activar/desactivar los hijos
    private void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in plane.transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}
