using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    public float speed;
    public ColourPicker colourPicker;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right*horizontalInput*Time.deltaTime*speed);
        transform.Translate(Vector3.forward*verticalInput*Time.deltaTime*speed);

        MeshRenderer[] renderer = player.GetComponentsInChildren<MeshRenderer>();
        renderer[0].material.color = colourPicker.colourPicked;
        renderer[1].material.color = colourPicker.colourPicked;
    }
}
