using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Shooting shooting;
    private int shieldUpgradeLevel = 1;
    List<int> shieldPercentages = new List<int>();
    private int shotgunUpgradeLevel = 1;
    [HideInInspector] public int AOEPulseUpgradeLevel = 1;
    private int fasterProjUpgradeLevel = 1;
    private int fasterFireRateUpgradeLevel = 1;
    private int fasterDashCooldownUpgradeLevel = 1;
    private List<string> activePowerups;
    private List<string> powerUpsToBuy = new List<string>();
    private float healAmount = 0.1f;

    public List<string> GetActivePowerUps(){
        return activePowerups;
    }

    public List<string> GeneratePowerUpsToBuy(){
        //logic to pick which ones
        powerUpsToBuy.Clear();
        List<string> availablePowerUps = new List<string>();
        if (shieldUpgradeLevel <= 10){
            availablePowerUps.Add("shield " + shieldUpgradeLevel.ToString());
        }
        if (shotgunUpgradeLevel <= 3){
            availablePowerUps.Add("shotgun " + shotgunUpgradeLevel.ToString());
        }
        if (AOEPulseUpgradeLevel <= 10){
            availablePowerUps.Add("pulse " + AOEPulseUpgradeLevel.ToString());
        }
        if (fasterProjUpgradeLevel <= 10){
            availablePowerUps.Add("fasterProj " + fasterProjUpgradeLevel.ToString());
        }
        if (fasterFireRateUpgradeLevel <= 10){
            availablePowerUps.Add("fasterFireRate " + fasterFireRateUpgradeLevel.ToString());
        }
        if (fasterDashCooldownUpgradeLevel <= 9){
            availablePowerUps.Add("dash " + fasterDashCooldownUpgradeLevel.ToString());
        }
        availablePowerUps.Add("heal");
        ShuffleThing.Shuffle(availablePowerUps);
        foreach (var s in availablePowerUps){
            Debug.Log(s);
        }
        int numOfUpgrades = 3;
        if (availablePowerUps.Count < 3){
            numOfUpgrades = availablePowerUps.Count;
        }
        for (int i = 0; i < numOfUpgrades; i++)
        {
            powerUpsToBuy.Add(availablePowerUps[i]);
        }
        return powerUpsToBuy;
    }

    public void ResetPowerUps(){
        ShieldPowerUp(false);
        ShotgunPowerUp(false);
        AOEPulsePowerUp(false);
        FasterDashCooldown(false);
        FasterFireRatePowerUp(false);
        FasterProjectilesPowerUp(false);
    }

    public void HealPowerUp(bool enable){
        if (enable){
            GameManager.Instance.IncreasePlayerHealth(Mathf.RoundToInt((GameManager.Instance.healthSlider.maxValue * healAmount)));
        }
    }

    public void ShieldPowerUp(bool enable){
        if (enable){
            if (shieldUpgradeLevel == 1){
                shieldPercentages.Add(50);
                GameManager.Instance.EnableShield(1, shieldPercentages);
            }
            else if (shieldUpgradeLevel == 2){
                GameManager.Instance.EnableShield(2, shieldPercentages);
            }
            else if (shieldUpgradeLevel == 3){
                shieldPercentages.Add(25);
                GameManager.Instance.EnableShield(2, shieldPercentages);
            }
            else if (shieldUpgradeLevel == 4){
                GameManager.Instance.EnableShield(3, shieldPercentages);
            }
            else if (shieldUpgradeLevel == 5){
                GameManager.Instance.EnableShield(4, shieldPercentages);
            }
            else if (shieldUpgradeLevel == 6){
                shieldPercentages.Add(75);
                GameManager.Instance.EnableShield(4, shieldPercentages);
            }
            else if (shieldUpgradeLevel == 7){
                GameManager.Instance.EnableShield(5, shieldPercentages);
            }
            else if (shieldUpgradeLevel == 8){
                GameManager.Instance.EnableShield(6, shieldPercentages);
            }
            else if (shieldUpgradeLevel == 9){
                shieldPercentages.Add(33); //MIGHT NOT WORK
                GameManager.Instance.EnableShield(6, shieldPercentages);
            }else if (shieldUpgradeLevel == 9){
                GameManager.Instance.EnableShield(8, shieldPercentages);
            }
        }
        else{ //reset
            shieldUpgradeLevel = 1;
            GameManager.Instance.DisableShield();
            shieldPercentages.Clear();
        }
    }

    public void ShotgunPowerUp(bool enable){
        //can shoot more bullets (3/5)
        if (enable){
            if (shotgunUpgradeLevel == 1){
                shooting.amountOfBulletsToFire = 3;
            }
            else if (shotgunUpgradeLevel == 2){
                shooting.amountOfBulletsToFire = 5;
            }
            else if (shotgunUpgradeLevel == 3){
                shooting.amountOfBulletsToFire = 7;
            }
        }
        else{ //reset
            shooting.amountOfBulletsToFire = 1;
            shotgunUpgradeLevel = 1;
        }
    }

    public void AOEPulsePowerUp(bool enable){ //logic for this is in Shooting.cs
        if (enable){
            //shoot 360 degrees with a 5%/10%/20% chance every second
            shooting.canPulse = true;
            if (AOEPulseUpgradeLevel == 1){
                shooting.pulseTime = 2;
            }
            else if (AOEPulseUpgradeLevel == 2){
                shooting.pulseTime = 2;
            }
            else if (AOEPulseUpgradeLevel == 3){
                shooting.pulseTime = 1.5f;
            }
            else if (AOEPulseUpgradeLevel == 4){
                shooting.pulseTime = 1.5f;
            }
            else if (AOEPulseUpgradeLevel == 5){
                shooting.pulseTime = 1;
            }
            else if (AOEPulseUpgradeLevel == 6){
                shooting.pulseTime = 1;
            }
            else if (AOEPulseUpgradeLevel == 7){
                shooting.pulseTime = 1;
            }
            else if (AOEPulseUpgradeLevel == 8){
                shooting.pulseTime = 1;
            }
        }
        else{ //reset
            shooting.canPulse = false;
            AOEPulseUpgradeLevel = 1;
        }
    }

    public void FasterProjectilesPowerUp(bool enable){
        //ADD BULLET SPEED TO GAMEMANAGER AND CHANGE IT HERE AND GET IT ON THE BULLET PREFAB
        if (enable){
            if (fasterProjUpgradeLevel == 1){
                GameManager.Instance.SetBulletSpeed(15);
            }
            else if (fasterProjUpgradeLevel == 2){
                GameManager.Instance.SetBulletSpeed(18);
            }
            else if (fasterProjUpgradeLevel == 3){
                GameManager.Instance.SetBulletSpeed(22);
            }
            else if (fasterProjUpgradeLevel == 4){
                GameManager.Instance.SetBulletSpeed(25);
            }
            else if (fasterProjUpgradeLevel == 5){
                GameManager.Instance.SetBulletSpeed(28);
            }
            else if (fasterProjUpgradeLevel == 6){
                GameManager.Instance.SetBulletSpeed(33);
            }
            else if (fasterProjUpgradeLevel == 7){
                GameManager.Instance.SetBulletSpeed(35);
            }
            else if (fasterProjUpgradeLevel == 8){
                GameManager.Instance.SetBulletSpeed(40);
            }
            else if (fasterProjUpgradeLevel == 9){
                GameManager.Instance.SetBulletSpeed(45);
            }
            else if (fasterProjUpgradeLevel == 10){
                GameManager.Instance.SetBulletSpeed(50);
            }
        }
        else{
            GameManager.Instance.SetBulletSpeed(12);
            fasterProjUpgradeLevel = 1;
        }
    }

    public void FasterFireRatePowerUp(bool enable){
        if (enable){
            if (fasterFireRateUpgradeLevel == 1){
                shooting.timeBetweenFiring = 0.4f;
            }
            else if (fasterFireRateUpgradeLevel == 2){
                shooting.timeBetweenFiring = 0.36f;
            }
            else if (fasterFireRateUpgradeLevel == 3){
                shooting.timeBetweenFiring = 0.33f;
            }
            else if (fasterFireRateUpgradeLevel == 4){
                shooting.timeBetweenFiring = 0.3f;
            }
            else if (fasterFireRateUpgradeLevel == 5){
                shooting.timeBetweenFiring = 0.26f;
            }
            else if (fasterFireRateUpgradeLevel == 6){
                shooting.timeBetweenFiring = 0.23f;
            }
            else if (fasterFireRateUpgradeLevel == 7){
                shooting.timeBetweenFiring = 0.2f;
            }
            else if (fasterFireRateUpgradeLevel == 8){
                shooting.timeBetweenFiring = 0.16f;
            }
            else if (fasterFireRateUpgradeLevel == 9){
                shooting.timeBetweenFiring = 0.13f;
            }
            else if (fasterFireRateUpgradeLevel == 10){
                shooting.timeBetweenFiring = 0.1f;
            }
            
        }
        else{
            shooting.timeBetweenFiring = 0.42f;
            fasterFireRateUpgradeLevel = 1;
        }
    }

    public void FasterDashCooldown(bool enable){
        if (enable){
            if (fasterDashCooldownUpgradeLevel == 1){
                playerController.dashCooldown = 4.5f;
            }
            else if (fasterDashCooldownUpgradeLevel == 2){
                playerController.dashCooldown = 4f;
            }
            else if (fasterDashCooldownUpgradeLevel == 3){
                playerController.dashCooldown = 3.5f;
            }
            else if (fasterDashCooldownUpgradeLevel == 4){
                playerController.dashCooldown = 3f;
            }
            else if (fasterDashCooldownUpgradeLevel == 5){
                playerController.dashCooldown = 2.5f;
            }
            else if (fasterDashCooldownUpgradeLevel == 6){
                playerController.dashCooldown = 2f;
            }
            else if (fasterDashCooldownUpgradeLevel == 7){
                playerController.dashCooldown = 1.5f;
            }
            else if (fasterDashCooldownUpgradeLevel == 8){
                playerController.dashCooldown = 1f;
            }
            else if (fasterDashCooldownUpgradeLevel == 9){
                playerController.dashCooldown = 0.5f;
            }
            playerController.dashBar.maxValue = playerController.dashCooldown;
        }
        else{
            fasterDashCooldownUpgradeLevel = 1;
            playerController.dashCooldown = 5f;
            playerController.dashBar.maxValue = playerController.dashCooldown;
        }
    }

    public void UpgradeShieldPowerUp(){
        if (shieldUpgradeLevel <= 10){
            shieldUpgradeLevel++;
        }
    }
    
    public void UpgradeShotgunPowerUp(){
        if (shotgunUpgradeLevel <= 3){
            shotgunUpgradeLevel++;
        }
    }
    
    public void UpgradeAOEPowerUp(){
        if (AOEPulseUpgradeLevel <= 8){
            AOEPulseUpgradeLevel++;
        }
    }

    public void UpgradeDashCooldownPowerUp(){
        if (fasterDashCooldownUpgradeLevel <= 9){
            fasterDashCooldownUpgradeLevel++;
        }
    }

    public void UpgradeFasterProjPowerUp(){
        if (fasterProjUpgradeLevel <= 10){
            fasterProjUpgradeLevel++;
        }
    }

    public void UpgradeFasterFireRatePowerUp(){
        if (fasterFireRateUpgradeLevel <= 10){
            fasterFireRateUpgradeLevel++;
        }
    }

    

    
}

static class ShuffleThing {
    public static void Shuffle<T>(this IList<T> list)  
    {
        System.Random rng = new System.Random();
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}
