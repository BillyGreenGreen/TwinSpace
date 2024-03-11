using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbDeposit : MonoBehaviour
{
    public Transform target;
    public string orbColour;
    // Start is called before the first frame update
    void Start()
    {
        if (orbColour == "Holy"){
            target = GameObject.Find("HolyBlackHole" + GameManager.Instance.arenaIndex.ToString()).transform;
        }
        else{
            target = GameObject.Find("VoidBlackHole" + GameManager.Instance.arenaIndex.ToString()).transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isGamePlaying){
            target = null;
        }
        if (target != null){
            if (orbColour == "Holy"){
                target = GameObject.Find("HolyBlackHole" + GameManager.Instance.arenaIndex.ToString()).transform;
            }
            else{
                target = GameObject.Find("VoidBlackHole" + GameManager.Instance.arenaIndex.ToString()).transform;
            }
            transform.position = Vector2.MoveTowards(transform.position, target.position, 6f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //HEALING
        if (orbColour == "Holy"){
            GameManager.Instance.IncreaseDepositedOrbCount("Holy");
            //GameManager.Instance.IncreasePlayerHealth(2);
        }
        else{
            GameManager.Instance.IncreaseDepositedOrbCount("Void");
            //GameManager.Instance.IncreasePlayerHealth(2);
        }
        Destroy(gameObject);
    }
}
