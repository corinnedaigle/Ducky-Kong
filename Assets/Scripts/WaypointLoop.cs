using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointLoop : MonoBehaviour
{
    public List<Transform> wayPoints = new List<Transform>();
    public bool isMoving;
    public int waypointIndex;
    public float moveSpeed;
    public bool isLoop;

    void Start()
    {
        isLoop = true;
    }

    public void StartMoving()
    {
        // initiates waypoints
        waypointIndex = 0;
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, wayPoints[waypointIndex].position, Time.deltaTime * moveSpeed);

        // makes object move to next waypoint
        var distance = Vector3.Distance(transform.position, wayPoints[waypointIndex].position);
        if (distance <= 0.05f)
        {
            waypointIndex++;

            if (isLoop && waypointIndex>=wayPoints.Count) 
            {
                waypointIndex = 0;
            }
        }
    }
}
