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
        if(wayPoints != null && wayPoints.Length > 0) {
            SetCurrentWayPoint();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, target) < 1) {
            IncrementWaypoint();
            SetCurrentWayPoint();
        }
    }

    void SetCurrentWayPoint() {
        target = wayPoints[wayPointsIndex].position;
        navigationAgent.SetDestination(target);
    }

    void IncrementWaypoint() {
        if(wayPointsIndex < wayPoints.Length) {
            wayPointsIndex++;
        } else {
            wayPointsIndex = 0;
        }
    }
}
