using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public interface IRangeWeaponBase
{
    void Fire(GameCharacter target);

    int Reload();

    int GetMagazine();

    int GetDamage();

    int GetFireRate();

    int GetDps();

    int GetCurrentAmmoInMagazine();

    GameObject GetProjectile();

    void ChangeFireMode(FireMode @enum);
}


public enum FireMode
{
    Automatic,
    Burst,
    Semi_Automatic,
    Custom,    
}

}
}