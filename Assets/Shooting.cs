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
    public Rigidbody2D playerRb;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;
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

            transform.rotation = Quaternion.Euler(0,0,rotZ);
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
                    Instantiate(voidBullet, bulletTransform.position, bulletTransform.rotation);
                }
                else{
                    Instantiate(holyBullet, bulletTransform.position, bulletTransform.rotation);
                }
                
            }
        }
        
    }
}
