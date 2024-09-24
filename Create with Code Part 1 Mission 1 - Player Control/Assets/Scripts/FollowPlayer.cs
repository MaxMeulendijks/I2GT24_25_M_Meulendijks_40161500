using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 5, -7);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // LateUpdate is called after the Update method
    void LateUpdate()
    {
        //Offset the camera behind the player by adding to the player's position
        transform.position = player.transform.position + offset;
    }
}
