using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip[] soundEffects;

    public List<AudioSource> playingSounds = new List<AudioSource>();

    public static SoundEffects Instance {get; private set;}

    private void Awake() {
        Instance = this;
    }
    public AudioClip[] GetAllSoundEffects(){
        return soundEffects;
    }

    public void PlaySound(string name){
        foreach(var sound in soundEffects){
            if (sound.name == name){
                AudioSource go = Instantiate(Resources.Load<GameObject>("Sounds/SelfDeletingAudio")).GetComponent<AudioSource>();
                playingSounds.Add(go);
                go.clip = sound;
                go.Play();
            }
        }
    }

    public void StopSound(string name){
        foreach (AudioSource sound in playingSounds){
            if (sound.clip.name == name){
                sound.Stop();
                playingSounds.Remove(sound);
            }
        }
    }
}
