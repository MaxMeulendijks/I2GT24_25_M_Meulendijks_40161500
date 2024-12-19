using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public Animator animator;
    public float speed;
    public ColourPicker colourPicker;
    public Color colourPicked;
    private GameObject player;

    public Sprite visibleImage;
    public Sprite hiddenImage;

    public Image northIndicator;
    public Image eastIndicator;
    public Image southIndicator;
    public Image westIndicator;

    public bool northVisible = false;
    public bool westVisible = false;
    public bool southVisible = false;
    public bool eastVisible = false;
    public bool gameOver = false;
    public bool gameWon = false;
    public bool keyFound = false;
    public bool directionForward = true;

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
            SkinnedMeshRenderer[] renderer = player.GetComponentsInChildren<SkinnedMeshRenderer>();
            colourPicked = colourPicker.colourPicked;
            renderer[0].material.color = colourPicked;

            //Get player colour
            string colourPickedHex = colourPicked.ToHexString();
            //Debug.Log("Hex colour string: "+colourPickedHex);
            float playerRed = int.Parse(colourPickedHex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            float playerGreen = int.Parse(colourPickedHex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            float playerBlue = int.Parse(colourPickedHex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            //Indicate visibility in each direction
            northVisible = CheckVisibility(1, playerRed, playerGreen, playerBlue);
            westVisible = CheckVisibility(2, playerRed, playerGreen, playerBlue);
            southVisible = CheckVisibility(3, playerRed, playerGreen, playerBlue);
            eastVisible = CheckVisibility(4, playerRed, playerGreen, playerBlue);

            //Turn player model around if moving downwards/upwards for first time
            if (verticalInput < 0 && directionForward) {
                directionForward = !directionForward;
                Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
                foreach(Transform transform in childTransforms) {
                    if(transform.CompareTag("PlayerModel")) {
                        transform.RotateAround(transform.position, Vector3.up, 180);
                    }
                }
            } else if (verticalInput > 0 && !directionForward) {
                directionForward = !directionForward;
                Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
                foreach(Transform transform in childTransforms) {
                    if(transform.CompareTag("PlayerModel")) {
                        transform.RotateAround(transform.position, Vector3.up, 180);
                    }
                }
            }

            animator.SetFloat("vertical", verticalInput);
            animator.SetFloat("horizontal", horizontalInput);

        } else {
            //Make sure player faces camera for gameOver animation
            if (directionForward) {
                directionForward = !directionForward;
                Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
                foreach(Transform transform in childTransforms) {
                    if(transform.CompareTag("PlayerModel")) {
                        transform.RotateAround(transform.position, Vector3.up, 180);
                    }
                }
            }
            
            //Set animations as relevant for game over type
            animator.SetBool("gameOver", gameOver);
            animator.SetBool("victory", gameWon);
            
            //Change scene in case of game over or victory - menu or retry
            if(Input.GetKeyDown(KeyCode.Space)) {
                SceneManager.LoadScene("Tutorial");
            } else if(Input.GetKeyDown(KeyCode.X)){
                SceneManager.LoadScene("Menu");
            }
        }
        
    }

    bool CheckVisibility(int directionIndication, float playerRed, float playerGreen, float playerBlue) {

        Vector3 directionVector;
        Image directionIndicator;

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
        if(Physics.Raycast(player.transform.position, directionVector, out RaycastHit directionObject, Mathf.Infinity) && directionObject.collider.gameObject.CompareTag("Obstacle")) {
            //Get the color of the object that was hit
            string colourDirectionHex = directionObject.collider.gameObject.GetComponent<MeshRenderer>().material.color.ToHexString();
            float directionRed = int.Parse(colourDirectionHex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            float directionGreen = int.Parse(colourDirectionHex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            float directionBlue = int.Parse(colourDirectionHex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            //Treat color as if it was in 3D space, as we get 3 "coordinates"
            float distance = Vector3.Distance(new Vector3(directionRed, directionGreen, directionBlue), new Vector3(playerRed, playerGreen, playerBlue));
            //Debug.Log("Colour distancefrom direction "+directionIndication+":"+distance);
            //Change colour of indicator to indicate player visibility

            if(distance > 60) {
                directionIndicator.sprite = visibleImage;
                directionIndicator.color = Color.red;
                return true;
            } else {
                directionIndicator.sprite = hiddenImage;
                directionIndicator.color = Color.green;
               return false;
            }
        } else {
            return true;
        } 

    }
}
