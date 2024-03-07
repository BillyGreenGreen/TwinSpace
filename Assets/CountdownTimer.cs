using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    private TextMeshProUGUI countdownText;
    private float timer = 2f;
    private float timeToHit = 0.4f;
    private int numberToShow = 4;
    // Start is called before the first frame update
    void Start()
    {
        countdownText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        countdownText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToHit){
            if (numberToShow == 0){
                countdownText.text = "";
                GameManager.Instance.BGM.Play();
                GameManager.Instance.isGamePlaying = true;
                Destroy(gameObject);
            }
            else{
                countdownText.text = numberToShow.ToString();
                SoundEffects.Instance.PlaySound("click");
            }
            
            numberToShow--;
            timer = 0;
        }
        
    }
}
