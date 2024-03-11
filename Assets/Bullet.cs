using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 mousePos;
    private Rigidbody2D rb;
    public float force;
    public GameObject impactEffect;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector2 playerVel = GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity;
        force = GameManager.Instance.GetBulletSpeed();
        rb.velocity = playerVel + new Vector2(transform.up.x, transform.up.y) * force;
        //rb.velocity = playerVel + new Vector2(direction.x, direction.y).normalized * force;
        //rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name.Contains("Slime") && !other.name.Contains("Big")){
            other.GetComponent<SlimeAI>().slimeHealth -= 1;
            if (other.GetComponent<SlimeAI>().slimeHealth <= 0){
                other.GetComponent<SlimeAI>().Die();
            }
        }
        if (gameObject.name.Contains("Holy")){
            impactEffect.GetComponent<SpriteRenderer>().color = new Color(1, 0.8f, 0);
        }
        else{
            impactEffect.GetComponent<SpriteRenderer>().color = new Color(0.23f, 0, 0.71f);
        }
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
