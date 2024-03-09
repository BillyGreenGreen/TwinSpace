using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Shooting shooting;
    private int shieldUpgradeLevel = 1;
    private int shotgunUpgradeLevel = 1;
    public int AOEPulseUpgradeLevel = 1;
    private int fasterProjUpgradeLevel = 1;
    private int fasterFireRateUpgradeLevel = 1;
    private int fasterDashCooldownUpgradeLevel = 1;
    private List<string> activePowerups;
    private List<string> powerUpsToBuy = new List<string>();

    public List<string> GetActivePowerUps(){
        return activePowerups;
    }

    public List<string> GeneratePowerUpsToBuy(){
        //logic to pick which ones
        powerUpsToBuy.Clear();
        List<string> availablePowerUps = new List<string>();
        if (shieldUpgradeLevel < 3){
            availablePowerUps.Add("shield " + shieldUpgradeLevel.ToString());
        }
        if (shotgunUpgradeLevel < 3){
            availablePowerUps.Add("shotgun " + shotgunUpgradeLevel.ToString());
        }
        if (AOEPulseUpgradeLevel < 3){
            availablePowerUps.Add("pulse " + AOEPulseUpgradeLevel.ToString());
        }
        if (fasterProjUpgradeLevel < 2){
            availablePowerUps.Add("fasterProj " + fasterProjUpgradeLevel.ToString());
        }
        if (fasterFireRateUpgradeLevel < 2){
            availablePowerUps.Add("fasterFireRate " + fasterFireRateUpgradeLevel.ToString());
        }
        if (fasterDashCooldownUpgradeLevel < 2){
            availablePowerUps.Add("dash " + fasterDashCooldownUpgradeLevel.ToString());
        }
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

    public void ShieldPowerUp(bool enable){
        //25/50/75% health add a shield for X (2/4/8) amount of time
        if (enable){
            if (shieldUpgradeLevel == 1){
                GameManager.Instance.EnableShield(2);
            }
            else if (shieldUpgradeLevel == 2){
                GameManager.Instance.EnableShield(4);
            }
            else if (shieldUpgradeLevel == 3){
                GameManager.Instance.EnableShield(8);
            }
        }
        else{ //reset
            shieldUpgradeLevel = 1;
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
        }
        else{ //reset
            shooting.canPulse = false;
        }
    }

    public void FasterProjectilesPowerUp(bool enable){
        //ADD BULLET SPEED TO GAMEMANAGER AND CHANGE IT HERE AND GET IT ON THE BULLET PREFAB
        if (enable){
            GameManager.Instance.SetBulletSpeed(40);
        }
        else{
            GameManager.Instance.SetBulletSpeed(20);
        }
    }

    public void FasterFireRatePowerUp(bool enable){
        if (enable){
            shooting.timeBetweenFiring = 0.1f;
        }
        else{
            shooting.timeBetweenFiring = 0.3f;
        }
    }

    public void FasterDashCooldown(bool enable){
        if (enable){
            if (fasterDashCooldownUpgradeLevel == 1){
                playerController.dashCooldown = 1f;
            }
            else if (fasterDashCooldownUpgradeLevel == 2){
                playerController.dashCooldown = 0.5f;
            }
            playerController.dashBar.maxValue = playerController.dashCooldown;
        }
        else{
            fasterDashCooldownUpgradeLevel = 1;
            playerController.dashCooldown = 2f;
            playerController.dashBar.maxValue = playerController.dashCooldown;
        }
    }

    public void UpgradeShieldPowerUp(){
        if (shieldUpgradeLevel < 3){
            shieldUpgradeLevel++;
        }
    }
    
    public void UpgradeShotgunPowerUp(){
        if (shotgunUpgradeLevel < 3){
            shotgunUpgradeLevel++;
        }
    }
    
    public void UpgradeAOEPowerUp(){
        if (AOEPulseUpgradeLevel < 3){
            AOEPulseUpgradeLevel++;
        }
    }

    public void UpgradeDashCooldownPowerUp(){
        if (fasterDashCooldownUpgradeLevel < 2){
            fasterDashCooldownUpgradeLevel++;
        }
    }

    public void UpgradeFasterProjPowerUp(){
        if (fasterProjUpgradeLevel < 2){
            fasterProjUpgradeLevel++;
        }
    }

    public void UpgradeFasterFireRatePowerUp(){
        if (fasterFireRateUpgradeLevel < 2){
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
