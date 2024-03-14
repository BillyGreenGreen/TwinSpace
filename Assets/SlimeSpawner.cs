using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    private float timeBetweenSpawning = 2f; //2
    private float timer = 0;
    public bool spawnerIsActive = false;
    public GameObject[] holySpawnPoints;
    public GameObject[] voidSpawnPoints;
    private GameObject[] spawnPoints;
    private float slimeFireRate = 1f; //1
    private float slimeMoveSpeed = 4; //4
    private float slimeRotateAimSpeed = 1; //1
    private float randomMinHealth = 1;
    private float randomMaxHealth = 3;
    private int chanceForBigSlime = 0;
    private int stageForBigSlime = 1;

    private void Update() {
        if (GameManager.Instance.isGamePlaying && spawnerIsActive){
            timer += Time.deltaTime;
            int randomPoint = Random.Range(0, holySpawnPoints.Length);
            if (timer > timeBetweenSpawning){
                if (GameManager.Instance.shouldSpawnHoly){
                    //spawn holy mobs
                    float randomNum = Random.Range(0f, 100f);
                    if (GameManager.Instance.stage >= stageForBigSlime && randomNum < chanceForBigSlime && GameObject.Find("Holy_Big_Slime(Clone)") == null && GameObject.Find("Void_Big_Slime(Clone)") == null){
                        Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Holy_Big_Slime"), new Vector2(-0.45f, 32f + GameManager.Instance.yOffset), Quaternion.identity);
                    }

                    spawnPoints = holySpawnPoints;
                    GameObject slime = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Holy_Slime"), spawnPoints[randomPoint].transform.position, Quaternion.identity);
                    SlimeAI slimeAI = slime.GetComponent<SlimeAI>();
                    int randomHealth = Random.Range((int)randomMinHealth, (int)randomMaxHealth);
                    slimeAI.slimeHealth = randomHealth;
                    slimeAI.fireRate = slimeFireRate;
                    slimeAI.speed = slimeMoveSpeed;
                    slimeAI.rotateSpeed = slimeRotateAimSpeed;
                    GameManager.Instance.holyEnemies.Add(slime);

                }
                else{
                    float randomNum = Random.Range(0f, 100f);
                    if (GameManager.Instance.stage >= stageForBigSlime && randomNum < chanceForBigSlime && GameObject.Find("Holy_Big_Slime(Clone)") == null && GameObject.Find("Void_Big_Slime(Clone)") == null){
                        Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Void_Big_Slime"), new Vector2(79.45f, 32f + GameManager.Instance.yOffset), Quaternion.identity);
                    }
                    //spawn void mobs
                    spawnPoints = voidSpawnPoints;
                    GameObject slime = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Void_Slime"), spawnPoints[randomPoint].transform.position, Quaternion.identity);
                    SlimeAI slimeAI = slime.GetComponent<SlimeAI>();
                    int randomHealth = Random.Range((int)randomMinHealth, (int)randomMaxHealth);
                    slimeAI.slimeHealth = randomHealth;
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
        //should be IMPOSSIBLE at wave 20 difficulty
        timeBetweenSpawning -= 0.0875f; //0.25 with wave 20
        slimeFireRate -= 0.04f; //0.2 with wave 20
        slimeMoveSpeed += 0.1f; //6 with wave 20
        slimeRotateAimSpeed += 0.45f; //10 with wave 20
        randomMinHealth += 0.3f; //7 with wave 20
        randomMaxHealth += 0.35f; //10 with wave 20
    }
}
