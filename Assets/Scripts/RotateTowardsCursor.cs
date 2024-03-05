using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCursor : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites = new Sprite[4];
    
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            // Get the position of the mouse cursor in screen space
            Vector3 cursorScreenSpace = Input.mousePosition;

            // Convert the mouse cursor position from screen space to world space
            Vector3 cursorWorldSpace = Camera.main.ScreenToWorldPoint(cursorScreenSpace);

            // Get the direction from the player to the mouse cursor
            Vector2 direction = cursorWorldSpace - transform.position;

            // Get the angle of the direction in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            if (angle <= 30 && angle >= -30)//look right
            {
                spriteRenderer.sprite = sprites[0];
            }
            else if (angle >= 30 && angle <= 135)//look up
            {
                spriteRenderer.sprite = sprites[1];
            }
            else if (angle >= 135 || angle <= -135)//look left
            {
                spriteRenderer.sprite = sprites[2];
            }
            else if (angle <= -30 && angle >= -135)//look down_left
            {
                spriteRenderer.sprite = sprites[3];
            }


        // Rotate the player towards the mouse cursor
        //transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
    }
}
