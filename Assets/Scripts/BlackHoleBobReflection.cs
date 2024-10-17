using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlackHoleBobReflection : MonoBehaviour
{
    float speed = 2f;
    float height = -0.12f;
    Vector3 pos;
    [SerializeField] TextMeshProUGUI thisText;
    [SerializeField] TextMeshProUGUI realText;
    private void Start() {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        thisText.text = realText.text;
    }
}
