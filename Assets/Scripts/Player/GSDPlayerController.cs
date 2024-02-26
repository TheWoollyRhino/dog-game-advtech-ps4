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

    //animator bools

    // movement variables
    private Vector2 currentWalkingInput;
    private Vector3 walking;
    private bool walkPressed;
    private bool runPressed;

    // jump variables
    private bool jumpPressed;
    private bool onGround;
    private bool requireNewJumpPress;
    private float jumpHorizontalSpeed;

    [Header("Controllers")]
    [SerializeField] [Range(0.1f, 1)] private float playerRotationSpeed = 0.4f;
    //[SerializeField] private float staticJumpForce = 3;
    //[SerializeField] private float walkingJumpForce = 6;
    [SerializeField] private float runningJumpForce = 6;

    void Awake()
    {
        playerCamera = GameObject.Find("Player Camera").transform;
        playerRigidBody = gameObject.GetComponent<Rigidbody>();

        playerInput = new PlayerInput();

        playerAnimationHashes = gameObject.AddComponent<PlayerAnimationHashes>();
        playerAnimationHashes.animator = gameObject.GetComponent<Animator>();

        playerInput.Player.Walk.performed += OnWalkInput;
        playerInput.Player.Walk.canceled += OnWalkInput;

        playerInput.Player.Run.started += OnRunInput;
        playerInput.Player.Run.canceled += OnRunInput;

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
    
    void OnWalkInput(InputAction.CallbackContext context)
    {
        currentWalkingInput = context.ReadValue<Vector2>(); // set the walking vector value to the left gamepad stick coordinates
        walking.x = currentWalkingInput.x;
        walking.z = currentWalkingInput.y;
        walkPressed = walking.x != 0 || walking.y != 0; // check if either the x or the y value in walkPressed is not 0
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        jumpPressed = context.ReadValueAsButton();
        requireNewJumpPress = false;
    }   
    
    void OnRunInput(InputAction.CallbackContext context)
    {
        runPressed = context.ReadValueAsButton();
    } 

    void PlayerMovement()
    {
        bool isRunning = playerAnimationHashes.animator.GetBool(playerAnimationHashes.isRunningBool);
        bool isWalking = playerAnimationHashes.animator.GetBool(playerAnimationHashes.isWalkingBool);

        if (onGround)
        {
            playerAnimationHashes.animator.SetBool(playerAnimationHashes.isJumpingBool, false);

            if (!isWalking && walkPressed)
            {
                playerAnimationHashes.animator.SetBool(playerAnimationHashes.isWalkingBool, true);
            }
            if (isWalking && !walkPressed)
            {
                playerAnimationHashes.animator.SetBool(playerAnimationHashes.isWalkingBool, false);
            }
            if (!isRunning && (walkPressed && runPressed))
            {
                playerAnimationHashes.animator.SetBool(playerAnimationHashes.isRunningBool, true);
            }
            if (isRunning && (!walkPressed || !runPressed))
            {
                playerAnimationHashes.animator.SetBool(playerAnimationHashes.isRunningBool, false);
            }
            if (isRunning && (jumpPressed && !requireNewJumpPress))
            {
                playerRigidBody.velocity = Vector3.up * runningJumpForce;
                requireNewJumpPress = true;
            }
            else
            {
                playerAnimationHashes.animator.SetBool(playerAnimationHashes.isIdleBool, true);
            }
        }

        if (!onGround)
        {
            playerAnimationHashes.animator.SetBool(playerAnimationHashes.isJumpingBool, true);

            Vector3 camForward = new Vector3(playerCamera.forward.x, 0, playerCamera.forward.z);
            Vector3 camRight = new Vector3(playerCamera.right.x, 0, playerCamera.right.z);
            Vector3 fallingVelocity = new Vector3(0, playerRigidBody.velocity.y, 0);

            playerRigidBody.velocity = (currentWalkingInput.y * jumpHorizontalSpeed * camForward) + (currentWalkingInput.x * jumpHorizontalSpeed * camRight) + fallingVelocity;
        }
        jumpHorizontalSpeed = 4;
    }

    void PlayerRotation()
    {
        // ROTATION

        // running.x for the x axis, and running.y for the Z axis. So moving the stick up will go forward on the Z axis

        Vector3 playerMovement = new Vector3(currentWalkingInput.x, 0, currentWalkingInput.y);

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
