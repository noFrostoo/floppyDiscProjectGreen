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
    int damage = 10;
    int fireRate = 20;
    int magazine = 9;
    int burstAmountOfShoots = 3;
    int waitingTimeBetwennShots; 
    int currentAmmoInMagazine;
    
    int chanceOfHit = 0;
    public BasicPistol(GameCharacter shooter)
    {
        this.shooter = shooter;
        bulletsMaker = shooter.GetComponent<BulletsMaker>();
        if(bulletsMaker == null)
            bulletsMaker = shooter.gameObject.AddComponent<BulletsMaker>();
        projectile = Resources.Load("Prefabs/Projectile") as GameObject;
        projectile.GetComponent<Bullet>().SetInfomationForBullet(damage, chanceOfHit);
        waitingTimeBetwennShots = fireRate/60;
        currentAmmoInMagazine = 9;
    }

    public void ChangeFireMode(FireMode fireMode)
    {
        this.fireMode = fireMode;
    }

    public int Fire(GameCharacter target)
    {
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
        int amoutOfShoots = shooter.GetActionPoints()/damage;
        if(amoutOfShoots < currentAmmoInMagazine)
            amoutOfShoots = currentAmmoInMagazine;
        currentAmmoInMagazine -= amoutOfShoots;
        shooter.StartShooting(amoutOfShoots, lookDirection, projectile, waitingTimeBetwennShots);
        return amoutOfShoots*damage;
    }

    private int FireBurst(GameCharacter target)
    {
        Vector2 lookDirection = target.transform.position - shooter.transform.position;
        int amoutOfShoots = shooter.GetActionPoints()/damage;
        if(amoutOfShoots < currentAmmoInMagazine)
            amoutOfShoots = currentAmmoInMagazine;
        if(amoutOfShoots < burstAmountOfShoots)
        {
            currentAmmoInMagazine -= amoutOfShoots;
            shooter.StartShooting(amoutOfShoots, lookDirection, projectile, waitingTimeBetwennShots);
            return amoutOfShoots*damage;
        }
        else
        {
            currentAmmoInMagazine -= burstAmountOfShoots;
            shooter.StartShooting(burstAmountOfShoots, lookDirection, projectile, waitingTimeBetwennShots);
            return burstAmountOfShoots*damage;
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

}
}}