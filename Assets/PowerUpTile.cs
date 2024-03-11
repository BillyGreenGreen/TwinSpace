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
        if (nameOfPowerUp.text == "heal"){
            powerUps.HealPowerUp(true);
        }
        else if (nameOfPowerUp.text == "shield"){
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
        transform.DOKill();
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
        if (nameOfPowerUp.text == "heal"){
            description.text = "Heal for <#FFAF0B>10% <#FFFFFF>of your maximum health.";
        }
        if (nameOfPowerUp.text == "shield"){
            List<string> upgradeText = new List<string>();
            switch (levelOfPowerUp.text.Split(" ")[1]){
                case "1":
                    upgradeText.Add("50%");
                    upgradeText.Add("1");
                    break;
                case "2":
                    upgradeText.Add("50%");
                    upgradeText.Add("2");
                    break;
                case "3":
                    upgradeText.Add("25/50%");
                    upgradeText.Add("2");
                    break;
                case "4":
                    upgradeText.Add("25/50%");
                    upgradeText.Add("3");
                    break;
                case "5":
                    upgradeText.Add("25/50%");
                    upgradeText.Add("4");
                    break;
                case "6":
                    upgradeText.Add("25/50/75%");
                    upgradeText.Add("4");
                    break;
                case "7":
                    upgradeText.Add("25/50/75%");
                    upgradeText.Add("5");
                    break;
                case "8":
                    upgradeText.Add("25/50/75%");
                    upgradeText.Add("6");
                    break;
                case "9":
                    upgradeText.Add("25/33/50/75%");
                    upgradeText.Add("6");
                    break;
                case "10":
                    upgradeText.Add("25/33/50/75%");
                    upgradeText.Add("8");
                    break;
            }
            string secondsPlural = "seconds";
            if (int.Parse(upgradeText[1]) == 1){
                secondsPlural = "second";
            }
            description.text = string.Format("At <#FFAF0B>{0} <#FFFFFF>health create a invulnerability shield for <#FFAF0B>{1} <#FFFFFF>{2}.", upgradeText[0], upgradeText[1], secondsPlural);
            upgradeText.Clear();
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
            List<string> upgradeText = new List<string>();
            switch (levelOfPowerUp.text.Split(" ")[1]){
                case "1":
                    upgradeText.Add("2");
                    upgradeText.Add("5%");
                    break;
                case "2":
                    upgradeText.Add("2");
                    upgradeText.Add("10%");
                    break;
                case "3":
                    upgradeText.Add("1.5");
                    upgradeText.Add("10%");
                    break;
                case "4":
                    upgradeText.Add("1.5");
                    upgradeText.Add("15%");
                    break;
                case "5":
                    upgradeText.Add("1");
                    upgradeText.Add("15%");
                    break;
                case "6":
                    upgradeText.Add("1");
                    upgradeText.Add("20%");
                    break;
                case "7":
                    upgradeText.Add("1");
                    upgradeText.Add("25%");
                    break;
                case "8":
                    upgradeText.Add("1");
                    upgradeText.Add("30%");
                    break;
            }
            string secondsPlural = "seconds";
            if (float.Parse(upgradeText[0]) == 1){
                secondsPlural = "second";
            }
            description.text = string.Format("Every <#FFAF0B>{0} <#FFFFFF>{1} you have a <#FFAF0B>{2} <#FFFFFF>chance to pulse bullets.", upgradeText[0], secondsPlural, upgradeText[1]);
        }
        else if (nameOfPowerUp.text == "faster fire rate"){
            string upgradeText = "";
            switch (levelOfPowerUp.text.Split(" ")[1]){
                case "1":
                    upgradeText = "5%";
                    break;
                case "2":
                    upgradeText = "16%";
                    break;
                case "3":
                    upgradeText = "27%";
                    break;
                case "4":
                    upgradeText = "40%";
                    break;
                case "5":
                    upgradeText = "61%";
                    break;
                case "6":
                    upgradeText = "82%";
                    break;
                case "7":
                    upgradeText = "110%";
                    break;
                case "8":
                    upgradeText = "162%";
                    break;
                case "9":
                    upgradeText = "223%";
                    break;
                case "10":
                    upgradeText = "320%";
                    break;
            }
            description.text = string.Format("Shoot <#FFAF0B>{0} <#FFFFFF>faster.", upgradeText);
        }
        else if (nameOfPowerUp.text == "faster projectiles"){
            string upgradeText = "";
            switch (levelOfPowerUp.text.Split(" ")[1]){
                case "1":
                    upgradeText = "25%";
                    break;
                case "2":
                    upgradeText = "50%";
                    break;
                case "3":
                    upgradeText = "83%";
                    break;
                case "4":
                    upgradeText = "108%";
                    break;
                case "5":
                    upgradeText = "133%";
                    break;
                case "6":
                    upgradeText = "175%";
                    break;
                case "7":
                    upgradeText = "191%";
                    break;
                case "8":
                    upgradeText = "233%";
                    break;
                case "9":
                    upgradeText = "275%";
                    break;
                case "10":
                    upgradeText = "316%";
                    break;
            }
            description.text = string.Format("Friendly projectiles move <#FFAF0B>{0} <#FFFFFF>faster.", upgradeText);
        }
        else if (nameOfPowerUp.text == "faster dash cooldown"){
            List<string> upgradeText = new List<string>();
            switch (levelOfPowerUp.text.Split(" ")[1]){
                case "1":
                    upgradeText.Add("4.5");
                    upgradeText.Add("");
                    break;
                case "2":
                    upgradeText.Add("4");
                    upgradeText.Add("");
                    break;
                case "3":
                    upgradeText.Add("3.5");
                    upgradeText.Add("");
                    break;
                case "4":
                    upgradeText.Add("3");
                    upgradeText.Add("\nInvulnerable while dashing.");
                    break;
                case "5":
                    upgradeText.Add("2.5");
                    upgradeText.Add("\nInvulnerable while dashing.");
                    break;
                case "6":
                    upgradeText.Add("2");
                    upgradeText.Add("\nInvulnerable while dashing.");
                    break;
                case "7":
                    upgradeText.Add("1.5");
                    upgradeText.Add("\nInvulnerable while dashing.");
                    break;
                case "8":
                    upgradeText.Add("1");
                    upgradeText.Add("\nInvulnerable while dashing.");
                    break;
                case "9":
                    upgradeText.Add("0.5");
                    upgradeText.Add("\nInvulnerable while dashing.");
                    break;
            }
            string secondsPlural = "seconds";
            if (float.Parse(upgradeText[0]) == 1){
                secondsPlural = "second";
            }
            description.text = string.Format("Dash cooldown reduced to <#FFAF0B>{0} <#FFFFFF>{1}.{2}", upgradeText[0], secondsPlural, upgradeText[1]);
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
