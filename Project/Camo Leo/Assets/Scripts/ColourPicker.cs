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

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        colourTexture = GetComponent<Image>().mainTexture as Texture2D;
    }

    // Update is called once per frame
    void Update()
    {
        if(RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition)) {
            Vector2 delta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out delta);

            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            delta += new Vector2(width * .5f, height * .5f);

            float colourX = Mathf.Clamp(delta.x /width, 0, 1);
            float colourY = Mathf.Clamp(delta.y /height, 0 , 1);

            int textureX = Mathf.RoundToInt(colourX * colourTexture.width);
            int textureY = Mathf.RoundToInt(colourY * colourTexture.height);
            
            OnColourSelected?.Invoke(colourTexture.GetPixel(textureX, textureY));
            if(Input.GetMouseButtonDown(0)) {
                colourPicked = colourTexture.GetPixel(textureX, textureY);
            }
        }
    }
}
