using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent navigationAgent;
    public Transform[] wayPoints;
    private Vector3 playerPosition;
    public GameObject player;
    private int wayPointsIndex;
    Vector3 nextWaypoint;
    public bool playerSpotted;
    public bool chaseStarted;
    public bool checkingLastLocation;
    public bool returnToStart;
    Vector3 startPosition;
    Quaternion startRotation;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = gameObject.transform.position;
        startRotation = gameObject.transform.rotation;
        navigationAgent = GetComponent<NavMeshAgent>();
        //Make sure guard starts with a waypoint to walk to.
        if(wayPoints != null && wayPoints.Length > 0) {
            SetCurrentWayPoint(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //If at last seen player location, focus on looking around, otherwise do actions.
        if(checkingLastLocation == false) {
            //If player was seen, focus on chasing
            if(playerSpotted == true) {
                //If chase in progress, check whether you've reached player's last known location
                if(chaseStarted == true && Vector3.Distance(transform.position, playerPosition) < 1) {
                    Debug.LogError("Have a look around");
                    checkingLastLocation = true;
                    StartCoroutine(InvestigateLastSeenLocation());
                } 
                //If player is seen, start a run for their position (ignore return to start)
                else if (chaseStarted == false) {
                    Debug.LogError("The chase has commenced");
                    chaseStarted = true;
                    returnToStart = false;
                    playerPosition = player.transform.position;
                    navigationAgent.SetDestination(playerPosition);
                }
            //If player wasn't seen, focus on guarding
            } else {
                //If guard has reached waypoint, move to next waypoint.
                if(wayPoints.Length > 0 && Vector3.Distance(transform.position, nextWaypoint) < 1) {
                    Debug.LogError("Checkout next checkpoint");
                    IncrementWaypoint();
                    SetCurrentWayPoint(false);
                //If guard gave chase and has no patrol route, make effort to return to starting position
                } else if (returnToStart && Vector3.Distance(transform.position, startPosition) < 1) {
                    gameObject.transform.SetPositionAndRotation(transform.position, startRotation);
                }
            }  
        } else {
            Debug.LogError("Still checking last location");
        }
    }

    void SetCurrentWayPoint(bool useStartPosition) {
        Vector3 destination;
        //Set start position if stationary guard
        if(useStartPosition) {
            destination = startPosition;
            returnToStart = true;
        //Set waypoint if patrolling guard
        } else {
            destination = wayPoints[wayPointsIndex].position;
            nextWaypoint = destination;
        }

        navigationAgent.SetDestination(destination);
    }

    void IncrementWaypoint() {
        //Only increment if not yet at end of list, if at end, restart.
        if(wayPointsIndex < wayPoints.Length) {
            wayPointsIndex++;
        } else {
            wayPointsIndex = 0;
        }
    }

    IEnumerator InvestigateLastSeenLocation() {
        //Look forward for player
        gameObject.transform.LookAt(gameObject.transform.position+Vector3.forward);
        Debug.LogError("Looking forward");
        yield return new WaitForSeconds(1f);
        //If player is found, stop and chase
        if(playerSpotted == true && chaseStarted == false) {
            checkingLastLocation = false;
            yield break;
        }
        //Look backward for player
        gameObject.transform.LookAt(gameObject.transform.position+Vector3.back);
        Debug.LogError("Looking backward");
        yield return new WaitForSeconds(1f);
        //If player is found, stop and chase
        if(playerSpotted == true && chaseStarted == false) {
            checkingLastLocation = false;
            yield break;
        }
        //Look right for player
        gameObject.transform.LookAt(gameObject.transform.position+Vector3.right);
        Debug.LogError("Looking right");
        yield return new WaitForSeconds(1f);
        //If player is found, stop and chase
        if(playerSpotted == true && chaseStarted == false) {
            checkingLastLocation = false;
            yield break;
        }
        //Look left for layer
        gameObject.transform.LookAt(gameObject.transform.position+Vector3.left);
        Debug.LogError("Looking left");
        yield return new WaitForSeconds(1f);
        //Player not found, so stop chase and return to start or waypoint
        checkingLastLocation = false;
        Debug.LogError("Are you getting here?");
        playerSpotted = false;
        chaseStarted = false;
        playerPosition = new Vector3(0, 0, 0);

        SetCurrentWayPoint(wayPoints.Length == 0 ? true : false);
    }
}
