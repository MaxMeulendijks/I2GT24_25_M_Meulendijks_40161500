using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColourInteraction : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;
    public Color colourRequired;
    private float requiredRed;
    private float requiredGreen;
    private float requiredBlue;
    private Vector3 requiredColourPosition;
    public bool successfulInteraction;
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
            string colourRequiredHex = colourRequired.ToHexString();
            //Debug.Log("Hex colour string: "+colourPickedHex);
            requiredRed = int.Parse(colourRequiredHex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            requiredGreen = int.Parse(colourRequiredHex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            requiredBlue = int.Parse(colourRequiredHex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            requiredColourPosition = new Vector3(requiredRed, requiredGreen, requiredBlue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision other)
    {
        if(successfulInteraction == false) {
            //Get player colour
            string colourPickedHex = playerController.colourPicked.ToHexString();
            //Debug.Log("Hex colour string: "+colourPickedHex);
            float playerRed = int.Parse(colourPickedHex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            float playerGreen = int.Parse(colourPickedHex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            float playerBlue = int.Parse(colourPickedHex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            if(Vector3.Distance(requiredColourPosition, new Vector3(playerRed, playerGreen, playerBlue)) < 60) {
                successfulInteraction = true;
            }
        } 
    }
}
