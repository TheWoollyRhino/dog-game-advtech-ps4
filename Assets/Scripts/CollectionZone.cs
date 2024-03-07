using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionZone : MonoBehaviour
{
    private List<GameObject> collectedObjects;
    [SerializeField] private int collectionZoneLimit = 5;

    private void Awake()
    {
        collectedObjects = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
/*        if (collectedObjects.Count == collectionZoneLimit)
        {
            Debug.Log("Collection Limit Reached");
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Collectable"))
        {
            return;
        }

        var interactablePickupBase = other.gameObject.GetComponent<InteractablePickup>();

        collectedObjects.Add(other.gameObject);
        interactablePickupBase.enabled = false;
        interactablePickupBase.enabled = false;
        interactablePickupBase.canvas.enabled = false;
    }
}
