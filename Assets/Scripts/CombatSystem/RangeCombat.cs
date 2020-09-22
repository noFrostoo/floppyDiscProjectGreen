using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class RangeCombat
{
    public event EventHandler<OnWeponChangeArgs> OnWeponChange;
    public class OnWeponChangeArgs : EventArgs
    {
        public IRangeWeaponBase newWeapon;
        public int newAmmo;
        
    }
    IRangeWeaponBase currentWeapon;
    [SerializeField] int ammuniton;
    GameCharacter shooter;

    public RangeCombat(GameCharacter shooter)
    {
        this.shooter = shooter;
        OnWeponChange += ChangeWeapon_EVENT;
    }

    public void Fire(GameCharacter target)
    {
        if(target == null) return;
        shooter.DecreaseActionPointsThisRound(currentWeapon.Fire(target));
    }
    
    public void Reload()
    {
        ammuniton -= currentWeapon.Reload();
    }
    public IRangeWeaponBase GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void ChangeWeapon_EVENT(object sender, OnWeponChangeArgs e)
    {
        currentWeapon = e.newWeapon;
        if(e.newAmmo > 0)
            ammuniton = e.newAmmo;
    }

    public void ChangeWeapon(IRangeWeaponBase weapon, int newAmmo = -1 )
    {
        if(newAmmo > 0)
            ammuniton = newAmmo;
        currentWeapon = weapon;
    }

}
}
}