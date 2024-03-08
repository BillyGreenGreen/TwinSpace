using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupFadingPulse : MonoBehaviour
{
    public Image image;
    float speed = 2f;

    // Update is called once per frame
    void Update()
    {
        //calculate what the new Y position will be
        float newAlpha = Mathf.PingPong((Time.time) * speed, 1);
        //set the object's Y to the new calculated Y
        image.color = new Color(1,1,1,Mathf.Lerp(.5f, 1f, newAlpha));
        //transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
