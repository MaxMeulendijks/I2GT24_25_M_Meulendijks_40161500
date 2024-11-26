using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class KeyFound : MonoBehaviour
{

    public PlayerController player;
    public GameObject keyFoundIndicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
            player.keyFound = true;
            keyFoundIndicator.SetActive(true);
        }
    }
}
