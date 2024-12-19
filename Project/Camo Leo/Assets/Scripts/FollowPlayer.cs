using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public GameObject player;
    public GameObject visibilityIndicators;
    public GameObject colourWheel;
    private PlayerController playerController;
    public Vector3 normalOffset = new Vector3(0, 12.5f, -3.5f);
    public Vector3 gameOverOffset = new Vector3(0, 5, -3.5f);

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.gameOver){
            //Zoom in on player to foreground animation + turn off UI elements for cleaner effect
            transform.position = player.transform.position + gameOverOffset;
            visibilityIndicators.SetActive(false);
            colourWheel.SetActive(false);
        } else {
            //Offset the camera behind the player by adding to the player's position
            transform.position = player.transform.position + normalOffset;
        }
    }
}
