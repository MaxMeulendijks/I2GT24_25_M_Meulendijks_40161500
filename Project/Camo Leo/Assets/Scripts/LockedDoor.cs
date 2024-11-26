using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    private ColourInteraction colourInteractor;
    private Renderer lockLightRenderer;
    private bool doorOpen;
    public Material unlockedMaterial;

    // Start is called before the first frame update
    void Start()
    {
        colourInteractor = GetComponentInChildren<ColourInteraction>();
        foreach(Transform transform in gameObject.transform) {
            if(transform.CompareTag("Interactive")) {
                lockLightRenderer = transform.gameObject.GetComponent<Renderer>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(colourInteractor.successfulInteraction && !doorOpen) {
            lockLightRenderer.material = unlockedMaterial;
            doorOpen = true;
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor(){
        for(float i =0; i<=90; i++) {
            gameObject.transform.Rotate(Vector3.up, -1f);
            yield return new WaitForSeconds(.01f);
        }
    }
}
