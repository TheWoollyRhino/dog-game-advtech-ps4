using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

//made by Amrit Chatha

public class GSDPlayerController : MonoBehaviour
{
    private PlayerInput playerInput; // input action asset
    private Rigidbody playerRigidBody;
    private Transform playerCamera;
    private PlayerAnimationHashes playerAnimationHashes;
	private Transform playerTransform;

    //animator bools

    // movement variables
    private Vector2 currentWalkingInput;
    private Vector3 walking;
    private bool walkPressed;

    private Vector2 currentJoggingInput;
    private Vector3 jogging;
    private bool jogPressed;

    // jump variables
    private bool jumpPressed;
    private bool onGround;
    private bool requireNewJumpPress;
    private float jumpHorizontalSpeed;

    [Header("Controllers")]
    [SerializeField] [Range(0.1f, 1)] private float playerRotationSpeed = 0.4f;
    [SerializeField] private float movingJumpForce = 6;
    [SerializeField] private float staticJumpForce = 3;
    //[SerializeField] private float walkSpeed = 60;

    void Awake()
    {
        playerCamera = GameObject.Find("Player Camera").transform;
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
        playerTransform = gameObject.transform;

        playerInput = new PlayerInput();

        playerAnimationHashes = gameObject.AddComponent<PlayerAnimationHashes>();
        playerAnimationHashes.animator = gameObject.GetComponent<Animator>();

        playerInput.Player.Jog.performed += OnMovementInput;
        playerInput.Player.Jog.canceled += OnMovementInput;
        playerInput.Player.Jump.started += OnJumpInput;
        playerInput.Player.Jump.canceled += OnJumpInput;
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    void FixedUpdate()
    {
        PlayerMovement();
        PlayerRotation();
    }
    
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentWalkingInput = context.ReadValue<Vector2>(); // set the walking vector value to the left gamepad stick coordinates
        walking.x = currentWalkingInput.x;
        walking.z = currentWalkingInput.y;
        walkPressed = walking.x != 0 || walking.y != 0; // check if either the x or the y value in walkPressed is not 0

        currentJoggingInput = context.ReadValue<Vector2>(); // set the jogging vector value to the left gamepad stick coordinates
        jogging.x = currentJoggingInput.x;
        jogging.z = currentJoggingInput.y;
        jogPressed = jogging.x != 0 || jogging.y != 0; // check if either the x or the y value in jogPressed is not 0
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        jumpPressed = context.ReadValueAsButton();
        requireNewJumpPress = false;
    }

    void PlayerMovement()
    {
        bool isJogging = playerAnimationHashes.animator.GetBool(playerAnimationHashes.isJoggingBool);
        bool isWalking = playerAnimationHashes.animator.GetBool(playerAnimationHashes.isWalkingBool);

        if (onGround)
        {
            playerAnimationHashes.animator.SetBool(playerAnimationHashes.isJumpingBool, false);

            if (jogPressed && !isJogging)
            {
                playerAnimationHashes.animator.SetBool(playerAnimationHashes.isJoggingBool, true);
            }
            if (!jogPressed && isJogging)
            {
                playerAnimationHashes.animator.SetBool(playerAnimationHashes.isJoggingBool, false);
            }
            if (jogPressed && jumpPressed && !requireNewJumpPress)
            {
                playerRigidBody.velocity = Vector3.up * movingJumpForce;
                requireNewJumpPress = true;
            }
            if (!jogPressed && jumpPressed && !requireNewJumpPress)
            {
                playerAnimationHashes.animator.SetBool(playerAnimationHashes.isJoggingBool, false);
                playerAnimationHashes.animator.SetBool(playerAnimationHashes.isJumpingBool, true);
                playerRigidBody.velocity = Vector3.up * staticJumpForce;
                requireNewJumpPress = true;
            }
        }

        if (!onGround)
        {
            playerAnimationHashes.animator.SetBool(playerAnimationHashes.isJumpingBool, true);

            Vector3 camForward = new Vector3(playerCamera.forward.x, 0, playerCamera.forward.z);
            Vector3 camRight = new Vector3(playerCamera.right.x, 0, playerCamera.right.z);
            Vector3 fallingVelocity = new Vector3(0, playerRigidBody.velocity.y, 0);

            playerRigidBody.velocity = (currentJoggingInput.y * jumpHorizontalSpeed * camForward) + (currentJoggingInput.x * jumpHorizontalSpeed * camRight) + fallingVelocity;
        }
        jumpHorizontalSpeed = 4;
    }

    void PlayerRotation()
    {
        // ROTATION

        // jogging.x for the x axis, and jogging.y for the Z axis. So moving the stick up will go forward on the Z axis

        Vector3 playerMovement = new Vector3(currentJoggingInput.x, 0, currentJoggingInput.y);

        if (playerMovement != Vector3.zero) // if the left gamepad stick is moving
        {
            // targetRotation refers to the player's rotation. Add the main camera Y rotation to player's Y rotation
            float targetRotation = Quaternion.LookRotation(playerMovement).eulerAngles.y + playerCamera.rotation.eulerAngles.y;
            // set the players transform rotation to the main camera's rotation
            playerRigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), playerRotationSpeed));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onGround = false;
        }
    }
}
