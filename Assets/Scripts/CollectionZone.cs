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

        var collectableScript = other.gameObject.GetComponent<InteractablePickup>();
        var collectableTrigger = other.gameObject.GetComponent<BoxCollider>();

        collectedObjects.Add(other.gameObject);
        collectableTrigger.enabled = false;
        collectableScript.enabled = false;
        collectableScript.enabled = false;
        collectableScript.canvas.enabled = false;

    }
}
