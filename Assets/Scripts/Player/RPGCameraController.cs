using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//made by Amrit Chatha

public class RPGCameraController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private Vector2 steerCameraInput;
    private Vector2 steerCamera;
    private RaycastHit hit;

    [Header("Player Camera Targets")]
    public GameObject playerTarget;
    public GameObject cameraTarget;

    [Header("Player Camera Controllers")]
    public float smoothingTime = 0.2f;
    public float rotationSpeed = 5;
    public LayerMask collisionLayers;

    void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.Player.SteerCamera.performed += OnSteerCamera;
        playerInput.Player.SteerCamera.canceled += OnSteerCamera;
    }

    void Start()
    {
        offset = cameraTarget.transform.localPosition;
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    void LateUpdate()
    {
        CameraRotation();
        SmoothCameraFollow();
        CameraCollision();
    }

    void OnSteerCamera(InputAction.CallbackContext context)
    {
        steerCameraInput = context.ReadValue<Vector2>();
        steerCamera.x = steerCameraInput.x;
        steerCamera.y = steerCameraInput.y;
    }

    void CameraRotation()
    {
        // rotates around the y axis using the x movement on the right joystick
        transform.RotateAround(playerTarget.transform.position, Vector3.up, steerCameraInput.x * rotationSpeed / 2.5f);

        float angleBetween = Vector3.Angle(Vector3.up, transform.forward);

        if (angleBetween > 70.0f && -steerCameraInput.y < 0 || angleBetween < 110.0f && -steerCameraInput.y > 0)
        {
            //uses euler angles to rotate. camera rotates relative to the player axis rather than the world axis
            transform.Rotate(-steerCameraInput.y * rotationSpeed / 3.5f, 0, 0, Space.Self);
        }
    }

    void SmoothCameraFollow()
    {
        if (playerTarget != null)
        {
            Vector3 playerPosition = playerTarget.transform.position;

            // smooths the follow camera
            transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, smoothingTime);
        }
    }

    void CameraCollision()
    {
        // if the line cast is intersected by the collisionLayer
        if (Physics.Linecast(transform.position, transform.position + transform.localRotation * offset, out hit, collisionLayers))
        {
            // set the main camera local position Z axis to minus the distance between the CameraManager and the collision
            // point on the geometry
            cameraTarget.transform.localPosition = new Vector3(0, playerTarget.transform.position.y, -Vector3.Distance(transform.position, hit.point) + 2f);
        }
        else
        {
            // move the main camera local position back to where it started, as fast as the delta time
            cameraTarget.transform.localPosition = Vector3.Lerp(cameraTarget.transform.localPosition, offset, Time.deltaTime);
        }
    }
}
