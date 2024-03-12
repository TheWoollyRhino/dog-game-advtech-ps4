using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//made by Amrit Chatha

public class InteractablePickup : MonoBehaviour
{

    [HideInInspector] public bool interactPressed;
    private bool playerInTrigger;
    private PlayerInput playerInput;
    private GameObject player;
    private PlayerAnimationHashes playerAnimationHashes;

    private bool carrying = false;

    [Header("Initializers")]
    [SerializeField] public Canvas canvas;
    [SerializeField] private Image interactionImage;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private GameObject itemPosition;

    [Header("Design & Text")]
    [SerializeField] private Sprite sprite;
    //[SerializeField] private string description;

    void Awake()
    {
        playerInput = new PlayerInput();
        player = GameObject.FindWithTag("Player");

        playerAnimationHashes = GameObject.FindWithTag("Player").GetComponent<PlayerAnimationHashes>();

        playerInput.Player.Interact.started += OnInteractInput;
        playerInput.Player.Interact.canceled += OnInteractInput;
    }

    void Start()
    {
        canvas.enabled = false;
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    private void Update()
    {
        StartCoroutine(PlayerInteract());
    }

    void OnInteractInput(InputAction.CallbackContext context)
    {
        interactPressed = context.ReadValueAsButton();
    }



    private IEnumerator PlayerInteract()
    {
        if (playerInTrigger)
        {
            if (carrying == false)
            {
                interactionText.text = "Pick Up";
                
                if (interactPressed)
                {
                    playerAnimationHashes.animator.SetBool(playerAnimationHashes.isPickingUpBool, true);
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;

                    yield return new WaitForSeconds(0.5f);

                    gameObject.transform.parent = itemPosition.transform;
                    gameObject.transform.position = itemPosition.transform.position;
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                    gameObject.GetComponent<BoxCollider>().size = new Vector3(1.5f, 1.5f, 1.5f);
                    carrying = true;
                }
            }


            if (carrying == true)
            {
                interactionText.text = "Drop";
                playerAnimationHashes.animator.SetBool(playerAnimationHashes.isPickingUpBool, false);

                if (interactPressed)
                {
                    gameObject.transform.parent = null;
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    gameObject.GetComponent<SphereCollider>().enabled = true;
                    gameObject.GetComponent<BoxCollider>().size = new Vector3(4, 4, 4);

                    yield return new WaitForSeconds(0.3f);

                    carrying = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = true;
            canvas.enabled = true;
            interactionImage.sprite = sprite;
        }
    }

    void OnTriggerExit(Collider other)
    {
        playerInTrigger = false;
        canvas.enabled = false;
    }
}
