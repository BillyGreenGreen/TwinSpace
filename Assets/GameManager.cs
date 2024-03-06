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
    private static ChromaticAberration ca;
    private static LensDistortion ld;
    private static Vignette vg;
    public float vigTimer = 0;
    private float vigTimerDuration = 30;

    [SerializeField] private TextMeshProUGUI timerText;
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
            vg.intensity.value = Mathf.Lerp(0, 0.7f, vigTimer / vigTimerDuration);
            TimeSpan timeS = TimeSpan.FromSeconds(vigTimerDuration - vigTimer);
            timerText.text = timeS.ToString(@"m\:ss");
        }
    }
}
