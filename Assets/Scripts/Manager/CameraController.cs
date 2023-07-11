using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    //-------------Movement fields
    [Header("Camera Movment Fields")]
    [SerializeField] InputAction cameraMovement;
    [SerializeField] InputAction movementSpeedUp;
    [SerializeField] float smoothInputSpeed = .3f;
    [SerializeField] float defaultMoveSpeed = 10f;
    [SerializeField] float fastMoveSpeed = 20f;
    Vector2 currentInputMovementVector, smoothInputVelocity;
    //-------------Zoom Fields
    [Header("Camera Zoom Fields")]
    [SerializeField] InputAction mouseScrollInput;
    [SerializeField] float scrollTickMoveAmmount = 0.5f;

    //-------------Drag Fields
    [Header("Camera Drag Fields")]
    [SerializeField] float dragAmount = 0.2f;
    Vector2 currentDragVector, smoothDragVelocity, startingDragPos, cameraInitPos;
    //-------------Rot Fields
    Vector2 currentRotVector, smoothRotVelocity;

    //-------------Cursor Fields
    [Header("Cursor Icons")]
    [SerializeField] Texture2D rotationTexture;
    [SerializeField] Texture2D grabTexture;
    bool isDefault = true;

    private void OnEnable()
    {
        cameraMovement.Enable();
        movementSpeedUp.Enable();
        mouseScrollInput.Enable();
    }
    private void OnDisable()
    {
        cameraMovement.Disable();
        movementSpeedUp.Disable();
        mouseScrollInput.Disable();
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
        if (Mouse.current.middleButton.isPressed)
        {
            HandleCameraDrag();
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                IconHandler("grab");
                isDefault = false;
            }

        }
        else if (Mouse.current.rightButton.isPressed)
        {
            HanldeCameraMovement();
            HandleCameraRotation();
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                IconHandler("rot");
                isDefault = false;
            }
        }
        else
        {
            ResetCameraInput();
        }

        float scrollInput = mouseScrollInput.ReadValue<float>();
        if (scrollInput != 0)
            ZoomCamera(scrollInput > 0);

    }

    private void IconHandler(string iconName)
    {
        Cursor.SetCursor(iconName == "grab" ? grabTexture : iconName == "rot" ? rotationTexture : null, new Vector2(0, 0), CursorMode.Auto);
    }

    private void ZoomCamera(bool isPositive)
    {
        float tickMovement = (isPositive ? 1 : -1) * scrollTickMoveAmmount;
        mainCamera.transform.Translate(0f, 0f, tickMovement, Space.Self);
    }

    private void ResetCameraInput()
    {
        if (currentInputMovementVector.x != 0 || currentInputMovementVector.y != 0)
            currentInputMovementVector = Vector2.zero;
        if (currentDragVector.x != 0 || currentDragVector.y != 0)
            currentDragVector = Vector2.zero;
        if (currentRotVector.x != 0 || currentRotVector.y != 0)
            currentDragVector = Vector2.zero;
        if (!isDefault)
        {
            IconHandler("default");
            isDefault = true;
        }
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
        currentDragVector = Vector2.SmoothDamp(currentDragVector, differentiatedPos, ref smoothDragVelocity, smoothInputSpeed);

        mainCamera.transform.localPosition = new Vector3(cameraInitPos.x - (currentDragVector.x * dragAmount), cameraInitPos.y - (currentDragVector.y * dragAmount), mainCamera.transform.position.z);
    }

    private void HanldeCameraMovement()
    {
        Vector2 movement = cameraMovement.ReadValue<Vector2>();
        if (currentInputMovementVector.x == 0 && currentInputMovementVector.y == 0 && movement.x == 0 && movement.y == 0) return;
        currentInputMovementVector = Vector2.SmoothDamp(currentInputMovementVector, movement, ref smoothInputVelocity, smoothInputSpeed);
        float currMoveSpeed = movementSpeedUp.ReadValueAsObject() == null ? defaultMoveSpeed : fastMoveSpeed;
        mainCamera.transform.Translate(currentInputMovementVector.x * currMoveSpeed * Time.deltaTime, 0f, currentInputMovementVector.y * currMoveSpeed * Time.deltaTime, Space.Self);
    }
    private void HandleCameraRotation()
    {
        Vector2 currMousePos = Mouse.current.position.ReadValue();
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            startingDragPos = currMousePos;
        }
        Vector2 relativeMousePos = currMousePos - startingDragPos;
        currentRotVector = Vector2.SmoothDamp(currentRotVector, relativeMousePos, ref smoothRotVelocity, smoothInputSpeed);
        mainCamera.transform.eulerAngles = new Vector3(-currentRotVector.y * dragAmount, currentRotVector.x * dragAmount, 0f);
    }
}
