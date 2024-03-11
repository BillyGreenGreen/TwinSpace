using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Vector3 mousePos;
    [SerializeField] private GameObject holyBullet;
    [SerializeField] private GameObject voidBullet;
    [SerializeField] private Transform bulletTransform;
    [SerializeField] private float bulletForce;
    [SerializeField] PowerUps powerUps;
    public Rigidbody2D playerRb;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;
    public int amountOfBulletsToFire;
    public bool canPulse = false;
    public float pulseTime;
    float pulseTimer = 0;
    float shakeIntensity = 2, duration = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGamePlaying){
            var pos = Input.mousePosition;
            pos.z = 1;
            mousePos = Camera.main.ScreenToWorldPoint(pos);

            Vector3 rotation = mousePos - transform.position;

            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            //transform.rotation = Quaternion.Euler(0,0,rotZ);
            if (!canFire){
                timer += Time.deltaTime;
                if (timer > timeBetweenFiring){
                    canFire = true;
                    timer = 0;
                }
            }
            if (Input.GetMouseButton(0) && canFire){
                canFire = false;
                if (GameManager.Instance.shouldSpawnHoly){
                    if (amountOfBulletsToFire == 1){
                        ShootOne("Void");
                    }
                    else if (amountOfBulletsToFire == 3){
                        ShootThree("Void");
                    }
                    else if (amountOfBulletsToFire == 5){
                        ShootFive("Void");
                    }
                    else if (amountOfBulletsToFire == 7){
                        ShootSeven("Void");
                    }
                }
                else{
                    if (amountOfBulletsToFire == 1){
                        ShootOne("Holy");
                    }
                    else if (amountOfBulletsToFire == 3){
                        ShootThree("Holy");
                    }
                    else if (amountOfBulletsToFire == 5){
                        ShootFive("Holy");
                    }
                    else if (amountOfBulletsToFire == 7){
                        ShootSeven("Holy");
                    }
                    
                }
                
            }
            if (canPulse){
                pulseTimer += Time.deltaTime;
                if (pulseTimer >= pulseTime){
                    float randomNum = Random.Range(0f, 100f);
                    if (powerUps.AOEPulseUpgradeLevel == 1 && randomNum < 5f){
                        if (GameManager.Instance.shouldSpawnHoly){
                            ShootAOEPulse("Void");
                        }
                        else{
                            ShootAOEPulse("Holy");
                        }
                    }
                    else if (powerUps.AOEPulseUpgradeLevel == 2 && randomNum < 10f){
                        if (GameManager.Instance.shouldSpawnHoly){
                            ShootAOEPulse("Void");
                        }
                        else{
                            ShootAOEPulse("Holy");
                        }
                    }
                    else if (powerUps.AOEPulseUpgradeLevel == 3 && randomNum < 10f){
                        if (GameManager.Instance.shouldSpawnHoly){
                            ShootAOEPulse("Void");
                        }
                        else{
                            ShootAOEPulse("Holy");
                        }
                    }
                    else if (powerUps.AOEPulseUpgradeLevel == 4 && randomNum < 15f){
                        if (GameManager.Instance.shouldSpawnHoly){
                            ShootAOEPulse("Void");
                        }
                        else{
                            ShootAOEPulse("Holy");
                        }
                    }
                    else if (powerUps.AOEPulseUpgradeLevel == 5 && randomNum < 15f){
                        if (GameManager.Instance.shouldSpawnHoly){
                            ShootAOEPulse("Void");
                        }
                        else{
                            ShootAOEPulse("Holy");
                        }
                    }
                    else if (powerUps.AOEPulseUpgradeLevel == 6 && randomNum < 20f){
                        if (GameManager.Instance.shouldSpawnHoly){
                            ShootAOEPulse("Void");
                        }
                        else{
                            ShootAOEPulse("Holy");
                        }
                    }
                    else if (powerUps.AOEPulseUpgradeLevel == 7 && randomNum < 25f){
                        if (GameManager.Instance.shouldSpawnHoly){
                            ShootAOEPulse("Void");
                        }
                        else{
                            ShootAOEPulse("Holy");
                        }
                    }
                    else if (powerUps.AOEPulseUpgradeLevel == 8 && randomNum < 30f){
                        if (GameManager.Instance.shouldSpawnHoly){
                            ShootAOEPulse("Void");
                        }
                        else{
                            ShootAOEPulse("Holy");
                        }
                    }
                    pulseTimer = 0;
                }
            }

        }
        
    }

    private void ShootOne(string colour){
        if (colour == "Holy"){
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            Instantiate(holyBullet, bulletTransform.position, bulletTransform.rotation);
        }
        else{
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            Instantiate(voidBullet, bulletTransform.position, bulletTransform.rotation);
        }
    }

    private void ShootTwo(string colour){
        if (colour == "Holy"){
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 5f;
            for (int i = 0; i < 2; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Instantiate(holyBullet, bulletTransform.position, shotRotation);
                angle -= 10f;
            }
        }
        else{
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 5f;
            for (int i = 0; i < 2; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                //transform.rotation = shotRotation;
                Instantiate(voidBullet, bulletTransform.position, shotRotation);
                angle -= 10f;
            }
        }
    }

    private void ShootThree(string colour){
        if (colour == "Holy"){
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 10f;
            for (int i = 0; i < 3; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Instantiate(holyBullet, bulletTransform.position, shotRotation);
                angle -= 10f;
            }
        }
        else{
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 10f;
            for (int i = 0; i < 3; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                //transform.rotation = shotRotation;
                Instantiate(voidBullet, bulletTransform.position, shotRotation);
                angle -= 10f;
            }
        }
    }

    private void ShootFive(string colour){
        if (colour == "Holy"){
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 30f;
            for (int i = 0; i < 5; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Instantiate(holyBullet, bulletTransform.position, shotRotation);
                angle -= 15f;
            }
        }
        else{
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 30f;
            for (int i = 0; i < 5; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Instantiate(voidBullet, bulletTransform.position, shotRotation);
                angle -= 15f;
            }
        }
    }

    private void ShootSeven(string colour){
        if (colour == "Holy"){
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 33f;
            for (int i = 0; i < 7; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Instantiate(holyBullet, bulletTransform.position, shotRotation);
                angle -= 11f;
            }
        }
        else{
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 33f;
            for (int i = 0; i < 5; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Instantiate(voidBullet, bulletTransform.position, shotRotation);
                angle -= 11f;
            }
        }
    }

    public void ShootAOEPulse(string colour){
        CinemachineShake.Instance.ShakeCamera(shakeIntensity, duration);
        if (colour == "Holy"){
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 30f;
            for (int i = 0; i < 12; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Debug.Log(transform.rotation);
                Instantiate(holyBullet, bulletTransform.position, shotRotation);
                angle -= 30f;
            }
        }
        else{
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 30f;
            for (int i = 0; i < 12; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Instantiate(voidBullet, bulletTransform.position, shotRotation);
                angle -= 30f;
            }
        }
    }
}
