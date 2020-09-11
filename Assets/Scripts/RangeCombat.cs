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

    // Start is called before the first frame update
    void Start()
    {
        OnWeponChange += ChangeWeapon_EVENT;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(GameObject target)
    {
        currentWeapon.Fire(target.GetComponent<GameCharacter>());
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
        ammuniton = e.newAmmo;
    }

    public void ChangeWeapon(int newAmmo, IRangeWeaponBase weapon)
    {
        ammuniton = newAmmo;
        currentWeapon = weapon;
    }

}
}
}