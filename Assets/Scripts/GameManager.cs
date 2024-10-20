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
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public PlayerCrosshair playerCrosshair;
    public PlayerController playerController;
    public Volume volume;
    public VolumeProfile defaultVolumeProfile;
    public VolumeProfile stormVolumeProfile;
    [SerializeField] private GameObject stormFlashes;
    [SerializeField] private GameObject rain;
    [SerializeField] private Light2D defaultLight;
    [SerializeField] private Light2D stormLight;
    public EventSystem eventSystem;
    public SlimeSpawner[] slimeSpawners;
    PlayerInputActions playerInputActions;
    public PowerUps powerUps;
    public Canvas uiCanvas;
    public bool isGamePlaying = false;
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;
    public GameObject pauseScreen;
    public GameObject gameOverScreenFirstButton;
    public GameObject gameWonScreenFirstButton;
    public GameObject pauseScreenFirstButton;
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
    [SerializeField] private GameObject[] powerUpTiles;
    

    [Header("Player Stats")]
    public int numberOfHolyOrbs;
    public int numberOfVoidOrbs;
    public int numberOfDepositedHolyOrbs;
    public int numberOfDepositedVoidOrbs;
    public int playerHealth;
    public bool dashUpgradeLevelAllowsNoDamage = false;
    private float numberOfOrbsToCollect = 5;
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
        playerInputActions = new PlayerInputActions();
        if (!playerInputActions.Player.enabled){
            playerInputActions.Player.Enable();
        }
        playerInputActions.Player.Pause.performed += PauseGame;
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
        if (!isGamePlaying){
            BGM.Play();
            isGamePlaying = true;
            pauseScreen.SetActive(false);
            playerCrosshair.ShowCrosshair();
        }
        
    }

    private void PauseGame(InputAction.CallbackContext ctx){
        if (isGamePlaying){
            isGamePlaying = false;
            pauseScreen.SetActive(true);
            BGM.Pause();
            playerCrosshair.HideCrosshair();
            eventSystem.firstSelectedGameObject = pauseScreenFirstButton;
            eventSystem.SetSelectedGameObject(pauseScreenFirstButton);
        }
        
    }

    public void KillPlayer(){
        if (isGamePlaying){
            isGamePlaying = false;
            gameOverScreen.SetActive(true);
            string stagesPlural;
            if (stage == 1){
                stagesPlural = "STAGE";
            }
            else{
                stagesPlural = "STAGES";
            }
            gameOverScreen.transform.Find("TitleStages").GetComponent<TextMeshProUGUI>().text = string.Format("YOU SURVIVED {0} {1}.", stage, stagesPlural);
            BGM.Stop();
            playerCrosshair.HideCrosshair();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Orb")){
                Destroy(go);
            }
            eventSystem.firstSelectedGameObject = gameOverScreenFirstButton;
            eventSystem.SetSelectedGameObject(gameOverScreenFirstButton);
        }   
        
    }

    private void GameWon(){
        isGamePlaying = false;
        gameWonScreen.SetActive(true);
        
        gameWonScreen.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "STAGE " + stage + " COMPLETE";
        stage++;
        //PlayerPrefs.SetInt("StageSaved", stage);
        //Debug.Log(powerUps.GeneratePowerUpsToBuy().Count);
        int count = 0;
        foreach (string s in powerUps.GeneratePowerUpsToBuy()){
            string nameOfPowerUp = s.Split(" ")[0];
            string levelOfPowerUp;
            try{
                levelOfPowerUp = s.Split(" ")[1];
            }catch{
                levelOfPowerUp = "";
            }
            
            //Debug.Log(nameOfPowerUp + levelOfPowerUp);
            GameObject go = powerUpTiles[count];
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
            count++;
            
            
        }
        eventSystem.firstSelectedGameObject = gameWonScreenFirstButton;
        eventSystem.SetSelectedGameObject(gameWonScreenFirstButton);
        playerCrosshair.HideCrosshair();
        
        BGM.Stop();
    }

    public void DecreasePlayerHealth(int damage){
        if (dashUpgradeLevelAllowsNoDamage){
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
        else{
            if (isGamePlaying){
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
        
    }

    public void IncreasePlayerHealth(int healAmount){
        playerHealth += healAmount;
        healthSlider.value = playerHealth;
        healthText.text = string.Format("{0}/{1}", playerHealth.ToString(), healthSlider.maxValue.ToString());
        if (playerHealth > healthSlider.maxValue){
            playerHealth = (int)healthSlider.maxValue;
        }
    }

    void ChangeSceneProfile(int _arenaIndex){
        if (_arenaIndex == 2){
            volume.profile = stormVolumeProfile;
            stormFlashes.SetActive(true);
            rain.SetActive(true);
            defaultLight.gameObject.SetActive(false);
            stormLight.gameObject.SetActive(true);
        }
        else{
            volume.profile = defaultVolumeProfile;
            stormFlashes.SetActive(false);
            rain.SetActive(false);
            stormLight.gameObject.SetActive(false);
            defaultLight.gameObject.SetActive(true);
        }
    }

    public void ResetGame(){
        playerCrosshair.ShowCrosshair();
        ResetDifficulty();
        numberOfHolyOrbs = 0;
        numberOfVoidOrbs = 0;
        holyOrbText.text = numberOfHolyOrbs.ToString();
        voidOrbText.text = numberOfVoidOrbs.ToString();
        numberOfDepositedHolyOrbs = 0;
        numberOfDepositedVoidOrbs = 0;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PowerUpTile")){
            //Destroy(go);
        }
        playerHealth = (int)healthSlider.maxValue;
        healthSlider.value = playerHealth;
        healthText.text = string.Format("{0}/{1}", playerHealth.ToString(), healthSlider.maxValue.ToString());
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        vigTimer = 0;
        stage = 0;
        slimeSpawners[arenaIndex].spawnerIsActive = false;
        arenaIndex = UnityEngine.Random.Range(0, amountOfArenas);
        slimeSpawners[arenaIndex].spawnerIsActive = true;
        //ChangeSceneProfile(arenaIndex);
        holyDepositText[arenaIndex].text = "0/" + Math.Truncate(numberOfOrbsToCollect).ToString();
        voidDepositText[arenaIndex].text = "0/" + Math.Truncate(numberOfOrbsToCollect).ToString();
        yOffset = -((arenaIndex * 100) + 1); //if 2 => -201
        playerController.gameObject.transform.position = new Vector3(0, yOffset, 0);
        //GameObject.Find("Player").transform.position = new Vector3(0, -1f, 0);

        foreach (GameObject chest in GameObject.FindGameObjectsWithTag("Chest")){
            chest.GetComponent<Chest>().ResetChest();
        }
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
        foreach (GameObject orb in GameObject.FindGameObjectsWithTag("Orb")){
            if (orb.layer != 12){
                Destroy(orb);
            }  
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
            //Destroy(go);
        }
        IncreaseDifficulty();
        
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);
        vigTimer = 0;

        //new arena
        slimeSpawners[arenaIndex].spawnerIsActive = false;
        arenaIndex = UnityEngine.Random.Range(0, amountOfArenas);
        Debug.Log(arenaIndex);
        slimeSpawners[arenaIndex].spawnerIsActive = true;
        //ChangeSceneProfile(arenaIndex);

        foreach (GameObject orb in GameObject.FindGameObjectsWithTag("Orb")){
            Destroy(orb);
        }

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
        numberOfOrbsToCollect += 1f;
        slimeSpawners[arenaIndex].IncreaseSlimeDifficulty();
    }

    private void ResetDifficulty(){
        numberOfOrbsToCollect = 5;
        foreach (SlimeSpawner spawner in slimeSpawners){
            spawner.ResetSlimeDifficulty();
        }
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
