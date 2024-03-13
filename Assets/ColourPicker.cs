using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourPicker : MonoBehaviour
{
    [SerializeField] RectTransform texture;
    [SerializeField] Texture2D refSprite;
    [SerializeField] SimpleCrosshair crosshair;

    private void Start() {
        if (PlayerPrefs.HasKey("CrosshairR")){
            SetActualColour(new Color(PlayerPrefs.GetFloat("CrosshairR"), PlayerPrefs.GetFloat("CrosshairG"), PlayerPrefs.GetFloat("CrosshairB")));
        }
        else{
            SetActualColour(new Color(1, 0, 0));
        }
    }

    public void OnClickPickerColour(){
        SetColour();
    }
    private void SetColour(){
        Vector3 imagePos = texture.position;
        
        float globalPosX = (Input.mousePosition.x - imagePos.x) * (1920f / Screen.width);
        float globalPosY = (Input.mousePosition.y - imagePos.y) * (1080f / Screen.height);

        int localPosX = (int)(globalPosX * (refSprite.width / texture.rect.width));
        int localPosY = (int)(globalPosY * (refSprite.height / texture.rect.height));

        //Debug.Log(localPosX + " : " + localPosY);

        Color c = refSprite.GetPixel(localPosX, localPosY);
        SetActualColour(c);
    }

    void SetActualColour(Color c){
        crosshair.SetColor(c, true);
        //crosshair.GetComponent<Image>().color = c;
        //set the playerprefs here aswell
        PlayerPrefs.SetFloat("CrosshairR", c.r);
        PlayerPrefs.SetFloat("CrosshairG", c.g);
        PlayerPrefs.SetFloat("CrosshairB", c.b);
    }
}
