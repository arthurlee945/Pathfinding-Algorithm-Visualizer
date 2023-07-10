using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    //-------------Movement fields
    [Header("Camera Movment Fields")]
    [SerializeField] InputAction cameraMovement;
    [SerializeField] InputAction movementSpeedUp;
    [SerializeField] float smoothInputSpeed = .3f;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float fastMoveSpeed = 20f;
    Vector2 currentInputMovementVector;
    Vector2 smoothInputVelocity;
    //-------------Drag fields
    Vector2 startingDragPos;
    Vector2 cameraInitPos;

    private void OnEnable()
    {
        cameraMovement.Enable();
    }
    private void OnDisable()
    {
        cameraMovement.Disable();
    }
    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    void Update()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            HanldeCameraMovement();
        }
        else if (Mouse.current.middleButton.isPressed)
        {
            HandleCameraDrag();
        }
        else
        {
            ResetCameraInput();
        }
    }

    private void ResetCameraInput()
    {
        if (currentInputMovementVector.x != 0 || currentInputMovementVector.y != 0)
            currentInputMovementVector = Vector2.zero;
    }

    private void HandleCameraDrag()
    {
        Vector2 currMousePos = Mouse.current.position.ReadValue();
        if (Mouse.current.middleButton.wasPressedThisFrame)
        {
            startingDragPos = currMousePos;
            cameraInitPos = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y);
        }
        Vector2 differentiatedPos = currMousePos - startingDragPos;
        mainCamera.transform.localPosition = new Vector3(cameraInitPos.x + (differentiatedPos.x / 2), cameraInitPos.y + (differentiatedPos.y / 2), mainCamera.transform.position.z);
    }

    private void HanldeCameraMovement()
    {
        Vector2 movement = cameraMovement.ReadValue<Vector2>();
        if (currentInputMovementVector.x == 0 && currentInputMovementVector.y == 0 && movement.x == 0 && movement.y == 0) return;
        currentInputMovementVector = Vector2.SmoothDamp(currentInputMovementVector, movement, ref smoothInputVelocity, smoothInputSpeed);
        mainCamera.transform.Translate(currentInputMovementVector.x * moveSpeed * Time.deltaTime, 0f, currentInputMovementVector.y * moveSpeed * Time.deltaTime, Space.Self);
    }
}
