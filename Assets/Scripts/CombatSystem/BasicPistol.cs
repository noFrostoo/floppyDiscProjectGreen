using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class BasicPistol : IRangeWeaponBase
{
    FireMode fireMode = FireMode.Semi_Automatic;
    GameObject projectile;
    GameCharacter shooter;
    BulletsMaker bulletsMaker;
    int oneShotActionPointsCost = 10;
    int damage = 10;
    int fireRate = 180;
    int magazine = 9;
    int burstAmountOfShoots = 3;
    int waitingTimeBetwennShots; 
    int currentAmmoInMagazine = 9;
    int chanceOfHitBouns = 20;
    
    public BasicPistol(GameCharacter shooter, int ammoInMagazine = 9)
    {
        this.shooter = shooter;
        bulletsMaker = shooter.GetComponent<BulletsMaker>();
        int chanceOfHit = chanceOfHitBouns + shooter.gameObject.GetComponent<StatsSystem>().Accuracy;
        if(bulletsMaker == null)
            bulletsMaker = shooter.gameObject.AddComponent<BulletsMaker>();
        projectile = Resources.Load("Prefabs/Projectile") as GameObject;
        projectile.GetComponent<Bullet>().SetInfomationForBullet(damage, chanceOfHit);
        waitingTimeBetwennShots = 60/fireRate;
        currentAmmoInMagazine = ammoInMagazine;
    }

    public void ChangeFireMode(FireMode fireMode)
    {
        this.fireMode = fireMode;
    }

    public int Fire(GameCharacter target)
    {
        int chanceOfHit = chanceOfHitBouns + shooter.gameObject.GetComponent<StatsSystem>().Accuracy;
        projectile.GetComponent<Bullet>().SetInfomationForBullet(damage, chanceOfHit);
        if(shooter == null) throw new NoShooterSetException();
        if(target == null) throw new NullTargetException();
        if(currentAmmoInMagazine == 0) throw new NoAmmoInMag();
        switch(fireMode)
        {
            case FireMode.Semi_Automatic:
                return FireSemiAutomatic(target);
            case FireMode.Automatic:
                return FireAutomatic(target);
            case FireMode.Burst:
                return FireBurst(target);
            default:
                Debug.Log("UNKNOWN FIRE MODE");
                return 0;
        }

    }

    private int FireAutomatic(GameCharacter target)
    {
        Vector2 lookDirection = target.transform.position - shooter.transform.position;
        int amoutOfShoots = (int)Mathf.Floor(shooter.GetCurrentActionPoints()/oneShotActionPointsCost);
        if(amoutOfShoots > currentAmmoInMagazine)
            amoutOfShoots = currentAmmoInMagazine;
        currentAmmoInMagazine -= amoutOfShoots;
        int cost = (int)Mathf.Floor((amoutOfShoots*oneShotActionPointsCost) * 0.9f);
        shooter.StartShooting(amoutOfShoots, lookDirection, projectile, waitingTimeBetwennShots);
        return cost;
    }

    private int FireBurst(GameCharacter target)
    {
        Vector2 lookDirection = target.transform.position - shooter.transform.position;
        int amoutOfShoots = shooter.GetCurrentActionPoints()/damage;
        if(amoutOfShoots > currentAmmoInMagazine)
            amoutOfShoots = currentAmmoInMagazine;
        if(amoutOfShoots < burstAmountOfShoots)
        {
            currentAmmoInMagazine -= amoutOfShoots;
            shooter.StartShooting(amoutOfShoots, lookDirection, projectile, waitingTimeBetwennShots);
            return (int)Mathf.Floor((amoutOfShoots*oneShotActionPointsCost) * 0.9f);
        }
        else
        {
            currentAmmoInMagazine -= burstAmountOfShoots;
            shooter.StartShooting(burstAmountOfShoots, lookDirection, projectile, waitingTimeBetwennShots);
            return (int)Mathf.Floor((burstAmountOfShoots*oneShotActionPointsCost) * 0.9f);
        }    
    }

    private int FireSemiAutomatic(GameCharacter target)
    {
        Vector2 lookDirection = target.transform.position - shooter.transform.position;
        currentAmmoInMagazine -= 1;
        shooter.StartShooting(1, lookDirection, projectile,  waitingTimeBetwennShots);
        return damage;
    }

    public int GetCurrentAmmoInMagazine()
    {
        return currentAmmoInMagazine;
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetFireRate()
    {
       return fireRate;
    }

    public int GetMagazine()
    {
        return magazine;
    }

    public GameObject GetProjectile()
    {
        return projectile;
    }

    public int Reload()
    {
        int missingAmmo = magazine - currentAmmoInMagazine;
        currentAmmoInMagazine = magazine;
        return missingAmmo;
    }

    public void SetShooter(GameCharacter shooter)
    {
        this.shooter = shooter;
    }

    public void SetFireMode(FireMode fireMode)
    {
        this.fireMode = fireMode;
    }

    public FireMode GetFireMode()
    {
        return fireMode;
    }
}
}}