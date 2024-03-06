using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private float timeBetweenOrbDrops = 0.1f;
    private float timer = 0;
    private bool shouldBeDroppingOrbs = false;
    private int numberOfOrbsToDrop = 0;
    private int numberOfOrbsLeftToDrop = 0;
    private string colourOfOrbs;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (shouldBeDroppingOrbs && numberOfOrbsLeftToDrop > 0){
            timer += Time.deltaTime;
            if (timer >= timeBetweenOrbDrops){
                //drop orb
                if (colourOfOrbs == "Void"){
                    Instantiate(Resources.Load<GameObject>("Prefabs/Orbs/VoidDeposit"), transform.position, Quaternion.identity);
                    numberOfOrbsLeftToDrop--;
                    GameManager.Instance.DecreaseOrbCount("Void");
                }
                else{
                    Instantiate(Resources.Load<GameObject>("Prefabs/Orbs/HolyDeposit"), transform.position, Quaternion.identity);
                    numberOfOrbsLeftToDrop--;
                    GameManager.Instance.DecreaseOrbCount("Holy");
                }
                timer = 0;
            }
        }
        else{
            shouldBeDroppingOrbs = false;
        }
    }

    private void FixedUpdate() {
        rb.velocity = movement * moveSpeed;
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "VoidBlackHole"){
            numberOfOrbsToDrop = GameManager.Instance.numberOfVoidOrbs;
            if (numberOfOrbsToDrop > 0){
                numberOfOrbsLeftToDrop = numberOfOrbsToDrop;
                shouldBeDroppingOrbs = true;
                colourOfOrbs = "Void";
            }
            
        }
        else if(other.name == "HolyBlackHole"){
            numberOfOrbsToDrop = GameManager.Instance.numberOfHolyOrbs;
            if (numberOfOrbsToDrop > 0){
                numberOfOrbsLeftToDrop = numberOfOrbsToDrop;
                shouldBeDroppingOrbs = true;
                colourOfOrbs = "Holy";
            }
        }
    }
}
