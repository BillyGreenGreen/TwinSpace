using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackFade : MonoBehaviour
{
    private Image image;
    private AudioSource mainMenuBGM;
    private float mainMenuBGMMaxVolume;
    private float timer = 0;
    private float fadeTimer = 0;
    public GameObject countdown;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (GameObject.Find("MainMenuBGM") != null){
            mainMenuBGM = GameObject.Find("MainMenuBGM").GetComponent<AudioSource>();
            mainMenuBGMMaxVolume = mainMenuBGM.volume;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f){
            fadeTimer += Time.deltaTime;
            float alphaThing = Mathf.Lerp(1f, 0, fadeTimer/4.5f);
            float mainMenuBGMVolume = Mathf.Lerp(mainMenuBGMMaxVolume, 0, fadeTimer/4.5f);
            image.color = new Color(0,0,0,alphaThing);
            if (mainMenuBGM != null){
                mainMenuBGM.volume = mainMenuBGMVolume;
            }
            if (alphaThing == 0){
                countdown.SetActive(true);
                fadeTimer = 0;
                if (GameObject.Find("MainMenuBGM") != null){
                    Destroy(mainMenuBGM.gameObject);
                }
                
                Destroy(gameObject);
                
            }
            
        }
        
        
    }
}
