using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public Volume volume;
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
    public int numberOfHolyOrbs;
    public int numberOfVoidOrbs;
    public int numberOfDepositedHolyOrbs;
    public int numberOfDepositedVoidOrbs;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (vigTimer < vigTimerDuration){
            vigTimer += Time.deltaTime;
            vg.intensity.value = Mathf.Lerp(0, 0.6f, vigTimer / vigTimerDuration);
            TimeSpan timeS = TimeSpan.FromSeconds(vigTimerDuration - vigTimer);
            timerText.text = timeS.ToString(@"m\:ss");
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
        }
    }
}
