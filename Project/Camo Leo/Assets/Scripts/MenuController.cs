using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    public GameObject howToScreen;
    public GameObject baseScreen;
    public GameObject levelPickerScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            baseScreen.SetActive(true);
            howToScreen.SetActive(false);
            levelPickerScreen.SetActive(false);
        }
        
    }

    public void OpenHowToScreen() {
        baseScreen.SetActive(false);
        howToScreen.SetActive(true);
        levelPickerScreen.SetActive(false);
    }

    public void OpenLevelSelectScreen() {
        baseScreen.SetActive(false);
        howToScreen.SetActive(false);
        levelPickerScreen.SetActive(true);
    }

    public void startTutorial() {
        SceneManager.LoadScene("Tutorial");
    }

    public void startTheGallery() {
        SceneManager.LoadScene("TheGallery");
    }
}
