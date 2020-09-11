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
    private int currentAmmoInMagazine;
    
    public BasicPistol(GameCharacter shooter)
    {
        this.shooter = shooter;
        bulletsMaker = shooter.GetComponent<BulletsMaker>();
        if(bulletsMaker == null)
            bulletsMaker = shooter.gameObject.AddComponent<BulletsMaker>();
    }

    public void ChangeFireMode(FireMode fireMode)
    {
        this.fireMode = fireMode;
    }

    public int Fire(GameCharacter target)
    {
        if(shooter == null) throw new NoShooterSetException();
        if(target == null) throw new NullTargetException();
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
        bulletsMaker.Fire(amoutOfShoots, lookDirection, projectile, shooter, waitingTimeBetwennShots);
        return amoutOfShoots*damage;
    }

    private int FireBurst(GameCharacter target)
    {
        Vector2 lookDirection = target.transform.position - shooter.transform.position;
        int amoutOfShoots = shooter.GetActionPoints()/damage;
        if(amoutOfShoots < burstAmountOfShoots)
        {
            bulletsMaker.Fire(amoutOfShoots, lookDirection, projectile, shooter, waitingTimeBetwennShots);
            return amoutOfShoots*damage;
        }
        else
        {
            bulletsMaker.Fire(burstAmountOfShoots, lookDirection, projectile, shooter, waitingTimeBetwennShots);
            return burstAmountOfShoots*damage;
        }    
    }

    private int FireSemiAutomatic(GameCharacter target)
    {
        Vector2 lookDirection = target.transform.position - shooter.transform.position;
        bulletsMaker.Fire(1, lookDirection, projectile, shooter, waitingTimeBetwennShots);
        return damage;
    }

    public int GetCurrentAmmoInMagazine()
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    public void SetShooter(GameCharacter shooter)
    {
        this.shooter = shooter;
    }

}
}}