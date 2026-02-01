using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public UIManager uiManager;

    private InputAction moveAction, jumpAction, interactAction;

    private CharacterController characterController;

    // Track nearby interactable objects within trigger range
    private List<GameObject> interactableObjects = new List<GameObject>();

    // Movement and physics parameters
    [SerializeField]
    private float moveSpeed = 5f,
    fakeGravity = -9.81f,
    jumpHeight = 1.5f,
    maxJumpHoldTime = 0.5f,
    initialJumpVelocityEffectOnJumpArc = 0.5f;
    
    private Animator anim;

    // Jump and vertical movement tracking
    private float verticalVelocity = 0f,
    jumpHoldTime = 0f;
    
    // Track jump state for detecting new press
    private bool wasJumpPressedLastFrame = false;
    
    // Horizontal velocity imparted when during a jump
    private Vector3 initialHorizontalJumpVelocity;

    // Quick check for whether the player is grounded
    public bool playerGrounded => characterController.isGrounded;

    void Awake()
    {
        // Cache components and input actions
        characterController = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        interactAction = InputSystem.actions.FindAction("Interact");
	    anim = GetComponentInChildren<Animator>();
    }

    // Add object to interaction list when entering trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractableObject"))
        {
            interactableObjects.Add(other.gameObject);
        }
    }
    
    // Remove object from interaction list when exiting trigger
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractableObject"))
        {
            interactableObjects.Remove(other.gameObject);
        }
    }

    //Audio stuff
    public AudioSource sounds;
    public AudioClip grass;
    public AudioClip ground;
    RaycastHit hit;
    public Transform RayStart;
    public float range;
    public LayerMask layerMask;
    public bool playSound;

    public void Footstep()
    {
        if (Physics.Raycast(RayStart.position, RayStart.transform.up * -1, out hit, range, layerMask))
        {
                if (hit.collider.CompareTag("grass") && playSound == false)
                {
                    PlayFootstepSoundL(grass);
                    playSound = true;
                    StartCoroutine(Wait());
                }
                if (hit.collider.CompareTag("ground") && playSound == false)
                {
                    PlayFootstepSoundL(ground);
                    playSound = true;
                    StartCoroutine(Wait());
                }
        }
    }

    void PlayFootstepSoundL(AudioClip audio)
    {
        sounds.pitch = Random.Range(0.5f, 0.8f);
        sounds.PlayOneShot(audio);
    }
    private void Start()
    {
        playSound = false;
    }

    //TODO: Switch inputs to event-based system rather than polling every frame
    void Update()
    {
        // Handle interaction with nearest interactable object
        if(interactAction.WasPressedThisFrame() && interactableObjects.Count > 0)
        {
            GameObject nearestObject = null;
            float nearestDistanceSqr = float.MaxValue;
            Vector3 playerPosition = transform.position;

            // Find the closest interactable object
            foreach(GameObject obj in interactableObjects)
            {
                float distSqr = (obj.transform.position - playerPosition).sqrMagnitude;
                if(distSqr < nearestDistanceSqr)
                {
                    nearestDistanceSqr = distSqr;
                    nearestObject = obj;
                }
            }

            // Open texture editor for the nearest interactable object if it has an opacity mask
            if(nearestObject != null)
            {
                Texture2D textureToEdit = nearestObject.GetComponent<Renderer>().material.GetTexture("_OpacityMask") as Texture2D;
                if(textureToEdit != null)
                {
                    Debug.Log("Opening Texture Editor for object: " + nearestObject.name);
                    uiManager.StartTextureEditorForObject(nearestObject);
                }
            }
        }

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
	        anim.SetTrigger("jump");
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
	    anim.SetBool("falling",!characterController.isGrounded);


        //apply deltaTime to horizontal movement
        horizontalMove *= Time.deltaTime;

        //apply speed to animation
        anim.SetFloat("speed",horizontalMove.magnitude);

        //calculate vertical movement
        Vector3 verticalMove = Vector3.up * verticalVelocity * Time.deltaTime;

        //combine and apply movement
        characterController.Move(horizontalMove + verticalMove);

        Footstep();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        playSound = false;
    }


}
