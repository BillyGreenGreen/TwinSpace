using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimelinesText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void OnEnable() {
        text.text = "TIMELINES SAVED: " + PlayerPrefs.GetInt("GamesWon").ToString();
    }
}
