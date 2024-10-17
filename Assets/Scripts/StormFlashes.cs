using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class StormFlashes : MonoBehaviour
{
    [SerializeField] private Light2D stormFlashLight1;
    [SerializeField] private Light2D stormFlashLight2;
    float timer = 0;
    bool disabled = true;
    // Start is called before the first frame update
    void Start()
    {
        //ENABLE AND DISABLE FOR FUNCTIONALITY
    }

    private void OnEnable() {
        disabled = false;
        stormFlashLight1.intensity = 0;
        stormFlashLight2.intensity = 0;
        Invoke("Flash", 0.5f);
        SoundEffects.Instance.PlaySound("rain");
    }

    private void OnDisable() {
        disabled = true;
        SoundEffects.Instance.StopSound("rain");
        SoundEffects.Instance.StopSound("thunderclap1");
    }

    void Flash(){
        if (!disabled){
            float randomTime = Random.Range(10, 15);
            float randomThunder = Random.Range(1, 3);
            SoundEffects.Instance.PlaySound("thunderclap1");
            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOVirtual.Float(0, 0.4f, 0.08f, v => {
                stormFlashLight1.intensity = v;
                stormFlashLight2.intensity = v;
            }));
            sequence.Append(DOVirtual.Float(0.4f, 0f, 0.08f, v => {
                stormFlashLight1.intensity = v;
                stormFlashLight2.intensity = v;
            }));
            sequence.Append(DOVirtual.Float(0, 0.4f, 0.08f, v => {
                stormFlashLight1.intensity = v;
                stormFlashLight2.intensity = v;
            }));
            sequence.Append(DOVirtual.Float(0.4f, 0f, 0.08f, v => {
                stormFlashLight1.intensity = v;
                stormFlashLight2.intensity = v;
            }));
            Invoke("Flash", randomTime);
        }
        
    }
}
