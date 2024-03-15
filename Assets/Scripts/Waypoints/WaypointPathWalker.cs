using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPathWalker : MonoBehaviour
{
    [SerializeField] private WaypointPathEditor waypointPathEditor;

    [SerializeField] private float moveSpeed = 5f;

    private Transform currentWaypoint;

    void Start()
    {
        currentWaypoint = waypointPathEditor.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;
    }
}
