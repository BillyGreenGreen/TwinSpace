using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    private float timeBetweenSpawning = 2f; //2
    private float timer = 0;
    private GameObject[] spawnPoints;
    private float slimeFireRate = 1f; //1
    private float slimeMoveSpeed = 4; //4
    private float slimeRotateAimSpeed = 1; //1
    private int chanceForBigSlime = 5;
    private int stageForBigSlime = 3;

    private void Update() {
        if (GameManager.Instance.isGamePlaying){
            timer += Time.deltaTime;
            int randomPoint = Random.Range(0, 4);
            if (timer > timeBetweenSpawning){
                if (GameManager.Instance.shouldSpawnHoly){
                    //spawn holy mobs
                    float randomNum = Random.Range(0f, 100f);
                    if (GameManager.Instance.stage == stageForBigSlime && randomNum < chanceForBigSlime && GameObject.Find("Holy_Big_Slime(Clone)") == null && GameObject.Find("Void_Big_Slime(Clone)") == null){
                        //Instantiate(Resources.Load<GameObject>("Prefabs/VFX/Shockwave"), new Vector2(0,0), Quaternion.identity);
                        Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Holy_Big_Slime"), new Vector2(-0.45f, 32.4f), Quaternion.identity);
                    }

                    spawnPoints = GameManager.Instance.holySpawnPoints;
                    GameObject slime = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Holy_Slime"), spawnPoints[randomPoint].transform.position, Quaternion.identity);
                    SlimeAI slimeAI = slime.GetComponent<SlimeAI>();
                    slimeAI.fireRate = slimeFireRate;
                    slimeAI.speed = slimeMoveSpeed;
                    slimeAI.rotateSpeed = slimeRotateAimSpeed;
                    GameManager.Instance.holyEnemies.Add(slime);

                }
                else{
                    float randomNum = Random.Range(0f, 100f);
                    if (GameManager.Instance.stage == stageForBigSlime && randomNum < chanceForBigSlime && GameObject.Find("Holy_Big_Slime(Clone)") == null && GameObject.Find("Void_Big_Slime(Clone)") == null){
                        //Instantiate(Resources.Load<GameObject>("Prefabs/VFX/Shockwave"), new Vector2(80,0), Quaternion.identity);
                        Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Void_Big_Slime"), new Vector2(79.45f, 32.4f), Quaternion.identity);
                    }
                    //spawn void mobs
                    spawnPoints = GameManager.Instance.voidSpawnPoints;
                    GameObject slime = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Void_Slime"), spawnPoints[randomPoint].transform.position, Quaternion.identity);
                    SlimeAI slimeAI = slime.GetComponent<SlimeAI>();
                    slimeAI.fireRate = slimeFireRate;
                    slimeAI.speed = slimeMoveSpeed;
                    slimeAI.rotateSpeed = slimeRotateAimSpeed;
                    GameManager.Instance.holyEnemies.Add(slime);
                }
                timer = 0;
            }
        }
        
        
    }

    public void IncreaseSlimeDifficulty(){
        //should be hard at wave 20 difficulty
        timeBetweenSpawning -= 0.075f; //0.5 with wave 20
        slimeFireRate -= 0.04f; //0.2 with wave 20
        slimeMoveSpeed += 0.1f; //6 with wave 20
        slimeRotateAimSpeed += 0.45f; //10 with wave 20
        
    }
}
