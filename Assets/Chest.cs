using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class Chest : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Light2D chestLight;
    bool isOpen = false;
    int numberOfOrbsToSpawn;
    Sequence s;

    private void Start() {
        StartShining();
    }

    void StartShining(){
        s = DOTween.Sequence();
        s.Append(DOVirtual.Float(0.3f, 0.6f, 0.5f, v => {
            chestLight.intensity = v;
        }).SetLoops(-1, LoopType.Yoyo));
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (!isOpen && other.CompareTag("Player")){
            isOpen = true;
            if (s.active){
                s.Kill();
            }
            
            sprite.sprite = Resources.Load<Sprite>("chestOpen");
            
            //xOffset random between -1 and 1;
            numberOfOrbsToSpawn = Random.Range(2, 5);
            InvokeRepeating("SpawnOrbs", 0f, 0.1f);
        }
    }

    void SpawnOrbs(){
        float yOffset = 0.4f;
        //float randomXOffset = Random.Range(-1f, 1f);
        float randomXOffset = 0;
        if (GameManager.Instance.shouldSpawnHoly){
            Instantiate(Resources.Load<GameObject>("Prefabs/Orbs/HolyOrb"), new Vector2(transform.position.x + randomXOffset, transform.position.y + yOffset), Quaternion.identity);
        }
        else{
            Instantiate(Resources.Load<GameObject>("Prefabs/Orbs/VoidOrb"), new Vector2(transform.position.x + randomXOffset, transform.position.y + yOffset), Quaternion.identity);
        }
        numberOfOrbsToSpawn--;
        //Debug.Log(numberOfOrbsToSpawn);
        if (numberOfOrbsToSpawn == 0){
            CancelInvoke();
        }
    }

    public void ResetChest(){
        if (isOpen){
            isOpen = false;
            sprite.sprite = Resources.Load<Sprite>("chestClosed");
            StartShining();
        }
    }
}
