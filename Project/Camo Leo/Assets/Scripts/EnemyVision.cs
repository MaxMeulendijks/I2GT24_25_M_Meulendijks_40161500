using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyVision : MonoBehaviour
{
    public float visionRange = 15;
    public float visionAngle = 30;

    public bool isInAngle, isInRange, isNotHidden;
    public GameObject player;
    // public TMP_Text rangeText;
    // public TMP_Text hiddenText;
    // public TMP_Text angleText;
    public TMP_Text detectedText;
    public Light spotLight;
    private GameObject enemy;
    public LayerMask playerLayer;
    public LayerMask obstructionLayer;
    private PlayerController playerController;
    private EnemyAI enemyAI;

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject;
        enemyAI = gameObject.GetComponent<EnemyAI>();
        playerController = player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        isInAngle = false;
        isInRange = false;
        isNotHidden = false;
    }

    void LateUpdate() {
        
        Vector3 enemyToPlayerDirection = (player.transform.position - enemy.transform.position).normalized;
        Vector3 enemySideForward = enemy.transform.forward;
        float enemyToPlayerDistance = Vector3.Distance(enemy.transform.position, player.transform.position);

        //Check player is in range of vision
        if(enemyToPlayerDistance < visionRange/2) {
            isInRange = true;
        } else {
            isInRange = false;
        }

        //Check if player is within the cone
        if(Vector3.Angle(enemySideForward, enemyToPlayerDirection) < visionAngle) {
            isInAngle = true;
        }

        //Check if player is not hidden behind an obstacle (like a wall) in the obstacle layer
        if(!Physics.Raycast(enemy.transform.position, enemyToPlayerDirection, enemyToPlayerDistance, obstructionLayer)) {
                //If not hidden behind object, check if player closely matches object behind them in colour.
                CheckCamo(enemySideForward);
        }

        //If within range, cone, and not hidden - player has been spotted
        if(isInAngle && isInRange && isNotHidden && !playerController.gameOver) {
            spotLight.color = Color.red;
            enemyAI.playerSpotted = true;
            enemyAI.chaseStarted = false;
            enemyAI.checkingLastLocation = false;
        //If player hasn't been spotted, indicate green light
        } else if (!playerController.gameOver && enemyAI.playerSpotted == false) {
            spotLight.color = Color.green;
        }
    }

    void CheckCamo(Vector3 enemySideForward){
        //Check in what direction enemy is looking, to know what value to compare against.
        float directionNorth = Vector3.Dot(enemySideForward, Vector3.forward);
        float directionEast = Vector3.Dot(enemySideForward, Vector3.left);
        float directionWest = Vector3.Dot(enemySideForward, Vector3.right);
        float directionSouth = Vector3.Dot(enemySideForward, Vector3.back);
        //Debug.Log(enemySideForward);

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
