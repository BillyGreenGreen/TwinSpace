using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackFade : MonoBehaviour
{
    private Image image;
    private AudioSource mainMenuBGM;
    private float timer;
    public GameObject countdown;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (GameObject.Find("MainMenuBGM") != null){
            mainMenuBGM = GameObject.Find("MainMenuBGM").GetComponent<AudioSource>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f){
            float alphaThing = Mathf.Lerp(1f, 0, 0.2f * Time.time);
            image.color = new Color(0,0,0,alphaThing);
            if (mainMenuBGM != null){
                mainMenuBGM.volume = alphaThing;
            }
            if (alphaThing == 0){
                countdown.SetActive(true);
                Destroy(gameObject);
            }
            
        }
        
        
    }
}
