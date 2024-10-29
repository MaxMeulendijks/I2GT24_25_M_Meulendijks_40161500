using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyVision : MonoBehaviour
{
    public float visionRange = 15;
    public float visionAngle = 30;

    public bool isInAngle, isInRange, isNotHidden;
    public GameObject Player;
    // public TMP_Text rangeText;
    // public TMP_Text hiddenText;
    // public TMP_Text angleText;
    public TMP_Text detectedText;
    public Light spotLight;
    private GameObject enemy;
    public LayerMask playerLayer;
    public LayerMask obstructionLayer;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject;
        playerController = Player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        isInAngle = false;
        isInRange = false;
        isNotHidden = false;
    }

    void LateUpdate() {
        
        Vector3 enemyToPlayerDirection = (Player.transform.position - enemy.transform.position).normalized;
        Vector3 enemySideForward = enemy.transform.forward;
        float enemyToPlayerDistance = Vector3.Distance(enemy.transform.position, Player.transform.position);

        //Check player is in range of vision
        if(enemyToPlayerDistance < visionRange/2) {
            isInRange = true;
            // rangeText.text = "In range";
            // rangeText.color = Color.red;
        } else {
            isInRange = false;
            // rangeText.text = "Not in range";
            // rangeText.color = Color.green;
        }

        //Check if player is within the cone
        if(Vector3.Angle(enemySideForward, enemyToPlayerDirection) < visionAngle) {
            isInAngle = true;
            // angleText.text = "In angle";
            // angleText.color = Color.red;
        } else {
            // angleText.text = "Not in angle";
            // angleText.color = Color.green;
        }

        //Check if player is not hidden behind an obstacle (like a wall) in the obstacle layer
        if(!Physics.Raycast(enemy.transform.position, enemyToPlayerDirection, enemyToPlayerDistance, obstructionLayer)) {
                //If not hidden behind object, check if player closely matches object behind them in colour.
                CheckCamo(enemySideForward);
                // hiddenText.text = "Not hidden";
                // hiddenText.color = Color.red;
        } else {
            // hiddenText.text = "Hidden";
            // hiddenText.color = Color.green;
        }

        //If within range, cone, and not hidden 
        if(isInAngle && isInRange && isNotHidden && !playerController.gameOver) {
            // detectedText.text = "Found";
            // detectedText.color = Color.red;
            spotLight.color = Color.red;
            playerController.gameOver = true;
            detectedText.text = "GAME OVER \n\n Press SPACE to try again";
        } else if (!playerController.gameOver) {
            // detectedText.text = "Not found";
            // detectedText.color = Color.green;
            spotLight.color = Color.green;
        }
    }

    void CheckCamo(Vector3 enemySideForward){
        //TODO: Float comparison does not work for patrolling guard - double check why
        //TODO: Wall hugging seems to prevent proper functioning of the camo, something with raytrace?
        //Check in what direction enemy is looking, to know what value to compare against.
        float directionNorth = Vector3.Dot(enemySideForward, Vector3.forward);
        float directionEast = Vector3.Dot(enemySideForward, Vector3.left);
        float directionWest = Vector3.Dot(enemySideForward, Vector3.right);
        float directionSouth = Vector3.Dot(enemySideForward, Vector3.back);
        Debug.Log(enemySideForward);

        //Use dot to check what direction enemy is looking
        if(directionNorth > .90f && playerController.northVisible) {
            isNotHidden = true;
        } else if (directionEast> .90f && playerController.eastVisible) {
            isNotHidden = true;
        } else if (directionWest> .90f && playerController.westVisible) {
            isNotHidden = true;
        } else if (directionSouth> .90f && playerController.southVisible) {
            isNotHidden = true;
        } else {
            isNotHidden = false;
        }
    }
}
