using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    

    [Header("Dash Settings")]
    [SerializeField] public Slider dashBar;
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] public float dashCooldown = 2f;
    [SerializeField] TrailRenderer trailRenderer;
    float dashTimer = 0;
    public bool isDashing = false;
    bool canDash = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashBar.maxValue = dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameManager.Instance.isGamePlaying){
            
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
            if (Input.GetKeyDown(KeyCode.Space) && canDash){
                dashTimer += Time.deltaTime;
                StartCoroutine(Dash());
            }
            if (dashTimer >= dashCooldown){
                dashTimer = 0;
            }
            else if (dashTimer > 0){
                dashTimer += Time.deltaTime;
                dashBar.value = dashTimer;
            }
            if (isDashing){
                return;
            }
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            
        }
        else{
            movement = Vector2.zero;
            shouldBeDroppingOrbs = false;
        }
        
    }

    private void FixedUpdate() {
        //rb.AddForce(movement * moveSpeed, ForceMode2D.Impulse);
        if (isDashing){
            return;
        }
        rb.velocity = movement * moveSpeed;
        
    }

    private IEnumerator Dash(){
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(movement.x * dashSpeed, movement.y * dashSpeed);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        trailRenderer.emitting = false;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "VoidBlackHole" + GameManager.Instance.arenaIndex.ToString()){
            numberOfOrbsToDrop = GameManager.Instance.numberOfVoidOrbs;
            if (numberOfOrbsToDrop > 0){
                numberOfOrbsLeftToDrop = numberOfOrbsToDrop;
                shouldBeDroppingOrbs = true;
                colourOfOrbs = "Void";
            }
            
        }
        else if(other.name == "HolyBlackHole" + GameManager.Instance.arenaIndex.ToString()){
            numberOfOrbsToDrop = GameManager.Instance.numberOfHolyOrbs;
            if (numberOfOrbsToDrop > 0){
                numberOfOrbsLeftToDrop = numberOfOrbsToDrop;
                shouldBeDroppingOrbs = true;
                colourOfOrbs = "Holy";
            }
        }
    }
}
