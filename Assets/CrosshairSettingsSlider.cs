using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrosshairSettingsSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_InputField valueText;
    [SerializeField] private SimpleCrosshair crosshair;
    [SerializeField] private CrosshairSettingsSliderText settingsSlider;
    public bool shouldChange = true;
    
    public void ValueChanged(){
        if (!shouldChange) return;
        settingsSlider.shouldChange = false;
        valueText.text = slider.value.ToString("0.0");
        settingsSlider.shouldChange = true;
        if (gameObject.name.Contains("Thickness")){
            crosshair.SetThickness((int)(slider.value * 10), true);
            PlayerPrefs.SetInt("CrosshairThickness", (int)(slider.value * 10));
        }
        else if (gameObject.name.Contains("Size")){
            crosshair.SetSize((int)(slider.value * 10), true);
            PlayerPrefs.SetInt("CrosshairSize", (int)(slider.value * 10));
        }
        else if (gameObject.name.Contains("Gap")){
            crosshair.SetGap((int)(slider.value * 10), true);
            PlayerPrefs.SetInt("CrosshairGap", (int)(slider.value * 10));
        }
        else if (gameObject.name.Contains("Red")){
            crosshair.SetColor(CrosshairColorChannel.RED, (int)(slider.value * 255), true);
            //Debug.Log((int)slider.value * 255);
            PlayerPrefs.SetInt("CrosshairR", (int)(slider.value * 255));
        }
        else if (gameObject.name.Contains("Green")){
            crosshair.SetColor(CrosshairColorChannel.GREEN, (int)(slider.value * 255), true);
            PlayerPrefs.SetInt("CrosshairG", (int)(slider.value * 255));
        }
        else if (gameObject.name.Contains("Blue")){
            crosshair.SetColor(CrosshairColorChannel.BLUE, (int)(slider.value * 255), true);
            PlayerPrefs.SetInt("CrosshairB", (int)(slider.value * 255));
        }
        
    }

    private void Start() {
        if (gameObject.name.Contains("Thickness")){
            slider.value = (float)PlayerPrefs.GetInt("CrosshairThickness") / 10;
        }
        else if (gameObject.name.Contains("Size")){
            slider.value = (float)PlayerPrefs.GetInt("CrosshairSize") / 10;
        }
        else if (gameObject.name.Contains("Gap")){
            slider.value = (float)PlayerPrefs.GetInt("CrosshairGap") / 10;
        }
        else if (gameObject.name.Contains("Red")){
            slider.value = (float)PlayerPrefs.GetInt("CrosshairR") / 255;
        }
        else if (gameObject.name.Contains("Green")){
            slider.value = (float)PlayerPrefs.GetInt("CrosshairG") / 255;
        }
        else if (gameObject.name.Contains("Blue")){
            slider.value = (float)PlayerPrefs.GetInt("CrosshairB") / 255;
        }
        valueText.text = slider.value.ToString("0.0");
    }

    private void ChangeCrosshair(){

    }
}
