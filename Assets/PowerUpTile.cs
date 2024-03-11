using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PowerUpTile : MonoBehaviour
{
    PowerUps powerUps;
    [SerializeField] TextMeshProUGUI nameOfPowerUp;
    [SerializeField] TextMeshProUGUI levelOfPowerUp;
    [SerializeField] GameObject hoverImage;
    private GameObject tooltip;
    
    private void Start() {
        powerUps = GameObject.Find("Player").GetComponent<PowerUps>();
    }
    public void PickPowerUp(){
        //Debug.Log(nameOfPowerUp.text);
        if (nameOfPowerUp.text == "shield"){
            powerUps.ShieldPowerUp(true);
            powerUps.UpgradeShieldPowerUp();
        }
        else if (nameOfPowerUp.text == "shotgun"){
            powerUps.ShotgunPowerUp(true);
            powerUps.UpgradeShotgunPowerUp();
        }
        else if (nameOfPowerUp.text == "pulse"){
            powerUps.AOEPulsePowerUp(true);
            powerUps.UpgradeAOEPowerUp();
        }
        else if (nameOfPowerUp.text == "faster fire rate"){
            powerUps.FasterFireRatePowerUp(true);
            powerUps.UpgradeFasterFireRatePowerUp();
        }
        else if (nameOfPowerUp.text == "faster projectiles"){
            powerUps.FasterProjectilesPowerUp(true);
            powerUps.UpgradeFasterProjPowerUp();
        }
        else if (nameOfPowerUp.text == "faster dash cooldown"){
            powerUps.FasterDashCooldown(true);
            powerUps.UpgradeDashCooldownPowerUp();
        }
        hoverImage.SetActive(false);
        GameObject [] gos = GameObject.FindGameObjectsWithTag("PowerUpTooltip");
        foreach (GameObject go in gos){
            Destroy(go);
        }
        //start next level (probably reset game in game manager)
        GameManager.Instance.NextStage();
    }

    public void Hover(){
        hoverImage.SetActive(true);
    }

    public void UnHover(){
        hoverImage.SetActive(false);
    }

    public void ShowTooltip(){
        //yellow colour upgrade #FFAF0B
        Transform gameWonCanvas = GameObject.Find("GameWon").transform;
        tooltip = Instantiate(Resources.Load<GameObject>("PowerUps/PowerUpTooltip"), new Vector2(transform.position.x, transform.position.y - 135), Quaternion.identity, gameWonCanvas);
        tooltip.transform.Find("PowerUpName").GetComponent<TextMeshProUGUI>().text = nameOfPowerUp.text;
        tooltip.transform.Find("PowerUpLevel").GetComponent<TextMeshProUGUI>().text = "L" + levelOfPowerUp.text.Split(" ")[1];
        TextMeshProUGUI description = tooltip.transform.Find("PowerUpDescription").GetComponent<TextMeshProUGUI>();
        if (nameOfPowerUp.text == "shield"){
            string upgradeText = "";
            switch (levelOfPowerUp.text.Split(" ")[1]){
                case "1":
                    upgradeText = "2";
                    break;
                case "2":
                    upgradeText = "4";
                    break;
                case "3":
                    upgradeText = "8";
                    break;
            }
            description.text = string.Format("At 25/50/75% health create a invulnerability shield for <#FFAF0B>{0} <#FFFFFF>seconds.", upgradeText);
        }
        else if (nameOfPowerUp.text == "shotgun"){
            string upgradeText = "";
            switch (levelOfPowerUp.text.Split(" ")[1]){
                case "1":
                    upgradeText = "3";
                    break;
                case "2":
                    upgradeText = "5";
                    break;
            }
            description.text = string.Format("Fire <#FFAF0B>{0} <#FFFFFF>shots every time you fire.", upgradeText);
        }
        else if (nameOfPowerUp.text == "pulse"){
            string upgradeText = "";
            switch (levelOfPowerUp.text.Split(" ")[1]){
                case "1":
                    upgradeText = "5%";
                    break;
                case "2":
                    upgradeText = "10%";
                    break;
                case "3":
                    upgradeText = "20%";
                    break;
            }
            description.text = string.Format("Every second you have a <#FFAF0B>{0} <#FFFFFF>chance to pulse bullets.", upgradeText);
        }
        else if (nameOfPowerUp.text == "faster fire rate"){
            string upgradeText = "";
            switch (levelOfPowerUp.text.Split(" ")[1]){
                case "1":
                    upgradeText = "2";
                    break;
                case "2":
                    upgradeText = "4";
                    break;
                case "3":
                    upgradeText = "8";
                    break;
            }
            description.text = string.Format("Shoot at a faster rate of fire. (Will have upgrades in the future)", upgradeText);
        }
        else if (nameOfPowerUp.text == "faster projectiles"){
            string upgradeText = "";
            switch (levelOfPowerUp.text.Split(" ")[1]){
                case "1":
                    upgradeText = "2";
                    break;
                case "2":
                    upgradeText = "4";
                    break;
                case "3":
                    upgradeText = "8";
                    break;
            }
            description.text = string.Format("Projectiles move faster. (Will have upgrades in the future)", upgradeText);
        }
        else if (nameOfPowerUp.text == "faster dash cooldown"){
            string upgradeText = "";
            switch (levelOfPowerUp.text.Split(" ")[1]){
                case "1":
                    upgradeText = "1";
                    break;
                case "2":
                    upgradeText = "0.5";
                    break;
            }
            description.text = string.Format("Dash cooldown is now <#FFAF0B>{0} <#FFFFFF>seconds.", upgradeText);
        }
        CanvasGroup cg = tooltip.GetComponent<CanvasGroup>();
        DOVirtual.Float(0, 1, 0.2f, v => {
            cg.alpha = v;
        });
    }

    public void HideTooltip(){
        CanvasGroup cg = tooltip.GetComponent<CanvasGroup>();
        DOVirtual.Float(1, 0, 0.2f, v => {
            cg.alpha = v;
        });
    }
}
