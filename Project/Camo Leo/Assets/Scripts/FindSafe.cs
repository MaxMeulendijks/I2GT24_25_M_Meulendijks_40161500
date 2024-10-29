using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FindSafe : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;
    public TMP_Text detectedText;
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check how far player is away from safe
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        Debug.Log(distance);
        //Allow player to win if within reach of save and not yet game over
        if(distance < 2 && !playerController.gameOver) {
            playerController.gameOver = true;
            detectedText.text = "You Won! \n\nCount that money\\Press Space to try again";
        }
    }
}
