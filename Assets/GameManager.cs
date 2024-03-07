using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public Volume volume;
    public Canvas uiCanvas;
    public bool isGamePlaying = false;
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;
    public AudioSource BGM;
    [Header("Post Processing")]
    private static ChromaticAberration ca;
    private static LensDistortion ld;
    private static Vignette vg;
    public float vigTimer = 0;
    private float vigTimerDuration = 20;

    [Header("Enemies")]
    [SerializeField] public GameObject[] holySpawnPoints;
    [SerializeField] public GameObject[] voidSpawnPoints;
    [SerializeField] public List<GameObject> holyEnemies;
    [SerializeField] public List<GameObject> voidEnemies;
    public bool shouldSpawnHoly = true;


    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI holyOrbText;
    [SerializeField] private TextMeshProUGUI voidOrbText;
    [SerializeField] private TextMeshProUGUI holyDepositText;
    [SerializeField] private TextMeshProUGUI voidDepositText;
    [SerializeField] private GameObject tpTooltip;
    [SerializeField] private Slider healthSlider;
    

    [Header("Player Stats")]
    public int numberOfHolyOrbs;
    public int numberOfVoidOrbs;
    public int numberOfDepositedHolyOrbs;
    public int numberOfDepositedVoidOrbs;
    public int playerHealth;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out ld);
        volume.profile.TryGet(out ca);
        volume.profile.TryGet(out vg);
        playerHealth = (int)healthSlider.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGamePlaying){
            if (vigTimer < vigTimerDuration){
                vigTimer += Time.deltaTime;
                vg.intensity.value = Mathf.Lerp(0, 0.6f, vigTimer / vigTimerDuration);
                TimeSpan timeS = TimeSpan.FromSeconds(vigTimerDuration - vigTimer);
                if (timeS.Seconds < 12 && tpTooltip != null){
                    tpTooltip.SetActive(true);
                }
                timerText.text = timeS.ToString(@"m\:ss");
            }
            if (vigTimer >= vigTimerDuration){
                KillPlayer();
            }
        }   
        
    }

    private void KillPlayer(){
        isGamePlaying = false;
        gameOverScreen.SetActive(true);
        BGM.Stop();
    }

    private void GameWon(){
        isGamePlaying = false;
        gameWonScreen.SetActive(true);

        BGM.Stop();
    }

    public void DecreasePlayerHealth(int damage){
        playerHealth -= damage;
        healthSlider.value = playerHealth;
        if (playerHealth <= 0){
            //lose
            KillPlayer();
        }
    }

    public void ResetGame(){
        numberOfHolyOrbs = 0;
        numberOfVoidOrbs = 0;
        holyOrbText.text = numberOfHolyOrbs.ToString();
        voidOrbText.text = numberOfVoidOrbs.ToString();
        numberOfDepositedHolyOrbs = 0;
        numberOfDepositedVoidOrbs = 0;
        holyDepositText.text = "0/50";
        voidDepositText.text = "0/50";
        playerHealth = (int)healthSlider.maxValue;
        healthSlider.value = playerHealth;
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        vigTimer = 0;
        GameObject.Find("Player").transform.position = new Vector3(0, -1.33f, 0);
        if (shouldSpawnHoly){
            foreach(GameObject go in holyEnemies){
                Destroy(go);
            }
            holyEnemies.Clear();
        }
        else{
            foreach(GameObject go in voidEnemies){
                Destroy(go);
            }
            voidEnemies.Clear();
        }
        Instantiate(Resources.Load<GameObject>("Prefabs/Countdown"), uiCanvas.transform).SetActive(true);
    }

    public void IncreaseOrbCount(string colour){
        if (colour == "Holy"){
            numberOfHolyOrbs++;
            holyOrbText.text = numberOfHolyOrbs.ToString();
        }
        else{
            numberOfVoidOrbs++;
            voidOrbText.text = numberOfVoidOrbs.ToString();
        }
    }

    public void DecreaseOrbCount(string colour){
        if (colour == "Holy"){
            numberOfHolyOrbs--;
            holyOrbText.text = numberOfHolyOrbs.ToString();
        }
        else{
            numberOfVoidOrbs--;
            voidOrbText.text = numberOfVoidOrbs.ToString();
        }
    }

    public void IncreaseDepositedOrbCount(string colour){
        if (colour == "Holy"){
            numberOfDepositedHolyOrbs++;
            if (numberOfDepositedHolyOrbs <= 50){
                holyDepositText.text = numberOfDepositedHolyOrbs.ToString() + "/50";
            }
        }
        else{
            numberOfDepositedVoidOrbs++;
            if (numberOfDepositedVoidOrbs <= 50){
                voidDepositText.text = numberOfDepositedVoidOrbs.ToString() + "/50";
            }
        }
        if (numberOfDepositedHolyOrbs >= 50 && numberOfDepositedVoidOrbs >= 50){
            //win
            if (PlayerPrefs.HasKey("GamesWon")){
                int num = PlayerPrefs.GetInt("GamesWon");
                num++;
                PlayerPrefs.SetInt("GamesWon", num);
            }
            else{
                PlayerPrefs.SetInt("GamesWon", 1);
            }
            GameWon();
            
            
        }
    }
}
