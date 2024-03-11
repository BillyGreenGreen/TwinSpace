using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PlayerTeleport : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private TrailRenderer tr;
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    public Volume volume;
    private static ChromaticAberration ca;
    private static LensDistortion ld;
    public Animator holyVortexAnim;
    public Animator voidVortexAnim;
    [SerializeField] private GameObject tpTooltip;
    float timerToDamp = 0;
    float dampDuration = 0.2f;
    private void Start() {
        volume.profile.TryGet(out ld);
        volume.profile.TryGet(out ca);
    }
    // Update is called once per frame
    void Update()
    {
        
        if (timerToDamp > 0){
            timerToDamp += Time.deltaTime;
            if (timerToDamp < dampDuration){
                ld.intensity.value = Mathf.Lerp(-1, 0, timerToDamp / dampDuration);
                ca.intensity.value = Mathf.Lerp(1, 0, timerToDamp / dampDuration);
            }
            if (timerToDamp >= dampDuration){
                //IF WE WANT DAMPING
                //cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0.5f;
                //cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 0.5f;
                timerToDamp = 0;
            }
        }
        if (GameManager.Instance.isGamePlaying){
            if (Input.GetKeyDown(KeyCode.F)){
                if (tr.emitting){
                    tr.emitting = false;
                }
                timerToDamp += Time.deltaTime;
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("Orb")){
                    Destroy(go);
                }
                ps1.Play();
                ps2.Play();
                GameManager.Instance.vigTimer = 0;
                
                if (GameManager.Instance.shouldSpawnHoly){
                    transform.position = new Vector2(transform.position.x + 80, transform.position.y);
                    voidVortexAnim.Play("Void_Vortex", 0, 0f);
                    SoundEffects.Instance.PlaySound("woosh1");
                    if (GameManager.Instance.holyEnemies.Count > 0){
                        foreach(GameObject go in GameManager.Instance.holyEnemies){
                            Destroy(go);
                        }
                        GameManager.Instance.holyEnemies.Clear();
                    }
                    GameManager.Instance.shouldSpawnHoly = false;
                    if (tpTooltip != null){
                        Destroy(tpTooltip);
                    }
                }
                else{
                    transform.position = new Vector2(transform.position.x - 80, transform.position.y);
                    holyVortexAnim.Play("Holy_Vortex", 0, 0f);
                    SoundEffects.Instance.PlaySound("woosh2");
                    if (GameManager.Instance.voidEnemies.Count > 0){
                        foreach(GameObject go in GameManager.Instance.voidEnemies){
                            Destroy(go);
                        }
                        GameManager.Instance.voidEnemies.Clear();
                    }
                    GameManager.Instance.shouldSpawnHoly = true;
                }
                
            }
        }
        
    }
}
