using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] versionTexts;
    string version = "0.1";
    // Start is called before the first frame update
    void Start()
    {
        foreach(TextMeshProUGUI textPro in versionTexts){
            textPro.text = version;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
