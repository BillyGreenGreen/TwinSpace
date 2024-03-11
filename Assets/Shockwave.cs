using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    private Material material;

    private void Start() {
        material = GetComponent<SpriteRenderer>().material;
        material.SetInt("_ShockwaveOn", 1);
        material.SetFloat("_ShockwaveTimer", Time.time);
        //Debug.Log(material.GetInt("_ShockwaveOn"));
        Destroy(gameObject, 1);
    }
}
