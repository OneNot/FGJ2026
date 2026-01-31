using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActionAsset;
    private InputAction moveAction, jumpAction;

    private CharacterController characterController;

    [SerializeField]
    private float moveSpeed = 5f,
    fakeGravity = -9.81f,
    jumpHeight = 1.5f,
    maxJumpHoldTime = 0.5f,
    initialJumpVelocityEffectOnJumpArc = 0.5f;


    private float verticalVelocity = 0f,
    jumpHoldTime = 0f;
    private bool wasJumpPressedLastFrame = false;
    private Vector3 initialHorizontalJumpVelocity;

    void OnEnable()
    {
        inputActionAsset.FindActionMap("Player").Enable();
    }
    void OnDisable()
    {
        inputActionAsset.FindActionMap("Player").Disable();
    }

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        moveAction = inputActionAsset.FindAction("Move");
        jumpAction = inputActionAsset.FindAction("Jump");
    }

    void Update()
    {
        //get movement input
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 moveInput = Vector3.ClampMagnitude(new Vector3(input.x, 0, input.y), 1f);

        //rotate player to face move direction if moving
        if (moveInput != Vector3.zero) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        //get jump input
        bool jumpPressed = jumpAction.IsPressed();

        //start jump immediately on press
        if (jumpPressed && !wasJumpPressedLastFrame && characterController.isGrounded)
        {
            jumpHoldTime = 0f;
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * fakeGravity);
            initialHorizontalJumpVelocity = moveInput * moveSpeed * initialJumpVelocityEffectOnJumpArc;
        }

        //while holding jump and haven't exceeded max hold time, reduce gravity to sustain height
        if (jumpPressed && jumpHoldTime < maxJumpHoldTime)
        {
            jumpHoldTime += Time.deltaTime;
            verticalVelocity += fakeGravity * 0.5f * Time.deltaTime;
        }
        else
        {
            //apply normal gravity when released or max time reached
            verticalVelocity += fakeGravity * Time.deltaTime;
        }

        //reset hold time when jump is released
        if (!jumpPressed)
        {
            jumpHoldTime = 0f;
        }

        wasJumpPressedLastFrame = jumpPressed;

        //calculate horizontal movement
        Vector3 horizontalMove = moveInput * moveSpeed;

        //if mid-air, add initial jump horizontal velocity
        if (!characterController.isGrounded)
        {
            horizontalMove += initialHorizontalJumpVelocity;
        }

        //apply deltaTime to horizontal movement
        horizontalMove *= Time.deltaTime;

        //calculate vertical movement
        Vector3 verticalMove = Vector3.up * verticalVelocity * Time.deltaTime;

        //combine and apply movement
        characterController.Move(horizontalMove + verticalMove);
    }
}
