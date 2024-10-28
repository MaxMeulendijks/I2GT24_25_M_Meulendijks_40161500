using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnimator;
    private AudioSource audioSource;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip crashSound;
    public AudioClip jumpSound;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();       
        audioSource = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier; 
    }

    // Update is called once per frame
    void Update()
    {
        //Check whether jump is possible
        if(Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver) {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnimator.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            audioSource.PlayOneShot(jumpSound, 1.0f);
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //Back on the ground
        if(collision.gameObject.CompareTag("Ground")) {
            isOnGround = true;
            dirtParticle.Play();
        //Ran into obstacle
        } else if (collision.gameObject.CompareTag("Obstacle")) {
            gameOver = true;
            dirtParticle.Stop();
            explosionParticle.Play();
            Debug.Log("Game over!");
            playerAnimator.SetBool("Death_b", true);
            playerAnimator.SetInteger("DeathType_int", 1);
            audioSource.PlayOneShot(crashSound, 1.0f);
        }
        
    }
}
