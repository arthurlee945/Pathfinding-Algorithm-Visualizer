using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] InputAction cameraMovement;
    [SerializeField] InputAction cameraRotation;
    [SerializeField] InputAction cameraDrag;
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
        // cameraControls.ReadValue<Vector2>();
    }
}
