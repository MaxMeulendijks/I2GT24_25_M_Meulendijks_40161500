using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class ColourEvent : UnityEvent<Color> {

}

public class ColourPicker : MonoBehaviour
{
    RectTransform rectTransform;
    Texture2D colourTexture;
    public Color colourPicked;
    public ColourEvent OnColourSelected;
    public ColourEvent OnColourPicked;
    public Image colourIndicator;

    // Start is called before the first frame update
    void Start()
    {
        //Get the space of the image on the screen
        rectTransform = GetComponent<RectTransform>();
        colourTexture = GetComponent<Image>().mainTexture as Texture2D;
    }

    // Update is called once per frame
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.forward, mouseX * 5);
        Vector2 delta;
        //Treat left bottom of image as point 0 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, colourIndicator.transform.position, null, out delta);

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        delta += new Vector2(width * .5f, height * .5f);

        float colourX = Mathf.Clamp(delta.x /width, 0, 1);
        float colourY = Mathf.Clamp(delta.y /height, 0 , 1);

        int textureX = Mathf.RoundToInt(colourX * colourTexture.width);
        int textureY = Mathf.RoundToInt(colourY * colourTexture.height);
        
        //Change the image next to it so player knows what colour the indicator arrow is on
        OnColourSelected?.Invoke(colourTexture.GetPixel(textureX, textureY));
        //When selecting a colour, pass to a variable that can be read by Player object
        if(Input.GetMouseButtonDown(0)) {
            colourPicked = colourTexture.GetPixel(textureX, textureY);
        }
    }
}
