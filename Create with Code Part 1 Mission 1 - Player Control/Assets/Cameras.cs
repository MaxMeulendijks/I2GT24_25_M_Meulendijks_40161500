using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{
    public Camera mainCamera;
    public Camera bonnetCamera;
    private boolean cameraSwitch;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera.enabled = true;
        bonnetCamera.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
