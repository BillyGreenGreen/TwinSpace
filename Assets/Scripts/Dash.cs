using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private Slider dashBar;

    //[SerializeField] private TrailRenderer tr;

    private float dashTimer = 0;

    //Will get this from upgrades when implemented
    private float dashTimerMax = 2;
    private bool onCooldown;

    public float dashPower; // This does nothing, have to change the mass of the rigidbody, only way.

    // Start is called before the first frame update
    void Start()
    {
        dashBar.maxValue = dashTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGamePlaying){
            if (Input.GetKeyDown(KeyCode.Space)){
                dashTimer += Time.deltaTime;
                if (dashTimer < dashTimerMax){
                    onCooldown = true;
                    Vector2 dashDirection = rb.velocity;
                    
                    //rb.AddForce(rb.velocity * dashPower, ForceMode2D.Impulse);
                }
            }
            if (dashTimer < dashTimerMax && onCooldown){
                dashTimer += Time.deltaTime;
                dashBar.value = dashTimer;
            }
            if (dashTimer > dashTimerMax){
                dashTimer = 0;
                onCooldown = false;
            }
        }
        
    }

    private void FixedUpdate() {
        if (dashTimer < dashTimerMax){
            Debug.Log(rb.velocity * dashPower);
            //rb.velocity
            rb.AddForce(rb.velocity * dashPower, ForceMode2D.Impulse);
        }
    }
}
