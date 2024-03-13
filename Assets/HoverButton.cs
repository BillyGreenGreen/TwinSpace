using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour
{
    public Color startColour;
    public Color highlightColour;
    public TextMeshProUGUI text;

    public GameObject settingsCanvas;
    public GameObject mainMenuCanvas;
    public GameObject mainMenuFirstButton;
    public GameObject settingsMenuFirstButton;
    public GameObject patchNotes;
    public CanvasGroup cg;
    public SimpleCrosshair crosshair;
    public EventSystem eventSystem;
    
    public void Hovering(){
        text.color = highlightColour;
    }

    public void NotHovering(){
        text.color = startColour;
    }

    public void VoidButtonsHover(){
        DOVirtual.Float(0, 1, 0.2f, v => {
            cg.alpha = v;
        });
    }

    public void DuplicateCrosshair(){
        GameObject go = Instantiate(crosshair.gameObject);
        go.GetComponent<SimpleCrosshair>().GenerateCrosshair();
        go.name = "Crosshair";
        go.tag = "Crosshair";
        Debug.Log(go.name);
    }

    public void VoidButtonsUnHover(){
        DOVirtual.Float(1, 0, 0.2f, v => {
            cg.alpha = v;
        });
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
        NotHovering();
        eventSystem.SetSelectedGameObject(mainMenuFirstButton);
    }

    public void ShowSettingsCanvas(){
        settingsCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        NotHovering();
        VoidButtonsUnHover();
        eventSystem.SetSelectedGameObject(settingsMenuFirstButton);
    }

    public void PatchNotes(){
        TextMeshProUGUI buttonText = transform.Find("ShowPatchNotesText").GetComponent<TextMeshProUGUI>();
        if (buttonText.text == "Show Patch Notes"){
            patchNotes.SetActive(true);
            buttonText.text = "Hide Patch Notes";
        }
        else{
            patchNotes.SetActive(false);
            buttonText.text = "Show Patch Notes";
        }
        
    }
}
