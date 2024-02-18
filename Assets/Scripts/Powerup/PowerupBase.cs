using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//made by Amrit Chatha

public class PowerupBase : MonoBehaviour
{
    [SerializeField] private Powerup powerup;
    private GameObject aimTarget;
    private Vector3 velocity = Vector3.zero;
    private bool moveToPlayer = false;

    void Start()
    {
        aimTarget = GameObject.Find("AimTarget");
    }
    void LateUpdate()
    {
        MoveToPlayer();
    }

    IEnumerator OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            powerup.ApplyEffects(GameObject.FindGameObjectWithTag("Player"));
            DestroyComponents();

            yield return new WaitForSeconds(powerup.powerupDuration);

            powerup.RemoveEffects(GameObject.FindGameObjectWithTag("Player"));
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        // if the player's collider enters the trigger
        if (collider.gameObject.CompareTag("Player"))
        {
            moveToPlayer = true;
        }
    }
    void MoveToPlayer()
    {
        if (moveToPlayer == true)
        {
            Vector3 playerPosition = aimTarget.transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, powerup.timeToReachTarget);
        }
    }
    void DestroyComponents()
    {
        Destroy(gameObject.GetComponent<ParticleSystem>());
        Destroy(gameObject.GetComponent<SphereCollider>());
        Destroy(gameObject.GetComponent<BoxCollider>());
        Destroy(gameObject.GetComponent<SkinnedMeshRenderer>());
    }
}
