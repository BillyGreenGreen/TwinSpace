using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HoverButton : MonoBehaviour
{
    public Color startColour;
    public Color highlightColour;
    public TextMeshProUGUI text;

    public GameObject settingsCanvas;
    public GameObject mainMenuCanvas;
    
    public void Hovering(){
        text.color = highlightColour;
    }

    public void NotHovering(){
        text.color = startColour;
    }

    public void LoadScene(string name){
        SceneManager.LoadScene(name);
    }

    public void RestartGame(){
        GameManager.Instance.ResetGame();
    }

    public void ExitGame(){
        Application.Quit();
    }

    public void ContinueGame(){
        GameManager.Instance.UnpauseGame();
    }

    public void ShowMainMenuCanvas(){
        mainMenuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }

    public void ShowSettingsCanvas(){
        settingsCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        
    }
}
