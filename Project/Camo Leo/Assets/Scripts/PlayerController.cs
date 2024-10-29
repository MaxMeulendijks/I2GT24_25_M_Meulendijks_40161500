using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    public float speed;
    public ColourPicker colourPicker;
    private GameObject player;

    public LineRenderer northIndicator;
    public LineRenderer westIndicator;
    public LineRenderer southIndicator;
    public LineRenderer eastIndicator;

    public bool northVisible = false;
    public bool westVisible = false;
    public bool southVisible = false;
    public bool eastVisible = false;
    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //If not game over, do this
        if (!gameOver) {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            //Allow player to walk horizontally & vertically
            transform.Translate(Vector3.right*horizontalInput*Time.deltaTime*speed);
            transform.Translate(Vector3.forward*verticalInput*Time.deltaTime*speed);

            //Grab the rendered to change colour to whatever colour was selected
            MeshRenderer[] renderer = player.GetComponentsInChildren<MeshRenderer>();
            renderer[0].material.color = colourPicker.colourPicked;
            renderer[1].material.color = colourPicker.colourPicked;

            //Get player colour
            //TODO: HSV does not seem reliable, try switching method to RGB
            Color.RGBToHSV(renderer[0].material.color, out float playerH, out float playerS, out float playerV);

            //Indicate visibility in each direction
            northVisible = CheckVisibility(1, playerH, playerS, playerV);
            westVisible = CheckVisibility(2, playerH, playerS, playerV);
            southVisible = CheckVisibility(3, playerH, playerS, playerV);
            eastVisible = CheckVisibility(4, playerH, playerS, playerV);
        } else {
            //Reset scene in case of game over or victory
            if(Input.GetKeyDown(KeyCode.Space)) {
                SceneManager.LoadScene("Tutorial");
            }
        }
        
    }

    bool CheckVisibility(int directionIndication, float playerH, float playerS, float playerV) {

        Vector3 directionVector;
        LineRenderer directionIndicator;

        switch(directionIndication) {
            case 1:
                directionIndicator = northIndicator;
                directionVector = Vector3.forward;
            break;
            case 2:
                directionIndicator = westIndicator;
                directionVector = Vector3.right;
            break;
            case 3:
                directionIndicator = southIndicator;
                directionVector = Vector3.back;
            break;
            case 4:
                directionIndicator = eastIndicator;
                directionVector = Vector3.left;
            break;
            default:
                return true;
        }

        //Send out raycasts in a specific direction.
        if(Physics.Raycast(player.transform.position, directionVector, out RaycastHit directionObject, Mathf.Infinity)) {
            //Get the color of the object that was hit
            Color colorDirection = directionObject.collider.gameObject.GetComponent<MeshRenderer>().material.color;
            Color.RGBToHSV(colorDirection, out float directionH, out float directionS, out float directionV);

            //Treat color as if it was in 3D space, as we get 3 "coordinates"
            //TODO: Change this to hex values as that might be more reliable
            float distance = Vector3.Distance(new Vector3(directionH, directionS, directionV), new Vector3(playerH, playerS, playerV));
            Debug.Log("Colour distance: "+distance);
            //Change colour of indicator to indicate player visibility
            //TODO: When switched to hex values, update the distance, so it might be more reliable
            if(distance > .15 && distance <.95) {
                directionIndicator.startColor = Color.red;
                directionIndicator.endColor = Color.red;
                return true;
            } else {
                directionIndicator.startColor = Color.green;
                directionIndicator.endColor = Color.green;
               return false;
            }
        } else {
            return true;
        } 

    }
}
