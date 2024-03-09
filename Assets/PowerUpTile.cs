using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpTile : MonoBehaviour
{
    PowerUps powerUps;
    [SerializeField] TextMeshProUGUI nameOfPowerUp;
    [SerializeField] GameObject hoverImage;
    
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
        //start next level (probably reset game in game manager)
        GameManager.Instance.ResetGame();
    }

    public void Hover(){
        hoverImage.SetActive(true);
    }

    public void UnHover(){
        hoverImage.SetActive(false);
    }
}
