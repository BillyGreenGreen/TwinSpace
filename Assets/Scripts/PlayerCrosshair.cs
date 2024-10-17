using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrosshair : MonoBehaviour
{
    [SerializeField] private Color crosshairColour;
    public GameObject crosshair;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Crosshair") == null){
            crosshair = GameObject.FindGameObjectWithTag("DebugCrosshair");
        }
        else{
            crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        }
        
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowCrosshair(){
        crosshair.SetActive(true);
        Cursor.visible = false;
    }

    public void HideCrosshair(){
        Cursor.visible = true;
        crosshair.SetActive(false);
    }
}
