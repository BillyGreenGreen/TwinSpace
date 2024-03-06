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
            target = GameObject.Find("HolyBlackHole").transform;
        }
        else{
            target = GameObject.Find("VoidBlackHole").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null){
            if (orbColour == "Holy"){
                target = GameObject.Find("HolyBlackHole").transform;
            }
            else{
                target = GameObject.Find("VoidBlackHole").transform;
            }
            transform.position = Vector2.MoveTowards(transform.position, target.position, 6f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.name);
        if (orbColour == "Holy"){
            GameManager.Instance.IncreaseDepositedOrbCount("Holy");
        }
        else{
            GameManager.Instance.IncreaseDepositedOrbCount("Void");
        }
        Destroy(gameObject);
    }
}
