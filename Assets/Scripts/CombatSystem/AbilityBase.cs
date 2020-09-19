using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FloppyDiscProjectGreen.CombatSystem;

namespace FloppyDiscProjectGreen
{
namespace Abilites
{
public abstract class AbilityBase : MonoBehaviour
{
    public abstract AbilityType type {get; }
    public abstract string abilityName {get;}
    public abstract float coolDown {get;}
    public abstract Sprite sprite {get; set;}
    public abstract AudioClip sound {get; set;}
    public abstract int actionPointsCost {get; set;}
    public abstract int level {get;}

    public abstract void Init(AbilitesSystem abSystem);
    public abstract void TrigerAbility(GridObject cell);
    public abstract void LevelUp();

    public abstract void VisualizeAbility();

    
}

public enum AbilityType
{
    active,
    flatBounus,
    pasive,
} 

}
}