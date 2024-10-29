using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent navigationAgent;
    public Transform[] wayPoints;
    private int wayPointsIndex;
    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        navigationAgent = GetComponent<NavMeshAgent>();
        //Make sure guard starts with a waypoint to walk to.
        if(wayPoints != null && wayPoints.Length > 0) {
            SetCurrentWayPoint();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //If guard has reached waypoint, move to next waypoint.
        if(Vector3.Distance(transform.position, target) < 1) {
            IncrementWaypoint();
            SetCurrentWayPoint();
        }
    }

    //TODO::Check why patrolling guard cannot see player
    void SetCurrentWayPoint() {
        target = wayPoints[wayPointsIndex].position;
        navigationAgent.SetDestination(target);
        gameObject.transform.LookAt(target);
    }

    void IncrementWaypoint() {
        //Only increment if not yet at end of list, if at end, restart.
        if(wayPointsIndex < wayPoints.Length) {
            wayPointsIndex++;
        } else {
            wayPointsIndex = 0;
        }
    }
}
