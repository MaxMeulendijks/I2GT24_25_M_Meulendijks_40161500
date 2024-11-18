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
    public Color colourPicked;
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
            colourPicked = colourPicker.colourPicked;
            renderer[0].material.color = colourPicked;
            renderer[1].material.color = colourPicked;

            //Get player colour
            string colourPickedHex = colourPicked.ToHexString();
            Debug.Log("Hex colour string: "+colourPickedHex);
            float playerH = int.Parse(colourPickedHex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            float playerS = int.Parse(colourPickedHex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            float playerV = int.Parse(colourPickedHex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

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
            string colourDirectionHex = directionObject.collider.gameObject.GetComponent<MeshRenderer>().material.color.ToHexString();
            float directionH = int.Parse(colourDirectionHex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            float directionS = int.Parse(colourDirectionHex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            float directionV = int.Parse(colourDirectionHex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            //Treat color as if it was in 3D space, as we get 3 "coordinates"
            float distance = Vector3.Distance(new Vector3(directionH, directionS, directionV), new Vector3(playerH, playerS, playerV));
            Debug.Log("Colour distancefrom direction "+directionIndication+":"+distance);
            //Change colour of indicator to indicate player visibility
            //TODO: When switched to hex values, update the distance, so it might be more reliable
            if(distance > 60) {
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
