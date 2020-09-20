using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FloppyDiscProjectGreen.CombatSystem;

namespace FloppyDiscProjectGreen
{
namespace Abilites
{
public class AbilitesSystem : MonoBehaviour
{
    [SerializeField] int amountOfActiveAblites;
    [SerializeField] int amountOfPasiveAblites;
    AbilityBase[] activeAbilities;
    AbilityBase[] pasiveAbilites;

    GameCharacter character;

    // Start is called before the first frame update
    void Start()
    {
        activeAbilities = new AbilityBase[amountOfActiveAblites];
        pasiveAbilites = new AbilityBase[amountOfPasiveAblites];
        character = GetComponent<GameCharacter>();

    }

    public void TrigerAbility(GridObject target, AbilitesCode ability)
    {
        int index = (int) ability;
        activeAbilities[index].TrigerAbility(target);
    }

    public void EquipAbility(AbilityBase ability, AbilitesCode code = AbilitesCode.A)
    {
        ability.Init(this);
        if(ability.type == AbilityType.active)
            activeAbilities[(int)code] = ability;
        else if(ability.type == AbilityType.pasive)
            pasiveAbilites[(int)code] = ability;
    }

    public bool CheckAbility(AbilityBase ability)
    {
        if(ability.type == AbilityType.active)
            return CheckActiveAbility(ability);
        else if(ability.type == AbilityType.pasive)
            return CheckPassiveAblity(ability);
        throw new CanNotCheckForFlatBouns();
    }

    private bool CheckPassiveAblity(AbilityBase abilityToCheck)
    {
        foreach(var ability in pasiveAbilites)
            if( ability.name == abilityToCheck.name)
                return false;
        return true;
    }

    private bool CheckActiveAbility(AbilityBase abilityToCheck)
    {
        foreach(var ability in activeAbilities)
            if( ability.name == abilityToCheck.name)
                return false;
        return true;
    }
}

public enum AbilitesCode
{
    A,
    B,
    C,
    D,
    E,
}

public class AbilityAlreadyOnCharacter : System.Exception
{

}

public class CanNotCheckForFlatBouns : System.Exception
{

}

interface Foo 
{
    void lol();
}

}
}
