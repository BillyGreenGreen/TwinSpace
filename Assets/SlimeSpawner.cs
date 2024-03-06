using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [SerializeField] private string nameOfSpawner;
    private float timeBetweenSpawning = 2;
    private float timer = 0;
    private GameObject[] spawnPoints;

    private void Start() {
    }

    private void Update() {
        timer += Time.deltaTime;
        int randomPoint = Random.Range(0, 4);
        if (timer >= timeBetweenSpawning){
            if (GameManager.Instance.shouldSpawnHoly){
                //spawn holy mobs
                spawnPoints = GameManager.Instance.holySpawnPoints;
                GameManager.Instance.holyEnemies.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Holy_Slime"), spawnPoints[randomPoint].transform.position, Quaternion.identity));
            }
            else{
                //spawn void mobs
                spawnPoints = GameManager.Instance.voidSpawnPoints;
                GameManager.Instance.voidEnemies.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Void_Slime"), spawnPoints[randomPoint].transform.position, Quaternion.identity));
            }
            timer = 0;
        }
        
    }
}
