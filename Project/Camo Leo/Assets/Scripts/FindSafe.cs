using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FindSafe : MonoBehaviour
{
    public PlayerController playerController;
    public TMP_Text detectedText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player") && playerController.keyFound) {
            playerController.gameOver = true;
            playerController.gameWon = true;
            detectedText.text = "You Won! \n\nCount that money\\Press Space to try again";
        }
    }
}
