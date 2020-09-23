using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public interface IRangeWeaponBase
{
    int Fire(GameCharacter target);

    int Reload();

    int GetMagazine();

    int GetDamage();

    int GetFireRate();

    int GetCurrentAmmoInMagazine();

    FireMode GetFireMode();

    GameObject GetProjectile();

    void ChangeFireMode(FireMode @enum);
    void SetShooter(GameCharacter shooter);

    void SetFireMode(FireMode fireMode);

}


public enum FireMode
{
    Automatic,
    Burst,
    Semi_Automatic,
    Custom,    
}

public class NoShooterSetException : System.Exception
{
    public NoShooterSetException(): base() {

    }
}

public class NullTargetException : System.Exception
{
    public NullTargetException(): base() {
        
    }
}

public class NoAmmoInMag : System.Exception
{
    
}
}
}