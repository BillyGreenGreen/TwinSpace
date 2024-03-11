using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public PlayerCrosshair playerCrosshair;
    public PlayerController playerController;
    public SlimeSpawner[] slimeSpawners;
    public PowerUps powerUps;
    public Volume volume;
    public Canvas uiCanvas;
    public bool isGamePlaying = false;
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;
    public GameObject pauseScreen;
    public AudioSource BGM;
    public int arenaIndex = 0;
    public int yOffset = -1;
    private int amountOfArenas = 2; //amount of arenas to choose from

    [Header("Post Processing")]
    private static ChromaticAberration ca;
    private static LensDistortion ld;
    private static Vignette vg;
    public float vigTimer = 0;
    private float vigTimerDuration = 20;

    [Header("Enemies")]
    [SerializeField] public List<GameObject> holyEnemies;
    [SerializeField] public List<GameObject> voidEnemies;
    public bool shouldSpawnHoly = true;


    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI holyOrbText;
    [SerializeField] private TextMeshProUGUI voidOrbText;
    [SerializeField] private TextMeshProUGUI[] holyDepositText;
    [SerializeField] private TextMeshProUGUI[] voidDepositText;
    [SerializeField] private GameObject tpTooltip;
    [SerializeField] public Slider healthSlider;
    [SerializeField] private GameObject shieldEffect;
    [SerializeField] private GameObject powerUpParent;
    [SerializeField] private TextMeshProUGUI healthText;
    

    [Header("Player Stats")]
    public int numberOfHolyOrbs;
    public int numberOfVoidOrbs;
    public int numberOfDepositedHolyOrbs;
    public int numberOfDepositedVoidOrbs;
    public int playerHealth;
    private float numberOfOrbsToCollect = 1;
    private float bulletSpeed = 20;
    public bool invincible = false;
    private float invincibleTimer;
    private float invincibleTimerDuration;
    private List<int> shieldHealthPercentages = new List<int>();
    public int stage = 1;

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
        Debug.Log("GM START");
        volume.profile.TryGet(out ld);
        volume.profile.TryGet(out ca);
        volume.profile.TryGet(out vg);
        playerHealth = (int)healthSlider.maxValue;
        holyDepositText[arenaIndex].text = "0/" + numberOfOrbsToCollect.ToString();
        voidDepositText[arenaIndex].text = "0/" + numberOfOrbsToCollect.ToString();

        if (!PlayerPrefs.HasKey("GamesWon")){
            PlayerPrefs.SetInt("GamesWon", 0);
        }
        if (!PlayerPrefs.HasKey("SavedStage")){
            //PlayerPrefs.SetInt("SavedStage", 1);
            //stage = PlayerPrefs.GetInt("SavedStage");
        }

        arenaIndex = 0;
        slimeSpawners[arenaIndex].spawnerIsActive = true;

        

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
            if (invincible){
                invincibleTimer += Time.deltaTime;
                if (invincibleTimer <= invincibleTimerDuration){
                    shieldEffect.SetActive(true);
                }
                else{
                    shieldEffect.SetActive(false);
                    invincible = false;
                    invincibleTimer = 0;
                }
            }
            if (vigTimer >= vigTimerDuration){
                KillPlayer();
            }
            if (Input.GetKeyDown(KeyCode.Escape)){
                PauseGame();
            }

        }
        
    }
    public void EnableShield(int duration, List<int> percentages){
        shieldHealthPercentages.Clear();
        foreach (int percent in percentages){
            shieldHealthPercentages.Add(percent);
        }
        invincibleTimerDuration = duration;
    }

    public void DisableShield(){
        invincibleTimerDuration = 0;
    }

    public float GetBulletSpeed(){
        return bulletSpeed;
    }

    public void SetBulletSpeed(float speed){
        bulletSpeed = speed;
    }

    public void UnpauseGame(){
        BGM.Play();
        isGamePlaying = true;
        pauseScreen.SetActive(false);
        playerCrosshair.ShowCrosshair();
    }

    private void PauseGame(){
        isGamePlaying = false;
        pauseScreen.SetActive(true);
        BGM.Pause();
        playerCrosshair.HideCrosshair();
    }

    public void KillPlayer(){
        if (isGamePlaying){
            isGamePlaying = false;
            gameOverScreen.SetActive(true);
            BGM.Stop();
            playerCrosshair.HideCrosshair();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Orb")){
                Destroy(go);
            }
        }   
        
    }

    private void GameWon(){
        isGamePlaying = false;
        gameWonScreen.SetActive(true);
        gameWonScreen.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "STAGE " + stage + " COMPLETE";
        stage++;
        //PlayerPrefs.SetInt("StageSaved", stage);
        //Debug.Log(powerUps.GeneratePowerUpsToBuy().Count);
        foreach (string s in powerUps.GeneratePowerUpsToBuy()){
            string nameOfPowerUp = s.Split(" ")[0];
            string levelOfPowerUp;
            try{
                levelOfPowerUp = s.Split(" ")[1];
            }catch{
                levelOfPowerUp = "";
            }
            
            //Debug.Log(nameOfPowerUp + levelOfPowerUp);
            GameObject go = Instantiate(Resources.Load<GameObject>("PowerUps/PowerUpTile"), powerUpParent.transform);
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>("PowerUps/" + nameOfPowerUp);
            if (nameOfPowerUp == "fasterFireRate"){
                nameOfPowerUp = "faster fire rate";
            }
            else if (nameOfPowerUp == "fasterProj"){
                nameOfPowerUp = "faster projectiles";
            }
            else if (nameOfPowerUp == "dash"){
                nameOfPowerUp = "faster dash cooldown";
            }
            go.transform.Find("NameOfPowerUp").GetComponent<TextMeshProUGUI>().text = nameOfPowerUp;
            if (levelOfPowerUp != ""){
                go.transform.Find("LevelOfPowerUp").GetComponent<TextMeshProUGUI>().text = "Level " + levelOfPowerUp;
            }
            
            
        }
        playerCrosshair.HideCrosshair();
        
        BGM.Stop();
    }

    public void DecreasePlayerHealth(int damage){
        if (isGamePlaying && !playerController.isDashing){
            if (!invincible){
                int calcHealth;
                calcHealth = playerHealth;
                playerHealth -= damage;
                healthSlider.value = playerHealth;
                
                healthText.text = string.Format("{0}/{1}", playerHealth.ToString(), healthSlider.maxValue.ToString());
                
                for (int i = 0; i < damage; i++)
                {
                    calcHealth -= 1;
                    if (shieldHealthPercentages.Contains(((int)((calcHealth / healthSlider.maxValue) * 100)))){
                        invincible = true;
                        break;
                    }
                }
                if (playerHealth <= 0){
                    KillPlayer();
                }
            }
            
            
        }
    }

    public void IncreasePlayerHealth(int healAmount){
        playerHealth += healAmount;
        healthSlider.value = playerHealth;
        healthText.text = string.Format("{0}/{1}", playerHealth.ToString(), healthSlider.maxValue.ToString());
        if (playerHealth > healthSlider.maxValue){
            playerHealth = (int)healthSlider.maxValue;
        }
    }

    public void ResetGame(){
        playerCrosshair.ShowCrosshair();
        numberOfHolyOrbs = 0;
        numberOfVoidOrbs = 0;
        holyOrbText.text = numberOfHolyOrbs.ToString();
        voidOrbText.text = numberOfVoidOrbs.ToString();
        numberOfDepositedHolyOrbs = 0;
        numberOfDepositedVoidOrbs = 0;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PowerUpTile")){
            Destroy(go);
        }
        playerHealth = (int)healthSlider.maxValue;
        healthSlider.value = playerHealth;
        healthText.text = string.Format("{0}/{1}", playerHealth.ToString(), healthSlider.maxValue.ToString());
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        vigTimer = 0;
        slimeSpawners[arenaIndex].spawnerIsActive = false;
        arenaIndex = UnityEngine.Random.Range(0, amountOfArenas);
        slimeSpawners[arenaIndex].spawnerIsActive = true;
        holyDepositText[arenaIndex].text = "0/" + Math.Truncate(numberOfOrbsToCollect).ToString();
        voidDepositText[arenaIndex].text = "0/" + Math.Truncate(numberOfOrbsToCollect).ToString();
        yOffset = -((arenaIndex * 100) + 1); //if 2 => -201
        playerController.gameObject.transform.position = new Vector3(0, yOffset, 0);
        //GameObject.Find("Player").transform.position = new Vector3(0, -1f, 0);


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
        if (GameObject.Find("Holy_Big_Slime(Clone)") != null){
            Destroy(GameObject.Find("Holy_Big_Slime(Clone)"));
        }
        else if (GameObject.Find("Void_Big_Slime(Clone)") != null){
            Destroy(GameObject.Find("Void_Big_Slime(Clone)"));
        }
        powerUps.ResetPowerUps();
        shouldSpawnHoly = true;
        Instantiate(Resources.Load<GameObject>("Prefabs/Countdown"), uiCanvas.transform).SetActive(true);
    }

    public void NextStage(){
        playerCrosshair.ShowCrosshair();
        numberOfHolyOrbs = 0;
        numberOfVoidOrbs = 0;
        holyOrbText.text = numberOfHolyOrbs.ToString();
        voidOrbText.text = numberOfVoidOrbs.ToString();
        numberOfDepositedHolyOrbs = 0;
        numberOfDepositedVoidOrbs = 0;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PowerUpTile")){
            Destroy(go);
        }
        IncreaseDifficulty();
        
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        vigTimer = 0;

        //new arena
        slimeSpawners[arenaIndex].spawnerIsActive = false;
        arenaIndex = UnityEngine.Random.Range(0, amountOfArenas);
        slimeSpawners[arenaIndex].spawnerIsActive = true;

        holyDepositText[arenaIndex].text = "0/" + Math.Truncate(numberOfOrbsToCollect).ToString();
        voidDepositText[arenaIndex].text = "0/" + Math.Truncate(numberOfOrbsToCollect).ToString();
        yOffset = -((arenaIndex * 100) + 1); //if 2 => -201
        playerController.gameObject.transform.position = new Vector3(0, yOffset, 0);
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
        if (GameObject.Find("Holy_Big_Slime(Clone)") != null){
            Destroy(GameObject.Find("Holy_Big_Slime(Clone)"));
        }
        else if (GameObject.Find("Void_Big_Slime(Clone)") != null){
            Destroy(GameObject.Find("Void_Big_Slime(Clone)"));
        }
        shouldSpawnHoly = true;
        Instantiate(Resources.Load<GameObject>("Prefabs/Countdown"), uiCanvas.transform).SetActive(true);
    }

    private void IncreaseDifficulty(){
        //numberOfOrbsToCollect += 1f;
        slimeSpawners[arenaIndex].IncreaseSlimeDifficulty();

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
            if (numberOfHolyOrbs - 1 >= 0){
                numberOfHolyOrbs--;
                holyOrbText.text = numberOfHolyOrbs.ToString();
            }
            if (numberOfHolyOrbs < 0){
                numberOfHolyOrbs = 0;
                
            }
            
        }
        else{
            if (numberOfVoidOrbs - 1 >= 0){
                numberOfVoidOrbs--;
                voidOrbText.text = numberOfVoidOrbs.ToString();
            }
            if (numberOfVoidOrbs < 0){
                numberOfVoidOrbs = 0;
            }
        }
    }

    public void IncreaseDepositedOrbCount(string colour){
        if (colour == "Holy"){
            numberOfDepositedHolyOrbs++;
            if (numberOfDepositedHolyOrbs <= Math.Truncate(numberOfOrbsToCollect)){
                holyDepositText[arenaIndex].text = numberOfDepositedHolyOrbs.ToString() + "/"+ Math.Truncate(numberOfOrbsToCollect).ToString();
            }
        }
        else{
            numberOfDepositedVoidOrbs++;
            if (numberOfDepositedVoidOrbs <= numberOfOrbsToCollect){
                voidDepositText[arenaIndex].text = numberOfDepositedVoidOrbs.ToString() + "/" + Math.Truncate(numberOfOrbsToCollect).ToString();
            }
        }
        if (numberOfDepositedHolyOrbs >= Math.Truncate(numberOfOrbsToCollect) && numberOfDepositedVoidOrbs >= Math.Truncate(numberOfOrbsToCollect) && !gameWonScreen.activeSelf){
            //win
            if (PlayerPrefs.HasKey("GamesWon")){
                int num = PlayerPrefs.GetInt("GamesWon");
                num++;
                PlayerPrefs.SetInt("GamesWon", num);
            }
            GameWon();
            
            
        }
    }
}
