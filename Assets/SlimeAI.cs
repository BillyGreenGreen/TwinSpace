using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public int slimeHealth;
    public int randomMaxHealth = 4;
    private GameObject playerPosition;
    public float speed;
    private string gameObjectName;
    private bool shouldMove = true;

    public Transform target;
    public float rotateSpeed = 1f;
    private Rigidbody2D rb;

    public float distanceToShoot = 5f;
    public float distanceToStop = 3f;

    public Animator anim;

    public float fireRate;
    private float timeToFire;

    public GameObject projectilePrefab;

    public Transform firingPointParent;
    public Transform firingPoint;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (GameObject.FindGameObjectWithTag("Player")){
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        int randomHealth = Random.Range(1, randomMaxHealth);
        slimeHealth = randomHealth;
        gameObjectName = gameObject.name;
    }

    private void FixedUpdate() {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGamePlaying){
            if (shouldMove){
                if (Vector2.Distance(target.position, transform.position) >= distanceToStop){
                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                    Vector2 dir = transform.position - target.position;
                    if (dir.x > 0){
                        GetComponent<SpriteRenderer>().flipX = true;
                    }
                    else{
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    anim.SetBool("isMoving", true);
                    anim.SetBool("isRangedAttacking", false);
                }
                else{
                    rb.velocity = Vector2.zero;
                    anim.SetBool("isMoving", false);
                }
                if (Vector2.Distance(target.position, transform.position) <= distanceToShoot){
                    anim.SetBool("isRangedAttacking", true);
                    int rng = Random.Range(0, 100);
                    if (rng <= 65){
                        ShootOne();
                    }
                    else if(rng > 65 && rng <= 90){
                        ShootThree();
                    }
                    else if(rng > 90){
                        ShootFive();
                    }
                }
            }
        }
        
        
    }

    private void ShootOne(){
        if (timeToFire <= 0f){
            Vector3 rotation = target.position - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg -90f;
            firingPointParent.rotation = Quaternion.Euler(0, 0, rotZ);

            Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
            timeToFire = fireRate;
        }
        else{
            timeToFire -= Time.deltaTime;
        }
    }

    private void ShootThree(){
        if (timeToFire <= 0f){
            Vector3 rotation = target.position - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg -90f;
            firingPointParent.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 10f;
            for (int i = 0; i < 3; i++){
                var shotRotation = firingPointParent.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Instantiate(projectilePrefab, firingPoint.position, shotRotation);
                angle -= 10f;
            }
            
            timeToFire = fireRate;
        }
        else{
            timeToFire -= Time.deltaTime;
        }
    }

    private void ShootFive(){
        if (timeToFire <= 0f){
            Vector3 rotation = target.position - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg -90f;
            firingPointParent.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 30f;
            for (int i = 0; i < 5; i++){
                var shotRotation = firingPointParent.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Instantiate(projectilePrefab, firingPoint.position, shotRotation);
                angle -= 15f;
            }
            
            timeToFire = fireRate;
        }
        else{
            timeToFire -= Time.deltaTime;
        }
    }
    public void Die(){
        if (gameObjectName.Contains("Holy")){
            Instantiate(Resources.Load<GameObject>("Prefabs/Orbs/HolyOrb"), transform.position, Quaternion.identity);
        }
        else{
            Instantiate(Resources.Load<GameObject>("Prefabs/Orbs/VoidOrb"), transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player"){
            //stop and shoot
            shouldMove = false;
        }
    }
}
