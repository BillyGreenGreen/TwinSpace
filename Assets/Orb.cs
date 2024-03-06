using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public Transform target;
    public string orbColour;
    
    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log(other.name);
        if (other.tag == "OrbPickupRange"){
            target = GameObject.Find("Player").transform;
        }
        if (other.name == "Player"){
            if (orbColour == "Holy"){
                GameManager.Instance.IncreaseOrbCount("Holy");
            }
            else{
                GameManager.Instance.IncreaseOrbCount("Void");
            }
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (target != null){
            target = GameObject.Find("Player").transform;
            transform.position = Vector2.MoveTowards(transform.position, target.position, 6f * Time.deltaTime);
        }
    }
}
