using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//made by Amrit Chatha

public class CameraCutsceneBase : MonoBehaviour
{
    public GameObject endTarget;
    public float transitionSpeed;
    public float rotationSpeed;

    [HideInInspector] public Camera cutsceneCamera;
    [HideInInspector] public Camera playerCamera;
    [HideInInspector] public bool doorButtonTransition;

    void Awake()
    {
        playerCamera = GameObject.Find("Player Camera").GetComponent<Camera>();
        cutsceneCamera = gameObject.GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (doorButtonTransition)
        {
            transform.position = Vector3.MoveTowards(transform.position, endTarget.transform.position, transitionSpeed * Time.deltaTime);

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, endTarget.transform.forward, rotationSpeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
