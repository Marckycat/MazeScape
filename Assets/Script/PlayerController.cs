using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 10.0f;
    private float jumpHeight = 5.0f;
    private float gravityValue = -4.90f;
    private float rotationSpeed = 100.0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // Si no se encuentra el CharacterController, muestra un error
        if (controller == null)
        {
            Debug.LogError("No se ha encontrado un CharacterController en el Prefab.");
        }
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Movimiento hacia adelante y atrás con W y S
        Vector3 move = transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Rotación con A y D
        float rotationInput = Input.GetAxis("Horizontal");
        if (rotationInput != 0)
        {
            transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);
        }

        // Salto
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // Aplicar gravedad
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}

