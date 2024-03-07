using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameWon : MonoBehaviour
{
    private void OnEnable() {
        TextMeshProUGUI timelinesSavedText = GameObject.Find("TimelinesSaved").GetComponent<TextMeshProUGUI>();
        timelinesSavedText.text = "TIMELINES SAVED: " + PlayerPrefs.GetInt("GamesWon").ToString();
    }
}
