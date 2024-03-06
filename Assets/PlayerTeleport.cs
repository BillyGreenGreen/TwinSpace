using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PlayerTeleport : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    public Volume volume;
    private static ChromaticAberration ca;
    private static LensDistortion ld;
    string side = "holy";
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
        if (Input.GetKeyDown(KeyCode.F)){
            timerToDamp += Time.deltaTime;
            ps1.Play();
            ps2.Play();

            
            if (side == "holy"){
                //ANIMATION HERE LIKE LAST EPOCH WHEN YOU SWITCH AND CHANGE CHROMATIC ABERRATION QUICK UP AND DOWN
                //MAYBE WE DONT NEED TO HAVE THE CAMERA DAMPENING SET TO 0 IF WE HAVE AN ANIMATION TO HIDE IT MOVING
                //cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0;
                //cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 0;
                transform.position = new Vector2(transform.position.x + 80, transform.position.y);
                side = "void";
                SoundEffects.Instance.PlaySound("woosh1");
            }
            else{
                //cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0;
                //cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 0;
                transform.position = new Vector2(transform.position.x - 80, transform.position.y);
                side = "holy";
                SoundEffects.Instance.PlaySound("woosh2");
            }
            
        }
    }
}
