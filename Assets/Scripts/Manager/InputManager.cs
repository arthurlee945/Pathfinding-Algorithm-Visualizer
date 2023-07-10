using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [Header("Camera Movment Fields")]
    [SerializeField] InputAction cameraMovement;
    [Header("Camera Rotation Fields")]
    [SerializeField] float smoothInputSpeed = .3f;
    [SerializeField] InputAction cameraRotation;
    [Header("Camera Drag Fields")]
    [SerializeField] InputAction cameraDrag;
    Vector2 currentInputMovementVector;
    Vector2 smoothInputVelocity;
    private void OnEnable()
    {
        cameraMovement.Enable();
    }
    private void OnDisable()
    {
        cameraMovement.Disable();
    }
    void Update()
    {
        HanldeCameraMovement();
    }

    private void HanldeCameraMovement()
    {
        Vector2 movement = cameraMovement.ReadValue<Vector2>();
        if (movement.x == 0 && movement.y == 0) return;
        currentInputMovementVector = Vector2.SmoothDamp(currentInputMovementVector, movement, ref smoothInputVelocity, smoothInputSpeed);
        // mainCamera.transform.localPosition = new Vector3()

    }
}
