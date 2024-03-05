using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 mousePos;
    private Rigidbody2D rb;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector2 playerVel = GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity;
        rb.velocity = playerVel + new Vector2(direction.x, direction.y).normalized * force;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //instantiate particle effect
        Destroy(gameObject);
    }
}
