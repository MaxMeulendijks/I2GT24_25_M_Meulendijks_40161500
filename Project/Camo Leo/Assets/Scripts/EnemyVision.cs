using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyVision : MonoBehaviour
{
    public float visionRange = 15;
    public float visionAngle = 30;

    public bool isInAngle, isInRange, isNotHidden;
    public GameObject Player;
    public TMP_Text rangeText;
    public TMP_Text hiddenText;
    public TMP_Text angleText;
    public TMP_Text detectedText;
    public Light spotLight;
    private GameObject enemy;
    public LayerMask playerLayer;
    public LayerMask obstructionLayer;

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject;
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

        if(enemyToPlayerDistance < visionRange/2) {
            isInRange = true;
            rangeText.text = "In range";
            rangeText.color = Color.red;
        } else {
            isInRange = false;
            rangeText.text = "Not in range";
            rangeText.color = Color.green;
        }

        if(Vector3.Angle(enemySideForward, enemyToPlayerDirection) < visionAngle) {
            isInAngle = true;
            angleText.text = "In angle";
            angleText.color = Color.red;
        } else {
            angleText.text = "Not in angle";
            angleText.color = Color.green;
        }

        if(!Physics.Raycast(enemy.transform.position, enemyToPlayerDirection, enemyToPlayerDistance, obstructionLayer)) {
                isNotHidden = true;
                hiddenText.text = "Not hidden";
                hiddenText.color = Color.red;
        } else {
            hiddenText.text = "Hidden";
            hiddenText.color = Color.green;
        }

        if(isInAngle && isInRange && isNotHidden) {
            detectedText.text = "Found";
            detectedText.color = Color.red;
            spotLight.color = Color.red;
        } else {
            detectedText.text = "Not found";
            detectedText.color = Color.green;
            spotLight.color = Color.green;
        }
    }
}
