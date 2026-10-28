using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float speed=5f;
    [SerializeField] private float jumpForce=8f;
    [SerializeField] private float gravity=20f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float cameraDistance=5f;
    [SerializeField] private float cameraTargetHeight=1.5f;
    [SerializeField] private float mouseSensitivity=0.1f;
    [SerializeField] private float minCameraPitch=-30f;
    [SerializeField] private float maxCameraPitch=70f;

    private CharacterController characterController;
    private InputAction moveInput;
    private InputAction jumpInput;
    private InputAction lookInput;
    private float verticalVelocity;
    private float cameraY;
    private float cameraPitch=15f;

    private void Awake()
    {
        characterController=GetComponent<CharacterController>();
        if(cameraTransform == null && Camera.main != null) //checks if camera isn't assigned
        {
            cameraTransform=Camera.main.transform; //uses main camera 
        }

        cameraY=transform.eulerAngles.y; 

        moveInput=new InputAction("Move", InputActionType.Value);
        moveInput.AddCompositeBinding("2DVector") // Map multiple keys to 2D vector input
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        jumpInput=new InputAction("Jump", InputActionType.Button);
        jumpInput.AddBinding("<Keyboard>/space"); 

        lookInput=new InputAction("Look", InputActionType.Value);
        lookInput.AddBinding("<Mouse>/delta");
    }

    private void OnEnable()
    {
        moveInput?.Enable();
        jumpInput?.Enable();
        lookInput?.Enable();
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;
    }

    private void Update()
    {
        HandleCameraInput();
        HandleJump();
        HandleMovement();
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void OnDisable() //disbles movement and enables cursor
    {
        moveInput?.Disable();
        jumpInput?.Disable();
        lookInput?.Disable();
        Cursor.lockState=CursorLockMode.None;
        Cursor.visible=true; 
    }


    private void HandleCameraInput()
    {
        Vector2 lookDelta=lookInput.ReadValue<Vector2>();
        cameraY += lookDelta.x*mouseSensitivity;
        cameraPitch -= lookDelta.y*mouseSensitivity;
        cameraPitch=Mathf.Clamp(cameraPitch, minCameraPitch, maxCameraPitch);

        transform.rotation=Quaternion.Euler(0f, cameraY, 0f);
    }

    private void HandleMovement()
    {
        Vector2 input=Vector2.ClampMagnitude(moveInput.ReadValue<Vector2>(), 1f);
        Vector3 movement=(transform.right*input.x + transform.forward*input.y)*speed;

        if(characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity=-2f;
        }

        verticalVelocity -= gravity*Time.deltaTime;
        movement.y=verticalVelocity;
        characterController.Move(movement*Time.deltaTime);
    }

    private void HandleJump() 
    {
        if(jumpInput.WasPressedThisFrame() && characterController.isGrounded) //Using input system to check if button pressed
        {
            verticalVelocity=jumpForce;
        }
    }

    private void UpdateCameraPosition() //changes camera position based on input
    {
        Quaternion cameraRotation= Quaternion.Euler(cameraPitch, cameraY, 0f);
        Vector3 targetPosition= transform.position + Vector3.up*cameraTargetHeight;

        cameraTransform.position= targetPosition + cameraRotation*Vector3.back*cameraDistance;
        cameraTransform.rotation= cameraRotation;
    }
}
