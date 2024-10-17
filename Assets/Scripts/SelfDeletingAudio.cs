using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeletingAudio : MonoBehaviour
{
    AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        //this is where we get the volume for sound effects from settings menu scriptable object or playerprefs
        //playerprefs probably easier
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying){
            SoundEffects.Instance.playingSounds.Remove(audioSource);
            Destroy(gameObject);
        }
    }
}
